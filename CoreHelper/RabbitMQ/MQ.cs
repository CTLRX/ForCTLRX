using Core.ConvertHelper;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
   public  static class MQ
    {
        private static Dictionary<string, object> QueueListenerCollenctions;
        static MQ()
        {
            MQ.QueueListenerCollenctions = new Dictionary<string, object>();
            RabbitPool.t = 1;
        }
        public static IMessageQueueListenEvent<T> ListenQueue<T>(string queueName)
        {
            return Listen<T>(queueName, false, false);
        }
        public static  IMessageQueueListenEvent<T> ListenTopic<T>(string queueName, bool isRemoveIdentity = false)
        {
            string topicName = GetTopicName(queueName);
            return Listen<T>(topicName, true, isRemoveIdentity);
        }
        private static IMessageQueueListenEvent<T> Listen<T>(string queueName, bool isTopic, bool isRemoveIdentity = false)
        {
            string text = queueName.ToLower();
            if (!MQ.QueueListenerCollenctions.ContainsKey(text))
            {
                string obj = string.Format("{0}_{1}", "MessageQueueEventLocker", text);
                lock (obj)
                {
                    if (!MQ.QueueListenerCollenctions.ContainsKey(text))
                    {
                        IMessageQueueListenEvent<T> value = new MessageEvent<T>(text, isTopic, isRemoveIdentity);
                        MQ.QueueListenerCollenctions.Add(text, value);
                    }
                }
            }
            return MQ.QueueListenerCollenctions[text] as IMessageQueueListenEvent<T>;
        }
        public static void PublishTopic(string queueName, List<object> messages)
        {
            string topicName = GetTopicName(queueName);
            using (IConnection rabbitConnection = RabbitPool.CreateRealConnection())
            {
                using (IModel model = rabbitConnection.CreateModel())
                {
                    model.ExchangeDeclare(topicName, "fanout", true, false, null);
                    IBasicProperties basicProperties = model.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    basicProperties.Persistent = true;
                    using (List<object>.Enumerator enumerator = messages.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            string s = JsonConvert.ObjectToJson(enumerator.Current, false, false, "yyyy-MM-dd HH:mm:ss");
                            byte[] bytes = Encoding.UTF8.GetBytes(s);
                            model.BasicPublish(topicName, "", basicProperties, bytes);
                        }
                    }
                }
            }
        }
        public static void PublishTopic(string queueName, object messages)
        {
            string topicName = GetTopicName(queueName);
            using (IConnection rabbitConnection = RabbitPool.CreateRealConnection())
            {
                using (IModel model = rabbitConnection.CreateModel())
                {
                    model.ExchangeDeclare(topicName, "fanout", true, false, null);
                    IBasicProperties basicProperties = model.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    basicProperties.Persistent = true;
                    string s = JsonConvert.ObjectToJson(messages, false, false, "yyyy-MM-dd HH:mm:ss");
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    model.BasicPublish(topicName, "", basicProperties, bytes);

                }
            }
        }
        public static void SendMessage(string queueName, object messages, long timeDelay = 0L)
        {
            using (IConnection rabbitConnection = RabbitPool.CreateRealConnection())
            {
                using (IModel model = rabbitConnection.CreateModel())
                {
                    IBasicProperties basicProperties = model.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    basicProperties.Persistent = true;
                    if (timeDelay > 0L)
                    {
                        string value = "exchange." + queueName + ".delay";
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        basicProperties.Expiration = timeDelay.ToString();
                        dictionary.Add("x-dead-letter-exchange", value);
                        dictionary.Add("x-dead-letter-routing-key", "routing-delay");
                        model.QueueDeclare(queueName + ".delay", true, false, false, dictionary);
                        string s = JsonConvert.ObjectToJson(messages, false, false, "yyyy-MM-dd HH:mm:ss");
                        byte[] bytes = Encoding.UTF8.GetBytes(s);
                        model.BasicPublish("", queueName + ".delay", basicProperties, bytes);
                        return;
                         
                    }
                    model.QueueDeclare(queueName, true, false, false, null);           
                    string s2 = JsonConvert.ObjectToJson(messages, false, false, "yyyy-MM-dd HH:mm:ss");
                    byte[] bytes2 = Encoding.UTF8.GetBytes(s2);
                    model.BasicPublish("", queueName, basicProperties, bytes2);
                }
            }
        }
        public static void SendMessage(string queueName, List<object> messages, long timeDelay = 0L)
        {
            using (IConnection rabbitConnection = RabbitPool.CreateRealConnection())
            {
                using (IModel model = rabbitConnection.CreateModel())
                {
                    IBasicProperties basicProperties = model.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    basicProperties.Persistent = true;
                    if (timeDelay > 0L)
                    {
                        string value = "exchange." + queueName + ".delay";
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        basicProperties.Expiration = timeDelay.ToString();
                        dictionary.Add("x-dead-letter-exchange", value);
                        dictionary.Add("x-dead-letter-routing-key", "routing-delay");
                        model.QueueDeclare(queueName + ".delay", true, false, false, dictionary);
                        using (List<object>.Enumerator enumerator = messages.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                string s = JsonConvert.ObjectToJson(enumerator.Current, false, false, "yyyy-MM-dd HH:mm:ss");
                                byte[] bytes = Encoding.UTF8.GetBytes(s);
                                model.BasicPublish("", queueName + ".delay", basicProperties, bytes);
                            }
                            return;
                        }
                    }
                    model.QueueDeclare(queueName, true, false, false, null);
                    using (List<object>.Enumerator enumerator = messages.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            string s2 = JsonConvert.ObjectToJson(enumerator.Current, false, false, "yyyy-MM-dd HH:mm:ss");
                            byte[] bytes2 = Encoding.UTF8.GetBytes(s2);
                            model.BasicPublish("", queueName, basicProperties, bytes2);
                        }
                    }
                }
            }
        }
        private static string GetTopicName(string queueName)
        {
            return "topic." + queueName;
        }
    }
}
