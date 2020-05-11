using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// Returns -1 instead of 1 if y is IsNullOrEmpty when x is Not.
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-18</para>
    /// </summary>
    public class EmptyStringsAreLast : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (String.IsNullOrEmpty(y) && !String.IsNullOrEmpty(x))
            {
                return -1;
            }
            else if (!String.IsNullOrEmpty(y) && String.IsNullOrEmpty(x))
            {
                return 1;
            }
            else
            {
                return String.Compare(x, y);
            }
        }
    }
}
