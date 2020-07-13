using CoreHelper.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
   public class Test
    {
        public static void Sync(object sender, MQEventArgs<TestUse> args)
        {
            
            Console.WriteLine(args.Data.Name);
           
        }
    }
}
