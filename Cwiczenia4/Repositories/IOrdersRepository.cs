using System.Reflection;
using Cwiczenia4.Model;

namespace Cwiczenia4.Repositories;

public interface IOrdersRepository
{

    int CreateOrder(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt);


}