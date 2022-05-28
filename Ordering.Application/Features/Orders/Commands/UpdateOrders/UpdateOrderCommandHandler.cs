using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _iOrderRepository;
        private readonly IMapper _iMapper;
        private readonly ILogger<UpdateOrderCommand> _iLogger;

        public UpdateOrderCommandHandler(IOrderRepository iOrderRepository, IMapper iMapper, ILogger<UpdateOrderCommand> iLogger)
        {
            _iOrderRepository = iOrderRepository;
            _iMapper = iMapper;
            _iLogger = iLogger;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _iOrderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null) {
                //_iLogger.LogError("Order not found.");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _iMapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await _iOrderRepository.UpdateAsync(orderToUpdate);
            _iLogger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
            return Unit.Value;         
        }
    }
}
