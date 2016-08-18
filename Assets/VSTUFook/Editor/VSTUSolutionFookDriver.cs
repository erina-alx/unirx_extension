using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class VSTUSolutionFookDriver
    {
        static VSTUSolutionFookDriver()
        {
            var solutionFooks = FindSolutionFooks();

            ProjectFilesGenerator.SolutionFileGeneration = (filename, content) =>
            {
                return solutionFooks.Aggregate(
                    content,
                    (current, fook) => fook.Fook(current)
                    );
            };
        }

        private static IEnumerable<SolutionFookAdapter> FindSolutionFooks()
        {
            return typeof(VSTUProjectFookDriver).Assembly.GetTypes()
                .Select(t => new {Type = t, Attribute = GetSolutionFookAttribute(t)})
                .Where(e => e.Attribute != null)
                .Where(e => HasSolutionFookInterface(e.Type))
                .Where(e => !e.Type.IsAbstract)
                .Select(e => SolutionFookAdapter.Create(e.Attribute, e.Type))
                .OrderByDescending(fook => fook.Priority)
                .ToArray();
        }

        private static VSTUSolutionFookAttribute GetSolutionFookAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(VSTUSolutionFookAttribute), inherit: true);

            return attributes.Length > 0 ? (VSTUSolutionFookAttribute) attributes[0] : null;
        }

        private static bool HasSolutionFookInterface(Type type)
        {
            return type.GetInterface(typeof(IVSTUSolutionFook).FullName) != null;
        }

        private class SolutionFookAdapter :
            IVSTUSolutionFook
        {
            private readonly int priority;
            private readonly IVSTUSolutionFook fookInstance;

            public static SolutionFookAdapter Create(VSTUSolutionFookAttribute attribute, Type type)
            {
                if (attribute == null)
                    throw new ArgumentNullException("attribute");
                if (type == null)
                    throw new ArgumentNullException("type");

                var constructor = type.GetConstructor(new Type[] {});

                if (constructor == null)
                    throw new ArgumentException(String.Format("Default constructor is not found (in '{0}').", type.FullName));

                return new SolutionFookAdapter(
                    attribute.Priority,
                    constructor.Invoke(new object[] {}) as IVSTUSolutionFook
                    );
            }

            private SolutionFookAdapter(
                int priority,
                IVSTUSolutionFook fookInstance
                )
            {
                if (fookInstance == null)
                    throw new ArgumentNullException("fookInstance");

                this.priority = priority;
                this.fookInstance = fookInstance;
            }

            public string Fook(string document)
            {
                return fookInstance.Fook(document);
            }

            public int Priority
            {
                get { return priority; }
            }
        }
    }
}
