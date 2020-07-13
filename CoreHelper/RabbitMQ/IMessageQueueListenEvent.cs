using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
  public  interface  IMessageQueueListenEvent<T>
    {
        /// <summary>
		/// 消息到达事件
		/// </summary>
		event MessageRecievedHandle<T> OnMessageRecieved;
    }
}
