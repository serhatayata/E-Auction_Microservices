using AutoMapper;
using EventBusRabbitMQ.Events.Concrete;
using Ordering.Application.Commands.OrderCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Order.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap(); //ReverseMap ile çift taraflı olabileceği belirtilir.
        }
    }
}

