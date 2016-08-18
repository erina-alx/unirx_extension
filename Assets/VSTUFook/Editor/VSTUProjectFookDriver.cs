using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class VSTUProjectFookDriver
    {
        static VSTUProjectFookDriver()
        {
            var projectFooks = FindProjectFooks();

            ProjectFilesGenerator.ProjectFileGeneration = (fileName, content) =>
            {
                var projectKinds = GetProjectKindFromFileName(fileName);

                if (projectKinds == VSTUProjectKinds.None)
                    return content;

                var document = projectFooks
                    .Where(f => (f.TargetProjects & projectKinds) == projectKinds)
                    .Aggregate(
                        XDocument.Parse(content),
                        (current, fook) => fook.Fook(current)
                    );

                return document.Declaration + Environment.NewLine + document.Root;
            };
        }

        private static IEnumerable<ProjectFookAdapter> FindProjectFooks()
        {
            return typeof(VSTUProjectFookDriver).Assembly.GetTypes()
                .Select(t => new {Type = t, Attribute = GetProjectFookAttribute(t)})
                .Where(e => e.Attribute != null)
                .Where(e => HasProjectFookInterface(e.Type))
                .Where(e => !e.Type.IsAbstract)
                .Select(e => ProjectFookAdapter.Create(e.Attribute, e.Type))
                .OrderByDescending(fook => fook.Priority)
                .ToArray();
        }

        private static VSTUProjectFookAttribute GetProjectFookAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(VSTUProjectFookAttribute), inherit: true);

            return attributes.Length > 0 ? (VSTUProjectFookAttribute) attributes[0] : null;
        }

        private static bool HasProjectFookInterface(Type type)
        {
            return type.GetInterface(typeof(IVSTUProjectFook).FullName) != null;
        }

        private static VSTUProjectKinds GetProjectKindFromFileName(string fileName)
        {
            if (fileName.EndsWith(".CSharp.Plugins.csproj"))
                return VSTUProjectKinds.Plugins;

            if (fileName.EndsWith(".CSharp.Editor.csproj"))
                return VSTUProjectKinds.Editor;

            if (fileName.EndsWith(".CSharp.csproj"))
                return VSTUProjectKinds.Application;

            return VSTUProjectKinds.None;
        }

        private class ProjectFookAdapter :
            IVSTUProjectFook
        {
            private readonly VSTUProjectKinds targetProjects;
            private readonly int priority;
            private readonly IVSTUProjectFook fookInstance;

            public static ProjectFookAdapter Create(VSTUProjectFookAttribute attribute, Type type)
            {
                if (attribute == null)
                    throw new ArgumentNullException("attribute");
                if (type == null)
                    throw new ArgumentNullException("type");

                var constructor = type.GetConstructor(new Type[] {});

                if (constructor == null)
                    throw new ArgumentException(String.Format("Default constructor is not found (in '{0}').", type.FullName));

                return new ProjectFookAdapter(
                    attribute.ProjectKinds,
                    attribute.Priority,
                    constructor.Invoke(new object[] {}) as IVSTUProjectFook
                    );
            }

            private ProjectFookAdapter(
                VSTUProjectKinds targetProjects,
                int priority,
                IVSTUProjectFook fookInstance
                )
            {
                if (fookInstance == null)
                    throw new ArgumentNullException("fookInstance");

                this.targetProjects = targetProjects;
                this.priority = priority;
                this.fookInstance = fookInstance;
            }

            public XDocument Fook(XDocument document)
            {
                return fookInstance.Fook(document);
            }

            public VSTUProjectKinds TargetProjects
            {
                get { return targetProjects; }
            }

            public int Priority
            {
                get { return priority; }
            }
        }
    }
}
