using System;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class)]
    public class VSTUSolutionFookAttribute :
        Attribute
    {
        private readonly int priority;

        public VSTUSolutionFookAttribute(int priority)
        {
            this.priority = priority;
        }

        public VSTUSolutionFookAttribute() :
            this(priority: 0)
        {
        }

        public int Priority
        {
            get { return priority; }
        }
    }
}
