using System;

namespace MovieTime {
    [Flags]
    public enum NativeWindowStyle : uint {
        WS_VISIBLE = 0x10000000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x02000000,
    }
}
