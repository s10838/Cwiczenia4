using Cwiczenia4.Model;

namespace Cwiczenia4.Services;

public interface IOrdersService
{

    int CreateOrder(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt);

}