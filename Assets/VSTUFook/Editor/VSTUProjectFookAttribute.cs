using System;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class)]
    public class VSTUProjectFookAttribute :
        Attribute
    {
        private readonly VSTUProjectKinds projectKinds;
        private readonly int priority;

        public VSTUProjectFookAttribute(VSTUProjectKinds projectKinds, int priority)
        {
            this.projectKinds = projectKinds;
            this.priority = priority;
        }

        public VSTUProjectFookAttribute(VSTUProjectKinds projectKinds) :
            this(projectKinds, priority: 0)
        {
        }

        public VSTUProjectKinds ProjectKinds
        {
            get { return projectKinds; }
        }

        public int Priority
        {
            get { return priority; }
        }
    }
}
