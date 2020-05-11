using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace AMS.Core
{
    /// <summary>
    /// 自然排序
    /// </summary>
    public sealed class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            return SafeNativeMethods.StrCmpLogicalW(left, right);
        }

        /// <summary>
        /// 加载window底层shlwapi.dll
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
        }
    }
}
