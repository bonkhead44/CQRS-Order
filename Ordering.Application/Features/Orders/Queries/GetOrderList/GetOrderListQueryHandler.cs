using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _iOrderRepository;
        private readonly IMapper _imapper;
        public GetOrderListQueryHandler(IOrderRepository iOrderRepository, IMapper imapper)
        {
            _imapper = imapper;
            _iOrderRepository = iOrderRepository;
        }
        public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _iOrderRepository.GetOrdersByUserName(request.UserName);
            return _imapper.Map<List<OrdersVm>>(orderList); 
        }
    }
}
