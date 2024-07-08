using System.Collections.Specialized;
using System.Text.Json;
using System.Text.Json.Serialization;
using ShoppingCart.Models;

namespace ShoppingCart;

public class Handler
{
    private List<Order> orders;
    private uint orderId = 0;
    private string path = Directory.GetCurrentDirectory();
    public void CreateOrder()
    {
        Order order = new Order(orderId);
        orders.Add(order);
        orderId++;
    }

    public void AddProduct(int id, Product product, uint count)
    {
        orders[id].AddProduct(product, count);
    }

    public void ShowProducts()
    {
        for (int i = 0; i < 3; i++)
        {
            path = Path.GetDirectoryName(path);
        }

        path = path + @"\Jsons\";
        string[] files = Directory.GetFiles(path);
        string json = File.ReadAllText(files[0]);
        List<Product> products = JsonSerializer.Deserialize<List<Product>>(json);
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine(products[i].ToString());
        }
    }



}