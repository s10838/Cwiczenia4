using System.Runtime.InteropServices.JavaScript;

namespace Cwiczenia4.Model;

public class Order
{

    
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public int IdWarehouse { get; set; }
    public DateTime CreatedAt { get; set; }
    
    
}
