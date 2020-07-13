using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
    internal class RabbitConnection : IDisposable
    {
        private static object objLock = new object();
        private IConnection _connection;
        public UseStatus UserStatus
        {
            get;
            private set;
        }
        public RabbitConnection(IConnection connection)
        {
            this._connection = connection;
            this.UserStatus = UseStatus.NotUse;
        }
        public IConnection Open()
        {
            this.UserStatus = UseStatus.Use;
            return this._connection;
        }
        public void Dispose()
        {
            this.UserStatus = UseStatus.NotUse;
        }
    }
}
