using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    public static class CreateCouponNo
    {
        /// <summary>
        /// 生成优惠券号
        /// </summary>
        /// <returns></returns>
        public static string GetCouponCode()
        {
            return GenerateRandomCode(12);
        }

        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }
    }
}
