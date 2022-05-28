using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _iOrderRepository;
        private readonly IMapper _iMapper;
        private readonly IEmailService _iEmailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _iLogger;

        public CheckoutOrderCommandHandler(IOrderRepository iOrderRepository, IMapper iMapper, IEmailService iEmailService, ILogger<CheckoutOrderCommandHandler> iLogger)
        {
            _iOrderRepository = iOrderRepository;
            _iMapper = iMapper;
            _iEmailService = iEmailService;
            _iLogger = iLogger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity  = _iMapper.Map<Order>(request);
            var newOrder = await _iOrderRepository.AddAsync(orderEntity);
            _iLogger.LogInformation($"Order {newOrder.Id} is successsfully created.");
            await SendMail(newOrder);
            return newOrder.Id;
        }

        public async Task SendMail(Order newOrder)
        {
            try
            {
                var mail = new Email() { To = "shovon@gmail.com", Subject = "New order email", Body = "Order created successfully." };
                await _iEmailService.SendEmail(mail);
            }
            catch (Exception ex)
            {
                _iLogger.LogError($"Order {newOrder.Id} due to an error with email service: {ex.Message}");
            }
        }
    }
}
