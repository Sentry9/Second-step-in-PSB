using System.Text.Json;

namespace ShoppingCart.Models;

public class Order
{
    private static int _lastId = -1;
    private static List<Product> _allProducts;
    public int orderId { get; set; }
    public double orderSum { get; set; }
    public double orderWeight { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public Dictionary<int, uint> productsCount { get; set; }
    public Dictionary<int, Product> products { get; set; }

    public Order()
    {
        if (_lastId == -1)
        {
            LoadProducts(); 
        }
        orderId = ++_lastId;
        orderSum = 0;
        orderWeight = 0;
        productsCount = new Dictionary<int, uint>();
        DeliveryDate = DateOnly.MinValue;
        products = new Dictionary<int, Product>();
    }

    private static void LoadProducts()
    {
        string? path = Directory.GetCurrentDirectory();
        for (int i = 0; i < 4; i++)
        {
            path = Path.GetDirectoryName(path);
        }
        path = path + @"\ShoppingCart\Jsons\";
        string[] files = Directory.GetFiles(path);
        string json = File.ReadAllText(files[0]);
        _allProducts = JsonSerializer.Deserialize<List<Product>>(json);
    }

    public static Order operator +(Order order, Product product)
    {
        order.AddProduct(product.productId, 1);
        return order;
    }
    public static Order operator -(Order order, Product product)
    {
        order.RemoveProduct(product.productId, 1);
        return order;
    }
    public static Order operator /(Order order, Product product)
    {
        order.RemoveProduct(product.productId, uint.MaxValue);
        return order;
    }
    public static Order operator /(Order order, uint num)
    {
        foreach (var product in order.productsCount)
        {
            order.RemoveProduct(product.Key, (uint)(product.Value * (1 - 1.0 / num)));
        }
        return order;
    }
    public static Order operator *(Order order, uint num)
    {
        foreach (var product in order.productsCount)
        {
            order.AddProduct(product.Key, product.Value * (num - 1));
        }
        return order;
    }
    public static Order operator -(Order order, Order order2)
    {
        bool checker = true;
        foreach (var product in order2.productsCount)
        {
            if (!order.productsCount.TryGetValue(product.Key, out uint value))
            {
                checker = false;
                Console.WriteLine("Всё херня");
                break;
            }
        }

        if (checker)
        {
            foreach (var product in order2.productsCount)
            {
                order.RemoveProduct(product.Key, product.Value);
            }
        }

        return order;
    }
    
    public void AddProduct(int productId, uint count)
    {
        if (productsCount.TryGetValue(productId, out uint value))
        {
            productsCount[productId] += count;
        }
        else
        {
            productsCount.Add(productId, count);
            products.Add(productId, _allProducts[productId]);
            DeliveryDate = products.Values.Max(p => p.date);
        }
        orderSum = Math.Round(orderSum + products[productId].price * count, 2);
        orderWeight = Math.Round(orderWeight + products[productId].weight * count, 2);
    }

    public void RemoveProduct(int productId, uint count)
    {
        if (productsCount.TryGetValue(productId, out uint value))
        {
            if (value > count)
            {
                productsCount[productId] -= count;
                orderSum = Math.Round(orderSum - products[productId].price * count, 2);
                orderWeight = Math.Round(orderWeight - products[productId].weight * count, 2);
            }
            else
            {
                orderSum = Math.Round(orderSum - products[productId].price * value, 2);
                orderWeight = Math.Round(orderWeight - products[productId].weight * value, 2);
                productsCount.Remove(productId);
                products.Remove(productId);
                if (products.Count == 0)
                {
                    DeliveryDate = DateOnly.MinValue;
                }
                else
                {
                    DeliveryDate = products.Values.Max(p => p.date);
                }
            }
        }
    }

    public void SaveOrder(string path)
    {
        path += @"\orders\";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        string jsonString = JsonSerializer.Serialize(this, options);
        path += $"order{orderId}.json";
        File.WriteAllText(path, jsonString);
    }

    public void PrintOrder()
    {
        Console.WriteLine($"{orderId}\n{orderSum}\n{orderWeight}\n{DeliveryDate}");
        foreach (var product in productsCount)
        {
            Console.WriteLine($"{product.Key} : {product.Value}");
        }
    }
    
}