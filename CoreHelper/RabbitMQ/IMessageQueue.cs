using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
    public interface IMessageQueue : IStrategy
    {
        /// <summary>
        /// 批量向消息队列发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="messages">消息集合</param>
        /// <param name="timeDelay">消息延时(毫秒)</param>
        void SendMessage(string queueName, List<object> messages, long timeDelay = 0L);
        /// <summary>
        /// 批量发布队列订阅主题
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息集合</param>
        void PublishTopic(string queueName, List<object> messages);
        /// <summary>
        /// 向消息队列发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="messages">消息集合</param>
        /// <param name="timeDelay">消息延时(毫秒)</param>
        void SendMessage(string queueName, object messages, long timeDelay = 0L);
        /// <summary>
        /// 发布队列订阅主题
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息集合</param>
        void PublishTopic(string queueName, object messages);
        /// <summary>
        /// 监听对列消息
        /// </summary>
        /// <param name="queueName">对列名称</param>
        /// <returns></returns>
        IMessageQueueListenEvent<T> ListenQueue<T>(string queueName);
        /// <summary>
        /// 监听订阅消息
        /// </summary>
        /// <param name="queueName">对列名称</param>
        /// <returns></returns>
        IMessageQueueListenEvent<T> ListenTopic<T>(string queueName, bool isRemoveIdentity = false);
    }
}
