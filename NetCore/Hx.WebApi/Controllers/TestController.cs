using DotNetCore.CAP;
using Hx.EventBus;
using Hx.Sqlsugar;
using Hx.WebApi.Models;
using Hx.WebApi.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hx.WebApi.Controllers
{
    public class TestController: BaseAdminController
    {
        private IEventPublisher _eventPublisher;
        private TestOptions _testOptions;
        /// <summary>
        /// 控制器
        /// </summary>
        /// <param name="eventPublisher"></param>
        public TestController(IOptions<TestOptions> options,
            IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _testOptions = options.Value;
        }

        [HttpPost]
        public string PublishTest(BasePush basePush)
        {
            _eventPublisher.PublishAsync("Hx.Cap.Test", basePush);
            return "ok";
        }

        [HttpPost]
        [CapSubscribe("Hx.Cap.Test")]
        public string SubscribeTest(BasePush basePush)
        {
            //_capPublisher.Publish("Hx.Cap.Test", basePush);
            return "ok";
        }
    }
}
