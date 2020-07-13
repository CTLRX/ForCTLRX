using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CoreHelper.RabbitMQ
{
    internal class RabbitPool
    {
        public static readonly RabbitPool rabbitPool = new RabbitPool();
  //      public static Func<RabbitConnection, bool> _9__12_0;
		//public static Func<RabbitConnection, bool> _9__14_0;
		//public static Func<RabbitConnection, bool> _9__14_1;
        private static ConnectionFactory factory;
        private static List<RabbitConnection> connections;
        private static List<RabbitConnection> autoConnections;
        private static object objLock;
        private static int maxPoolSize;
        private static string _addresss;
        private static string _username;
        private static string _pwd;
        private static string _port;
        private static System.Timers.Timer timer;
        internal static int t;
        static RabbitPool()
        {
            RabbitPool.factory = new ConnectionFactory();
            RabbitPool.connections = new List<RabbitConnection>();
            RabbitPool.autoConnections = new List<RabbitConnection>();
            RabbitPool.objLock = new object();
            RabbitPool.maxPoolSize = 10;
            RabbitPool._addresss = string.Empty;
            RabbitPool._username = string.Empty;
            RabbitPool._pwd = string.Empty;
            RabbitPool._port = string.Empty;
            RabbitPool.timer = new System.Timers.Timer(60000.0);
            RabbitPool.t = 0;
            string text = ConfigurationManager.AppSettings["Rabbit_MaxPoolSize"];
            if (!string.IsNullOrEmpty(text))
            {
                int num = 0;
                if (int.TryParse(text, out num) && num > 0)
                {
                    RabbitPool.maxPoolSize = num;
                }
            }
            RabbitPool._addresss = ConfigurationManager.AppSettings["MQServer"];
            if (string.IsNullOrEmpty(RabbitPool._addresss))
            {
                throw new Exception("未指定MQ服务器");
            }
            RabbitPool._username = ConfigurationManager.AppSettings["RabbitUserName"];
            if (string.IsNullOrEmpty(RabbitPool._username))
            {
                throw new Exception("未指定MQ服务器帐号");
            }
            RabbitPool._pwd = ConfigurationManager.AppSettings["RabbitPassword"];
            if (string.IsNullOrEmpty(RabbitPool._pwd))
            {
                throw new Exception("未指定MQ服务器密码");
            }
            RabbitPool._port = ConfigurationManager.AppSettings["RabbitPort"];
            if (RabbitPool._addresss.IndexOf(':') > 0)
            {
                RabbitPool.factory.HostName = RabbitPool._addresss.Substring(0, RabbitPool._addresss.IndexOf(':'));
                RabbitPool.factory.Port = int.Parse(RabbitPool._addresss.Substring(RabbitPool._addresss.IndexOf(':') + 1));
            }
            else
            {
                RabbitPool.factory.HostName = RabbitPool._addresss;
            }
            RabbitPool.factory.UserName = RabbitPool._username;
            RabbitPool.factory.Password = RabbitPool._pwd;
            if (!string.IsNullOrEmpty(RabbitPool._port))
            {
                RabbitPool.factory.Port = int.Parse(RabbitPool._port);
            }
            RabbitPool.factory.AutomaticRecoveryEnabled = true;
            RabbitPool.InitPool();
           // RabbitPool.timer.Elapsed += new ElapsedEventHandler(RabbitPool.Timer_Elapsed);
            RabbitPool.timer.AutoReset = true;
            RabbitPool.timer.Enabled = true;
        }
        //private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    object obj = RabbitPool.objLock;
        //    lock (obj)
        //    {
        //        IEnumerable<RabbitConnection> arg_34_0 = RabbitPool.autoConnections;
        //        Func<RabbitConnection, bool> arg_34_1;
        //        if ((arg_34_1 = RabbitPool._9__12_0) == null)
        //        {
        //            arg_34_1 = (RabbitPool._9__12_0 = new Func<RabbitConnection, bool>(RabbitPool._c.<> 9.< Timer_Elapsed > b__12_0));
        //        }
        //        List<RabbitConnection> list = arg_34_0.Where(arg_34_1).ToList<RabbitConnection>();
        //        for (int i = 0; i < list.Count<RabbitConnection>(); i++)
        //        {
        //            list[i].Dispose();
        //            list[i].Open().Dispose();
        //            RabbitPool.autoConnections.Remove(list[i]);
        //        }
        //    }
        //}
        private static void InitPool()
        {
            for (int i = 0; i <= RabbitPool.maxPoolSize; i++)
            {
                RabbitConnection item = new RabbitConnection(RabbitPool.factory.CreateConnection());
                RabbitPool.connections.Add(item);
            }
        }
        internal static void IncreasePool()
        {
            RabbitPool.autoConnections.Add(new RabbitConnection(RabbitPool.factory.CreateConnection()));
        }
        internal static IConnection CreateRealConnection()
        {
            return RabbitPool.factory.CreateConnection();
        }
    }
}
