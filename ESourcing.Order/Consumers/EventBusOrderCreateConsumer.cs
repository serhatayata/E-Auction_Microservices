using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events.Concrete;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands.OrderCreate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Order.Consumers
{
    public class EventBusOrderCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _persistenConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection persistenConnection, IMediator mediator, IMapper mapper)
        {   //Generate Constructor null check ile
            _persistenConnection = persistenConnection ?? throw new ArgumentNullException(nameof(persistenConnection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //
        public void Consume()
        {
            if (!_persistenConnection.IsConnected)
            {
                _persistenConnection.TryConnect();
            }

            var channel = _persistenConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel); //queue consume edebilmemizi sağlayan nesne oluşur.
            //Custom her consume ettiğim mesaj için, oluşturduğum custom oluşturduğumuz event metoduna git diye yeni bir custom event metot geliştirdik.
            consumer.Received += ReceivedEvent;
            //Queue consume başlatma
            channel.BasicConsume(queue:EventBusConstants.OrderCreateQueue,autoAck:true,consumer:consumer);


        }
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span); //mesaja ulaşıyoruz
            var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message); // Sourcingten gelen Mesaj OrderCreateEvent'i Deserialize ederiz.
            if (e.RoutingKey == EventBusConstants.OrderCreateQueue) 
            {
                //OrderCreateEvent olarak geldi ancak biz bunu Mediator üzerinden OrderCreateHandler'a göndereceğiz.
                var command = _mapper.Map<OrderCreateCommand>(@event);
                command.CreatedAt = DateTime.Now;
                command.TotalPrice = @event.Quantity * @event.Price; //TotalPrice hesaplama
                command.UnitPrice = @event.Price;
                //Mediator altyapısı ile geliştirdiğimiz veritabanına insert edeceğiz.
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            //Disconnect olursa GarbageCollector'un temizlemesini beklemeden biz bağlantıyı kaldırırız.
            _persistenConnection.Dispose(); 
        }
    }
}
