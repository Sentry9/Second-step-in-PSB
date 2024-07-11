using System.Collections.Specialized;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using ShoppingCart.Models;

namespace ShoppingCart;

public class Handler
{
    public static uint orderId = 0;
    public static List<Product>? _products;
    private List<Order> orders = new List<Order>();
    private string? path = Directory.GetCurrentDirectory();
    private readonly OrderCalc _calc;
    private string? productsPath;

    public Handler()
    {
        for (int i = 0; i < 3; i++)
        {
            path = Path.GetDirectoryName(path);
        }

        path = path + @"\Jsons\";
        productsPath = path + "products.json";
        string[] files = Directory.GetFiles(path);
        string json = File.ReadAllText(files[0]);
        _products = JsonSerializer.Deserialize<List<Product>>(json);
        _calc = new OrderCalc(null, null);
    }

    public uint CreateOrder()
    {
        Order order = new Order();
        orders.Add(order);
        return order.orderId;
    }

    public void AddProduct(int id, int productId, uint count)
    {
        orders[id].AddProduct(productId, count);
    }

    public void SaveOrder(int id)
    {
        orders[id].SaveOrder(path);
    }

    public void ShowProducts()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            Console.WriteLine(_products[i].ToString());
        }
    }

    public void ShowOrder(uint orderId)
    {
        orders[(int)orderId].PrintOrder();
    }

    public void OrderAndOrder(int id, int id2)
    {
        Order order = _calc.Calculate(orders[id], orders[id2]);
        order.SaveOrder(path);
    }

    public void OrderAndNum(int id, string op, uint num)
    {
        Order order = _calc.Calculate(orders[id], op, num);
        order.SaveOrder(path);
    }

    public void ProductAndProduct(int id, int id2)
    {
        Order order = _calc.Calculate(_products[id], _products[id2]);
        order.SaveOrder(path);
    }

    public void OrderAndProduct(int id, string op, int productId)
    {
        Order order = _calc.Calculate(orders[id], op, _products[productId]);
        order.SaveOrder(path);
    }

    public void ChangeProduct(int id, string? name = null, double price = -1.0, double weight = -1.0,
        DateOnly date = default, string? param = null)
    {
        if (name == null)
        {
            name = _products[id].name;
        }

        if (price == -1.0)
        {
            price = _products[id].price;
        }

        if (weight == -1.0)
        {
            weight = _products[id].weight;
        }

        if (date == default)
        {
            date = _products[id].date;
        }

        if (param == null)
        {
            param = _products[id].parametr;
        }

        _products[id].name = name;
        _products[id].date = date;
        _products[id].price = price;
        _products[id].weight = weight;
        _products[id].parametr = param;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        string jsonString = JsonSerializer.Serialize(_products, options);
        File.WriteAllText(productsPath, jsonString);
    }

    public void GenerateOrder()
    {
        Random random = new Random();
        int count = random.Next(1, _products.Count);
        uint orderId = CreateOrder();
        List<int> satisfyingProducts = new List<int>();
        for (int i = 0; i < _products.Count(); i++)
        {
            satisfyingProducts.Add(i);
        }
        for (int i = 0; i < count; i++)
        {
            int product = random.Next(0, satisfyingProducts.Count());
            int productCount = random.Next(1, 100);
            orders[(int)orderId].AddProduct(product, (uint)productCount);
            satisfyingProducts.RemoveAt(product);
            if (satisfyingProducts.Count == 0)
            {
                break;
            }
        }
        orders[(int)orderId].SaveOrder(path);
    }
    public void GenerateOrderBySum(double maxSum)
    {
        Random random = new Random();
        int count = random.Next(1, _products.Count());
        uint orderId = CreateOrder();
        List<int> satisfyingProducts = new List<int>();
        for (int i = 0; i < _products.Count(); i++)
        {
            if (_products[i].price < maxSum)
            {
                satisfyingProducts.Add(i);
            }
        }
        for (int i = 0; i < count; i++)
        {
            int id = random.Next(0, satisfyingProducts.Count);
            int productCount = random.Next(1, 100);
            orders[(int)orderId].AddProduct(satisfyingProducts[id], (uint)productCount);
            satisfyingProducts.RemoveAt(id);
            if (satisfyingProducts.Count == 0)
            {
                break;
            }
        }
        orders[(int)orderId].SaveOrder(path);
    }
    public void GenerateOrderBySum(double minSum, double maxSum)
    {
        Random random = new Random();
        int count = random.Next(1, _products.Count());
        uint orderId = CreateOrder();
        List<int> satisfyingProducts = new List<int>();
        for (int i = 0; i < _products.Count(); i++)
        {
            if ((_products[i].price < maxSum)&&(_products[i].price > minSum))
            {
                satisfyingProducts.Add(i);
            }
        }
        for (int i = 0; i < count; i++)
        {
            int id = random.Next(0, satisfyingProducts.Count);
            int productCount = random.Next(1, 100);
            orders[(int)orderId].AddProduct(satisfyingProducts[id], (uint)productCount);
            satisfyingProducts.RemoveAt(id);
            if (satisfyingProducts.Count == 0)
            {
                break;
            }
        }
        orders[(int)orderId].SaveOrder(path);
    }
    public void GenerateOrderByCount(int maxCount)
    {
        int limit = maxCount;
        Random random = new Random();
        int count = random.Next(1, _products.Count());
        uint orderId = CreateOrder();
        List<int> satisfyingProducts = new List<int>();
        for (int i = 0; i < _products.Count(); i++)
        {
            satisfyingProducts.Add(i);
        }
        for (int i = 0; i < count; i++)
        {
            int product = random.Next(0, satisfyingProducts.Count());
            int productCount = random.Next(1, limit);
            orders[(int)orderId].AddProduct(product, (uint)productCount);
            satisfyingProducts.RemoveAt(product);
            limit -= productCount;
            if ((satisfyingProducts.Count == 0) || (limit <= 0))
            {
                break;
            }
        }
        orders[(int)orderId].SaveOrder(path);
    }

    public void LINQtester(double maxSum, double minSum, int id, DateOnly date)
    {
        var expensOrders = orders.Where(o => o.orderSum > maxSum).ToList();
        Console.WriteLine("Expens\n\n\n\n\n\n\n\n");
        foreach (var order in expensOrders)
        {
            ShowOrder(order.orderId);
            Console.WriteLine();
        }
        var cheepOrders = orders.Where(o => o.orderSum < minSum).ToList();
        Console.WriteLine("Cheep\n\n\n\n\n\n\n");
        foreach (var order in cheepOrders)
        {
            ShowOrder(order.orderId);
            Console.WriteLine();
        }
        var ordersWith = orders.Where(o => o.products.ContainsKey(id)).ToList();
        Console.WriteLine("With\n\n\n\n\n\n\n");
        foreach (var order in ordersWith)
        {
            ShowOrder(order.orderId);
            Console.WriteLine();
        }
        var sortedOrders = orders.OrderBy(o => o.orderWeight).ToList();
        Console.WriteLine("Sorted\n\n\n\n\n\n\n");
        foreach (var order in sortedOrders)
        {
            ShowOrder(order.orderId);
            Console.WriteLine();
        }
    }
}