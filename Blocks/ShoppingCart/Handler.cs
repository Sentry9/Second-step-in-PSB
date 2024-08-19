using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using ShoppingCart.Exceptions;
using ShoppingCart.Validator;
using ShoppingCart.Models;
using JsonException = ShoppingCart.Exceptions.JsonException;

namespace ShoppingCart;

public class Handler
{
    private readonly IValidator _validator;
    public static List<Product>? _products;
    private List<Order> orders = new List<Order>();
    private string? path = Directory.GetCurrentDirectory();
    private readonly OrderCalc _calc;
    private string? productsPath;

    public Handler(IValidator validator, Logger.Logger logger)
    {
        for (int i = 0; i < 4; i++)
        {
            path = Path.GetDirectoryName(path);
        }

        path = path + @"\ShoppingCart\Jsons\";
        Directory.CreateDirectory(path + @"orders");
        productsPath = path + "products.json";
        string[] files = Directory.GetFiles(path);
        string json = File.ReadAllText(files[0]);
        _products = JsonSerializer.Deserialize<List<Product>>(json);
        _calc = new OrderCalc(logger, null);
        _validator = validator;
    }
    
    /// <summary>
    /// Метод для создания заказа
    /// </summary>
    /// <returns>Номер заказа</returns>
    public int CreateOrder()
    {
        Order order = new Order();
        orders.Add(order);
        return order.orderId;
    }
    /// <summary>
    /// Метод для добавления продукта в заказ
    /// </summary>
    /// <param name="idInput">Номер заказ</param>
    /// <param name="productIdInput">Номер продукта</param>
    /// <param name="countInput">Количество продукта</param>
    /// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void AddProduct(string idInput, string productIdInput, string countInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id) &&
            _validator.ValidateInputInt(productIdInput, out int productId) &&
            _validator.ValidateInputInt(countInput, out int count))
        {
            try
            {
                orders[id].AddProduct(productId, count);
            }
            catch(IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа или продукта с данными id не существуют");
                throw;
            }
        }
        else
        {
            throw new ValidatorException("При добавлении продукта введены некорректные данные входные данные");
        }
    }
    /// <summary>
    /// Метод для сохранения заказа в виде json файла
    /// </summary>
    /// <param name="idInput">Номер заказа</param>
    /// <exception cref="ValidatorException">Выбрасывается при неврных входных данных</exception>
    public void SaveOrder(string idInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id))
        {
            try
            {
                orders[id].SaveOrder(path);
            }
            catch(IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
        }
        else
        {
            throw new ValidatorException("Введён некорректный номер заказа");
        }
    }
    /// <summary>
    /// Метод для вывода списка продуктов
    /// </summary>
    public void ShowAssortment()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            Console.WriteLine(_products[i].ToString());
        }
    }
    /// <summary>
    /// Метод для вывода заказа
    /// </summary>
    /// <param name="idInput">Номер заказа</param>
    /// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ShowOrder(string idInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id))
        {
            try
            {
                orders[id].PrintOrder();
            }
            catch(IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
        }
        else
        {
            throw new ValidatorException("Введён некорректный номер заказа");
        }
    }
    /// <summary>
    /// Метод для вычитания заказа из заказа
    /// </summary>
    /// <param name="idInput">Номер заказа из которого вычитают</param>
    /// <param name="id2Input">Номер заказа который вычитают</param>
    public void OrderAndOrder(string idInput, string id2Input)
    {
        if (_validator.ValidateInputInt(idInput, out int id) && _validator.ValidateInputInt(id2Input, out int id2))
        {
            try
            {
                Order order = _calc.Calculate(orders[id], orders[id2]);
                order.SaveOrder(path);
            }
            catch (IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
            
        }
    }
    /// <summary>
    /// Метод для вычесления операции между заказом и числом
    /// </summary>
    /// <param name="idInput">Номер заказа</param>
    /// <param name="op">Знак операции</param>
    /// <param name="numInput">Число</param>
    /// <exception cref="ValidatorException">Выбрасывается при неверных вводных данных</exception>
    public void OrderAndNum(string idInput, string op, string numInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id) && _validator.ValidateInputCount(numInput, out int num))
        {
            try
            {
                Order order = _calc.Calculate(orders[id], op, num);
                order.SaveOrder(path);
            }
            catch (IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
        }
        else
        {
            throw new ValidatorException("Введён некорректный номер заказа или число");
        }
    }
    /// <summary>
    /// Метод для сложения двух продуктов
    /// </summary>
    /// <param name="idInput">Номер первого продукта</param>
    /// <param name="id2Input">Номер второго продукта</param>
    /// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ProductAndProduct(string idInput, string id2Input)
    {
        if(_validator.ValidateInputInt(idInput, out int id) && _validator.ValidateInputInt(id2Input, out int id2))
        {
            try
            {
                Order order = _calc.Calculate(_products[id], _products[id2]);
                order.SaveOrder(path);
            }
            catch (IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
        }
        else
        {
            throw new ValidatorException("Введён некорректный номер заказа");
        }
    }
    /// <summary>
    /// Метод для вычисления операции между заказом и продуктом
    /// </summary>
    /// <param name="idInput">Номер заказа</param>
    /// <param name="op">Знак операции</param>
    /// <param name="productIdInput">Номер продукта</param>
    /// <exception cref="ValidationException">Выбрасывается при неверных входных данных</exception>
    public void OrderAndProduct(string idInput, string op, string productIdInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id) &&
            _validator.ValidateInputInt(productIdInput, out int productId))
        {
            try
            {
                Order order = _calc.Calculate(orders[id], op, _products[productId]);
                order.SaveOrder(path);
            }
            catch (IndexException e)
            {
                Console.WriteLine($"{e.Message}, Заказа под номером {id} не существует");
                throw;
            }
        }
        else
        {
            throw new ValidationException("Введён некорректный номер заказа или продукта");
        }
    }
