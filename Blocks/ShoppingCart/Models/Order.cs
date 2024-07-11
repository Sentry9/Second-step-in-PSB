using System.Text.Json;

namespace ShoppingCart.Models;

public class Order
{
    public uint orderId { get; set; }
    public double orderSum { get; set; }
    public double orderWeight { get; set; }
    public Dictionary<int, uint> products { get; set; }

    public Order()
    {
        orderId = Handler.orderId;
        Handler.orderId++;
        orderSum = 0;
        orderWeight = 0;
        products = new Dictionary<int, uint>();
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
        foreach (var product in order.products)
        {
            order.RemoveProduct(product.Key, (uint)(product.Value * (1 - 1.0 / num)));
        }
        return order;
    }
    public static Order operator *(Order order, uint num)
    {
        foreach (var product in order.products)
        {
            order.AddProduct(product.Key, product.Value * (num - 1));
        }
        return order;
    }
    public static Order operator -(Order order, Order order2)
    {
        bool checker = true;
        foreach (var product in order2.products)
        {
            if (!order.products.TryGetValue(product.Key, out uint value))
            {
                checker = false;
                Console.WriteLine("Всё херня");
                break;
            }
        }

        if (checker)
        {
            foreach (var product in order2.products)
            {
                order.RemoveProduct(product.Key, product.Value);
            }
        }

        return order;
    }
    
    public void AddProduct(int productId, uint count)
    {
        if (products.TryGetValue(productId, out uint value))
        {
            products[productId] += count;
        }
        else
        {
            products.Add(productId, count);
        }
        orderSum = Math.Round(orderSum + Handler._products[productId].price * count, 2);
        orderWeight = Math.Round(orderWeight + Handler._products[productId].weight * count, 2);
    }

    public void RemoveProduct(int productId, uint count)
    {
        if (products.TryGetValue(productId, out uint value))
        {
            if (value > count)
            {
                products[productId] -= count;
                orderSum = Math.Round(orderSum - Handler._products[productId].price * count, 2);
                orderWeight = Math.Round(orderWeight - Handler._products[productId].weight * count, 2);
            }
            else
            {
                products.Remove(productId);
                orderSum = Math.Round(orderSum - Handler._products[productId].price * value, 2);
                orderWeight = Math.Round(orderWeight - Handler._products[productId].weight * value, 2);
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
        Console.WriteLine($"{orderId}\n{orderSum}\n{orderWeight}");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Key} : {product.Value}");
        }
    }
    
}