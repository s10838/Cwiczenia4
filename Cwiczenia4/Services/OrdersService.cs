using Cwiczenia4.Repositories;

namespace Cwiczenia4.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public int CreateOrder(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt)
        {
            return _ordersRepository.CreateOrder(IdProduct, IdWarehouse, Amount, CreatedAt);
        }
    }
}