/// <summary>
/// Метод для изменения продуктов из ассортимента
/// </summary>
/// <param name="id">Номер продукта</param>
/// <param name="name">Название</param>
/// <param name="price">Цена</param>
/// <param name="weight">Вес</param>
/// <param name="date">Дата доставки</param>
/// <param name="param">Уникальный параметр</param>
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
/// <summary>
/// Метод для генерации продукта
/// </summary>
    public void GenerateOrder()
    {
        Random random = new Random();
        int count = random.Next(1, _products.Count);
        int orderId = CreateOrder();
        List<int> satisfyingProducts = new List<int>();
        for (int i = 0; i < _products.Count(); i++)
        {
            satisfyingProducts.Add(i);
        }
        for (int i = 0; i < count; i++)
        {
            int product = random.Next(0, satisfyingProducts.Count());
            int productCount = random.Next(1, 100);
            orders[orderId].AddProduct(product, productCount);
            satisfyingProducts.RemoveAt(product);
            if (satisfyingProducts.Count == 0)
            {
                break;
            }
        }
        orders[orderId].SaveOrder(path);
    }
/// <summary>
/// Метод для генерации набора продуктов, чья сумма меньше maxSum
/// </summary>
/// <param name="maxSumInput">Строковое представление максимальной стоимости продукта</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void GenerateOrderBySum(string maxSumInput)
    {
        if (_validator.ValidateInputDouble(maxSumInput, out double maxSum))
        {
            Random random = new Random();
            int count = random.Next(1, _products.Count());
            int orderId = CreateOrder();
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
                orders[(int)orderId].AddProduct(satisfyingProducts[id], productCount);
                satisfyingProducts.RemoveAt(id);
                if (satisfyingProducts.Count == 0)
                {
                    break;
                }
            }

            orders[(int)orderId].SaveOrder(path);
        }
        else
        {
            throw new ValidatorException("Введена некорректная сумма");
        }
    }
/// <summary>
/// Метод для генерации случайного сриска продуктов, чья сумма лежит в пределах minSum и maxSum
/// </summary>
/// <param name="minSumInput">Минимальная цена продукта в виде строки</param>
/// <param name="maxSumInput">Максимальная цена продукта в виде строки</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных данных</exception>
    public void GenerateOrderBySum(string minSumInput, string maxSumInput)
    {
        if(_validator.ValidateInputDouble(minSumInput, out double minSum) && _validator.ValidateInputDouble(maxSumInput, out double maxSum))
        {
            Random random = new Random();
            int count = random.Next(1, _products.Count());
            int orderId = CreateOrder();
            List<int> satisfyingProducts = new List<int>();
            for (int i = 0; i < _products.Count(); i++)
            {
                if ((_products[i].price < maxSum) && (_products[i].price > minSum))
                {
                    satisfyingProducts.Add(i);
                }
            }

            for (int i = 0; i < count; i++)
            {
                int id = random.Next(0, satisfyingProducts.Count);
                int productCount = random.Next(1, 100);
                orders[orderId].AddProduct(satisfyingProducts[id], productCount);
                satisfyingProducts.RemoveAt(id);
                if (satisfyingProducts.Count == 0)
                {
                    break;
                }
            }

            orders[orderId].SaveOrder(path);
        }
        else
        {
            throw new ValidatorException("Введены некорректные суммы");
        }
    }
/// <summary>
/// Метод для случайной генерации списка продуктов, общее количество которых меньше или равно maxCount
/// </summary>
/// <param name="maxCountInput">Максимальное количество продуктов в виде строки</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void GenerateOrderByCount(string maxCountInput)
    {
        if(_validator.ValidateInputCount(maxCountInput, out int maxCount))
        {
            int limit = maxCount;
            Random random = new Random();
            int count = random.Next(1, _products.Count());
            int orderId = CreateOrder();
            List<int> satisfyingProducts = new List<int>();
            for (int i = 0; i < _products.Count(); i++)
            {
                satisfyingProducts.Add(i);
            }

            for (int i = 0; i < count; i++)
            {
                int product = random.Next(0, satisfyingProducts.Count());
                int productCount = random.Next(1, limit);
                orders[orderId].AddProduct(product, productCount);
                satisfyingProducts.RemoveAt(product);
                limit -= productCount;
                if ((satisfyingProducts.Count == 0) || (limit <= 0))
                {
                    break;
                }
            }

            orders[orderId].SaveOrder(path);
        }
        else
        {
            throw new ValidatorException("Введено неверное количество");
        }
    }
