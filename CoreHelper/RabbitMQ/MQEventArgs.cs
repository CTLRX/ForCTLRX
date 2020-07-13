using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
    /// <summary>
	/// 监听消息事件参数
	/// </summary>
	public class MQEventArgs<T> : EventArgs
    {
        public T Data
        {
            get;
            private set;
        }
        public MQEventArgs()
        {
            this.Data = default(T);
        }
        public MQEventArgs(T data)
        {
            this.Data = data;
        }
    }
}
