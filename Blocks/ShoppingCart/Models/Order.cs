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
    public Dictionary<int, int> productsCount { get; set; }
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
        productsCount = new Dictionary<int, int>();
        DeliveryDate = DateOnly.MinValue;
        products = new Dictionary<int, Product>();
    }
/// <summary>
/// Метод для загрузки списка продуктов(ассортимента)
/// </summary>
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
/// <summary>
/// Перегрузка сложения заказа и продукта
/// </summary>
/// <param name="order">Сам заказ</param>
/// <param name="product">Продукт, который мы хотим добавить</param>
/// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator +(Order order, Product product)
    {
        order.AddProduct(product.productId, 1);
        return order;
    }
    /// <summary>
    /// Перегрузка вычитания продукта из заказа
    /// </summary>
    /// <param name="order">Сам заказ</param>
    /// <param name="product">Продукт, который мы хотим вычесть</param>
    /// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator -(Order order, Product product)
    {
        order.RemoveProduct(product.productId, 1);
        return order;
    }
    /// <summary>
    /// Перегрузка деления на продукт
    /// </summary>
    /// <param name="order">Сам заказ</param>
    /// <param name="product">Продукт, который мы хотим убрать</param>
    /// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator /(Order order, Product product)
    {
        order.RemoveProduct(product.productId, int.MaxValue);
        return order;
    }
    /// <summary>
    /// Перегрузка деления на число
    /// </summary>
    /// <param name="order">Сам заказ</param>
    /// <param name="num">Во сколько раз мы хотим уменьшить</param>
    /// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator /(Order order, int num)
    {
        foreach (var product in order.productsCount)
        {
            order.RemoveProduct(product.Key, (int)(product.Value * (1 - 1.0 / num)));
        }
        return order;
    }
    /// <summary>
    /// Перегрузка умножения на число
    /// </summary>
    /// <param name="order">Сам заказ</param>
    /// <param name="num">Во сколько раз хотим увеличить</param>
    /// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator *(Order order, int num)
    {
        foreach (var product in order.productsCount)
        {
            order.AddProduct(product.Key, product.Value * (num - 1));
        }
        return order;
    }
    /// <summary>
    /// Перегрузка вычитания заказов
    /// </summary>
    /// <param name="order">Заказ из которого вычитаем</param>
    /// <param name="order2">Заказ который вычитаем</param>
    /// <returns>Возвращает обновлённый заказ</returns>
    public static Order operator -(Order order, Order order2)
    {
        bool checker = true;
        foreach (var product in order2.productsCount)
        {
            if (!order.productsCount.TryGetValue(product.Key, out int value))
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
    /// <summary>
    /// Метод для добавления продукта в заказ
    /// </summary>
    /// <param name="productId">Номер продукта для добавления</param>
    /// <param name="count">Сколько мы хотим добавить</param>
    public void AddProduct(int productId, int count)
    {
        if (productsCount.TryGetValue(productId, out int value))
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
    /// <summary>
    /// Метод для удаления продукта из заказа
    /// </summary>
    /// <param name="productId">Номер продукта для удаления</param>
    /// <param name="count">Сколько мы хотим убрать</param>
    public void RemoveProduct(int productId, int count)
    {
        if (productsCount.TryGetValue(productId, out int value))
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
    /// <summary>
    /// Метод для сохранения заказа
    /// </summary>
    /// <param name="path">Путь к директории в которую сохранияем заказ</param>
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
    /// <summary>
    /// Метод для вывода информации о заказе в консоль
    /// </summary>
    public void PrintOrder()
    {
        Console.WriteLine($"{orderId}\n{orderSum}\n{orderWeight}\n{DeliveryDate}");
        foreach (var product in productsCount)
        {
            Console.WriteLine($"{product.Key} : {product.Value}");
        }
    }
    
}