using ShoppingCart.Models;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using ShoppingCart.Validator;

namespace ShoppingCart;

class Program
{
    public static void Main(string[] args)
    {
        IValidator validator = new Validator.Validator();
        Handler handler = new Handler(validator);
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

        handler.ShowProducts(@"D:\PSB\Second-step-in-PSB\Blocks\ShoppingCart\Jsons\products.json");
    }
}