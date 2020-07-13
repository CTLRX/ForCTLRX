using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.RabbitMQ
{
    public enum HandlerStatus
    {
        Ack = 1,
        Nack,
        Record
    }
}
