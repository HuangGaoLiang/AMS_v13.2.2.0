using System.Collections.Generic;

namespace AMS.Core
{
    public delegate bool EqualsComparer<T>(T x, T y);

    /// <summary>
    /// 集合去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Compare<T> : IEqualityComparer<T>
    {
        private EqualsComparer<T> _equalsComparer;

        public Compare(EqualsComparer<T> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }

        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
                return this._equalsComparer(x, y);
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
