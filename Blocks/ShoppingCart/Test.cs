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
        handler.ShowProducts();
    }
}