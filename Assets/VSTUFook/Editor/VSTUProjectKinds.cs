using System;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    [Flags]
    public enum VSTUProjectKinds
    {
        None = 0x0,
        Application = 0x1,
        Editor = 0x2,
        Plugins = 0x4,
        All = Application | Editor | Plugins,
    }
}
