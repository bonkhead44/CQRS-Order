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

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _iOrderRepository;
        private readonly ILogger<DeleteOrderCommand> _iLogger;

        public DeleteOrderCommandHandler(IOrderRepository iOrderRepository, ILogger<DeleteOrderCommand> iLogger)
        {
            _iOrderRepository = iOrderRepository;
            _iLogger = iLogger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _iOrderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                //_iLogger.LogError("Order not found.");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            await _iOrderRepository.DeleteAsync(orderToDelete);
            _iLogger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");
            return Unit.Value;
        }
    }
}
