using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
   public  class TestUse
    {
        public static string QueueName = "user";
        public TestUse(int id,string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
