using ShoppingCart.Models;
using ShoppingCart.Models.Goods;
using System.IO;
using System.Text.Json;

namespace ShoppingCart;

class Program
{
    public static void Main(string[] args)
    {
        Order order = new Order(1);
        Order order2 = new Order(2);
        Mayo365 mayo365 = new Mayo365();
        MayoMaheev mayoMaheev = new MayoMaheev();
        order.AddProduct(mayo365, 123);
        order2.AddProduct(mayo365, 21);
        Console.WriteLine($"{order.orderSum}, {order.orderWeight}, {order.orderId}" );
        Console.WriteLine($"{order2.orderSum}, {order2.orderWeight}, {order2.orderId}" );
    }
}