using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omipay.Core
{
    public static class MathHelper
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        public static decimal ToRound(this decimal d, int decimals)
        {
            return System.Math.Round(d, decimals); 
        }
    }
}
