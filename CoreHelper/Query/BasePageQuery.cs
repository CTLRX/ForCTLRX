using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omipay.Core
{
    public class BasePageQuery
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page_index { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int page_size { get; set; } 
    }
}
