using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EventBus
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseDomainEvent
    {
        /// <summary>
        /// 事件基类
        /// </summary>
        public BaseDomainEvent()
        {
            EventId = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 事件基类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        public BaseDomainEvent(string id, DateTime time)
        {
            EventId = id;
            CreateTime = time;
        }
        /// <summary>
        /// 事件id
        /// </summary>
        public string EventId { get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; }
    }
}
