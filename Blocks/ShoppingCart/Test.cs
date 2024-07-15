using ShoppingCart.Models;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ShoppingCart;

class Program
{
    public static void Main(string[] args)
    {
        Handler handler = new Handler();
        //handler.CreateOrder();
        //handler.AddProduct(0, 1, 45);
        //handler.AddProduct(0, 3, 23);
        //handler.SaveOrder(0);
        //handler.CreateOrder();
        //handler.AddProduct(1, 1, 20); ;
        //handler.AddProduct(1, 3, 20);
        //handler.SaveOrder(1);
        //handler.OrderAndNum(1, "/", 4);
        //handler.ProductAndProduct(0, 1);

        //handler.GenerateOrder();
        //handler.GenerateOrderByCount(10);
        //handler.GenerateOrderBySum(170);
        //handler.GenerateOrderBySum(140, 240);
        //handler.LINQtester(20000,20000, 4, new DateOnly(2024, 10, 27));
        handler.ShowProducts(@"D:\PSB\Second-step-in-PSB\Blocks\ShoppingCart\Jsons\orders\order2.json");
    }
}