/// <summary>
/// Метод для отображения заказов стоимость которых от minSum и больше
/// </summary>
/// <param name="minSumInput">Нижняя планка стоимости в виде строки</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ShowExpensiveOrders(string minSumInput)
    {
        if (_validator.ValidateInputDouble(minSumInput, out double maxSum))
        {
            var expensOrders = orders.Where(o => o.orderSum > maxSum).ToList();
            foreach (var order in expensOrders)
            {
                ShowOrder(order.orderId.ToString());
                Console.WriteLine();
            }
        }
        else
        {
            throw new ValidatorException("Введена неверная сумма");
        }
    }
/// <summary>
/// Метод для отоброжения заказов стоимостью до maxSum
/// </summary>
/// <param name="maxSumInput">Верхняя планка стоимости заказа</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ShowCheapOrders(string maxSumInput)
    {
        if (_validator.ValidateInputDouble(maxSumInput, out double minSum))
        {
            var cheepOrders = orders.Where(o => o.orderSum < minSum).ToList();
            foreach (var order in cheepOrders)
            {
                ShowOrder(order.orderId.ToString());
                Console.WriteLine();
            }
        }
        else
        {
            throw new ValidatorException("Введена неверная сумма");
        }
    }
/// <summary>
/// Метод для отображения заказов которые содержат определённый продукт
/// </summary>
/// <param name="idInput">Номер продукта</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ShowOrdersWith(string idInput)
    {
        if (_validator.ValidateInputInt(idInput, out int id))
        {
            var ordersWith = orders.Where(o => o.productsCount.ContainsKey(id)).ToList();
            foreach (var order in ordersWith)
            {
                ShowOrder(order.orderId.ToString());
                Console.WriteLine();
            }
        }
        else
        {
            throw new ValidatorException("Введён неверный id");
        }
    }
/// <summary>
/// Метод для отображения отсортированного списка заказов
/// </summary>
    public void ShowSortedOrders()
    {
        var sortedOrders = orders.OrderBy(o => o.orderWeight).ToList();
        foreach (var order in sortedOrders)
        {
            ShowOrder(order.orderId.ToString());
            Console.WriteLine();
        }
    }
/// <summary>
/// Метод для отображения заказов содержащих уникальных продукт
/// </summary>
    public void ShowUniqueOrders()
    {
        var uniqueOrders = orders.Where(order =>
            order.productsCount.Keys.Any(productId =>
                orders.Where(o => o != order).All(o => !o.productsCount.ContainsKey(productId))
            )
        ).ToList();
        foreach (var order in uniqueOrders)
        {
            ShowOrder(order.orderId.ToString());
            Console.WriteLine();
        }
    }
/// <summary>
/// Метод для отображения заказов отправленных до определённой даты
/// </summary>
/// <param name="dateInput">Дата отправки</param>
/// <exception cref="ValidatorException">Выбрасывается при неверных входных данных</exception>
    public void ShowOrdersByDate(string dateInput)
    {
        if (_validator.ValidateInputDate(dateInput, out DateOnly date))
        {
            var DeliveryOrders = orders.Where(o => o.DeliveryDate < date).ToList();
            foreach (var order in DeliveryOrders)
            {
                ShowOrder(order.orderId.ToString());
                Console.WriteLine();
            }
        }
        else
        {
            throw new ValidatorException("Введена неверная дата");
        }
    }
/// <summary>
/// Метод для отображения списка продуктов из стороннего json файла
/// </summary>
/// <param name="clientPath">Путь к файлу</param>
/// <exception cref="JsonException">Выбрасывается если файл не соответствует файлу заказа</exception>
/// <exception cref="ValidationException">Выбрасывается при неверных входных данных</exception>
    public void ShowProducts(string clientPath)
    {
        if(_validator.ValidatePath(clientPath))
        {
            Dictionary<Product, int> products = new Dictionary<Product, int>();
            string json = File.ReadAllText(clientPath);
            if (_validator.ValidateOrderJson(json))
            {
                var order = JsonSerializer.Deserialize<Order>(json);
                foreach (var product in order.productsCount)
                {
                    products.Add(_products[product.Key], (int)product.Value);
                }

                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Key.ToString()}Количество - {product.Value}\n");
                }
            }
            else
            {
                throw new JsonException("Некорректный файл");
            }
        }
        else
        {
            throw new ValidationException("Некорректный путь к файлу");
        }
    }
}