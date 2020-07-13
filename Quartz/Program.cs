using Core.Helper;
using Omipay.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Info("收到汇款单已退款通知",null, "收到汇款单已退款通知");
            QuartzHelper.ExecuteAtTime(ForeignExchange, "*/55 * * * * ?");
        }
        public static void ForeignExchange(Dictionary<string, object> paramsters)
        {

            try
            {
                Console.WriteLine("Hello World!");
            }
            catch (Exception ex)
            {

            }

        }
    }
}
