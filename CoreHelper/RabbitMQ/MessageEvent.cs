using Core.ConvertHelper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
    public class MessageEvent<T> : IMessageQueueListenEvent<T>
    {
        public static Dictionary<string, int> dic = new Dictionary<string, int>();
        public event MessageRecievedHandle<T> OnMessageRecieved;
        public MessageEvent(string queueName, bool isTopic = false, bool isRemoveIdentity = false)
        {
            if (isTopic)
            {
                this.ListenTopic(queueName, isRemoveIdentity);
                return;
            }
            this.ListenQueue(queueName);
        }
        public void ListenQueue(string queueName)
        {
            string exchange = "exchange." + queueName + ".delay";
            IConnection connection = RabbitPool.CreateRealConnection();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.BasicQos(0u, 1, false);
            channel.ExchangeDeclare(exchange, "direct", true, false, null);
            channel.QueueBind(queueName, exchange, "routing-delay", null);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += delegate (object sender, BasicDeliverEventArgs e)
            {
                string @string = Encoding.UTF8.GetString(e.Body);
                this.Handler(queueName, e.DeliveryTag, channel, @string);
            };
            channel.BasicConsume(queueName, false, eventingBasicConsumer);
        }
        public void ListenTopic(string queueName, bool isRemoveIdentity = false)
        {
            string queueName2 = queueName;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Rabbit_TopicIdentity"]) && !isRemoveIdentity)
            {
                queueName = queueName2 + "." + ConfigurationManager.AppSettings["Rabbit_TopicIdentity"].ToLower();
            }
            else
            {
                string expr_75 = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
                {
                    '\\'
                });
                string str = expr_75.Substring(expr_75.LastIndexOf('\\') + 1);
                queueName = queueName2 + "." + str;
            }
            IConnection connection = RabbitPool.CreateRealConnection();
            IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(queueName2, "fanout", true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, queueName2, "", null);
            channel.BasicQos(0u, 1, false);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += delegate (object sender, BasicDeliverEventArgs e)
            {
                string @string = Encoding.UTF8.GetString(e.Body);
                this.Handler(queueName, e.DeliveryTag, channel, @string);
            };
            channel.BasicConsume(queueName, false, eventingBasicConsumer);
        }
        private void Handler(string queue, ulong deliveryTag, IModel channel, string message)
        {
            Exception ex = null;
            HandlerStatus handlerStatus = HandlerStatus.Ack;
            T t = default(T);
            try
            {
                t = JsonConvert.JsonToObject<T>(message, false, false);
                MQEventArgs<T> mQEventArgs = new MQEventArgs<T>(t);
                this.OnMessageRecieved.Invoke(this, mQEventArgs);
            }
            catch (Exception)
            {
                if (MessageEvent<T>.dic.ContainsKey(queue))
                {
                    Dictionary<string, int> expr_45 = MessageEvent<T>.dic;
                    int num = expr_45[queue];
                    expr_45[queue] = num + 1;
                    if (MessageEvent<T>.dic[queue] < 5)
                    {
                        handlerStatus = HandlerStatus.Nack;
                    }
                    else
                    {
                        handlerStatus = HandlerStatus.Record;
                    }
                }
                else
                {
                    MessageEvent<T>.dic.Add(queue, 1);
                    handlerStatus = HandlerStatus.Nack;
                }
            }
            finally
            {
                switch (handlerStatus)
                {
                    case HandlerStatus.Ack:
                        channel.BasicAck(deliveryTag, false);
                        break;
                    case HandlerStatus.Nack:
                        channel.BasicNack(deliveryTag, false, true);
                        break;
                    case HandlerStatus.Record:
                        //OmiRecord.Insert<MQRecord>(new MQRecord(queue, ConfigurationManager.AppSettings["Rabbit_TopicIdentity"], ex.ToString(), t), null);
                        //channel.BasicAck(deliveryTag, false);
                        break;
                }
            }
        }
    }
}
