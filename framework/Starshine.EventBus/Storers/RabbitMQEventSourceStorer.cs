﻿//using RabbitMQ.Client.Events;
//using RabbitMQ.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Channels;
//using System.Threading;
//using System.Threading.Tasks;
//using Starshine.EventBus.RabbitMq;

//namespace Starshine.EventBus.Storers
//{
//    public sealed class RabbitMQEventSourceStorer : IEventSourceStorer, IDisposable
//    {
//        /// <summary>
//        /// 内存通道事件源存储器
//        /// </summary>
//        private readonly Channel<IEventSource> _channel;

//        /// <summary>
//        /// 通道对象
//        /// </summary>
//        private readonly IModel _model;

//        /// <summary>
//        /// 连接对象
//        /// </summary>
//        private readonly IConnection _connection;

//        /// <summary>
//        /// 路由键
//        /// </summary>
//        private readonly string _routeKey;

//        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="factory">连接工厂</param>
//        /// <param name="routeKey">路由键</param>
//        /// <param name="capacity">存储器最多能够处理多少消息，超过该容量进入等待写入</param>
//        public RabbitMQEventSourceStorer(ConnectionFactory factory, string routeKey, int capacity, IRabbitMQPersistentConnection rabbitMQPersistentConnection)
//        {
//            // 配置通道，设置超出默认容量后进入等待
//            var boundedChannelOptions = new BoundedChannelOptions(capacity)
//            {
//                FullMode = BoundedChannelFullMode.Wait
//            };

//            // 创建有限容量通道
//            _channel = Channel.CreateBounded<IEventSource>(boundedChannelOptions);

//            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;


//            if (!_rabbitMQPersistentConnection.IsConnected)
//            {
//                _rabbitMQPersistentConnection.TryConnect();
//            }

//            // 创建通道
//            _model = _rabbitMQPersistentConnection.CreateModel();


//            //声明交换机
//            _model.ExchangeDeclare(Exchange_Name, ExchangeType.Direct);
//            //声明一个死信队列的交换机
//            _model.ExchangeDeclare(DlxExchange_Name, ExchangeType.Direct);
//            _model.QueueDeclare(Queue_Name, true, false, false, null);
//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += async (model, ea) =>
//            {
//                var message = Encoding.UTF8.GetString(ea.Body);

//                await ProcessEvent(ea.RoutingKey, message);

//                channel.BasicAck(ea.DeliveryTag, multiple: false);
//            };

//            //channel.BasicConsume(Queue_Name, false, consumer);
//            _model.CallbackException += (sender, ea) =>
//            {
//                _consumerChannel.Dispose();
//                _consumerChannel = CreateConsumerChannel();
//            };






//            // 创建连接
//            _connection = factory.CreateConnection();
//            _routeKey = routeKey;

//            // 创建通道
//            _model = _connection.CreateModel();

//            // 声明路由队列
//            _model.QueueDeclare(routeKey, false, false, false, null);

//            // 创建消息订阅者
//            var consumer = new EventingBasicConsumer(_model);

//            // 订阅消息并写入内存 Channel
//            consumer.Received += (ch, ea) =>
//            {
//                // 读取原始消息
//                var stringEventSource = Encoding.UTF8.GetString(ea.Body.ToArray());

//                // 转换为 IEventSource，这里可以选择自己喜欢的序列化工具，如果自定义了 EventSource，注意属性是可读可写
//                var eventSource = JsonSerializer.Deserialize<ChannelEventSource>(stringEventSource);

//                // 写入内存管道存储器
//                _channel.Writer.WriteAsync(eventSource);

//                // 确认该消息已被消费
//                _model.BasicAck(ea.DeliveryTag, false);
//            };

//            // 启动消费者 设置为手动应答消息
//            _model.BasicConsume(routeKey, false, consumer);
//        }

//        /// <summary>
//        /// 将事件源写入存储器
//        /// </summary>
//        /// <param name="eventSource">事件源对象</param>
//        /// <param name="cancellationToken">取消任务 Token</param>
//        /// <returns><see cref="ValueTask"/></returns>
//        public async ValueTask WriteAsync(IEventSource eventSource, CancellationToken cancellationToken)
//        {
//            // 空检查
//            if (eventSource == default)
//            {
//                throw new ArgumentNullException(nameof(eventSource));
//            }

//            // 这里判断是否是 ChannelEventSource 或者 自定义的 EventSource
//            if (eventSource is ChannelEventSource source)
//            {
//                // 序列化，这里可以选择自己喜欢的序列化工具
//                var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source));

//                // 发布
//                _model.BasicPublish("", _routeKey, null, data);
//            }
//            else
//            {
//                // 这里处理动态订阅问题
//                await _channel.Writer.WriteAsync(eventSource, cancellationToken);
//            }
//        }

//        /// <summary>
//        /// 从存储器中读取一条事件源
//        /// </summary>
//        /// <param name="cancellationToken">取消任务 Token</param>
//        /// <returns>事件源对象</returns>
//        public async ValueTask<IEventSource> ReadAsync(CancellationToken cancellationToken)
//        {
//            // 读取一条事件源
//            var eventSource = await _channel.Reader.ReadAsync(cancellationToken);
//            return eventSource;
//        }

//        /// <summary>
//        /// 释放非托管资源
//        /// </summary>
//        public void Dispose()
//        {
//            _model?.Dispose();
//            _connection?.Dispose();
//        }
//    }
//}
