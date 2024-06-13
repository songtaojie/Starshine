using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.EventBus
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseDomainEvent
    {
        public BaseDomainEvent()
        {
            EventId = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }

        public BaseDomainEvent(string id, DateTime time)
        {
            EventId = id;
            CreateTime = time;
        }

        public string EventId { get; }

        public DateTime CreateTime { get; }
    }
}
