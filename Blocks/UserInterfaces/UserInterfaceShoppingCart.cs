using Logger;
using ShoppingCart;
using ShoppingCart.Validator;

namespace UserInterfaces;

public class UserInterfaceShoppingCart : UserInterface
{
    private readonly InterfaceValidator _validator;
    private Handler _handler;
    private IValidator handValidator = new Validator();

    private LoggerStatus _loggerStatus;

    public UserInterfaceShoppingCart()
    {
        _validator = new InterfaceValidator();
    }
    public override LoggerStatus LoggerControl(string input)
    {
        switch (input)
        {
            case "y":
                Console.WriteLine("Методы калькулятора будут отслеживаться");
                return LoggerStatus.On;
            case "n":
                Console.WriteLine("Методы калькулятора отслеживаться не будут");
                return LoggerStatus.Off;
            default:
                return base.LoggerControl(input);
        }
    }
    
    public override void ChooseMode()
    {
        string logInput;
        do
        {
            Console.WriteLine("Отслеживать методы калькулятора? y/n");
            logInput = Console.ReadLine();
        } while (!_validator.ValidateLogControl(logInput));

        _loggerStatus = LoggerControl(logInput);
        if (_loggerStatus == LoggerStatus.On)
        {
            Logger.Logger logger = new Logger.Logger();
            _handler = new Handler(handValidator, logger);
        }
        else
        {
            _handler = new Handler(handValidator, null);
        }

        do
        {
            Console.WriteLine("1 - Операции с заказами\n2 - Операции с продуктом\ne - отмена");
            string input = Console.ReadLine();
            if (_validator.ValidateMainInput(input))
            {
                if (input == "1")
                {
                    OrderOp();
                }

                if (input == "2")
                {
                    ProductOp();
                }

                if (input == "e")
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз.");
            }
        } while (true);
    }

    private void OrderOp()
    {
        do
        {
            Console.WriteLine(
                "1 - Создать заказ\n2 - Добавить продукты в заказ\n3 - Сохранить заказ\n4 - Показать заказ\n5 - калькулятор\ne - отмена");
            string op = Console.ReadLine();
            if (_validator.ValidateOrderOpInput(op))
            {
                if (op == "1")
                {
                    Console.WriteLine(
                        "1 - Создать пустой заказ\n2 - Создать случайный заказ\n3 - Случайный набор товаров, чья сумма меньше maxSum\n4 - Случайный набор товаров, чья сумма больше minSum, но меньше MaxSum\n5 - Случайный набор товаров, общее количество которых <= maxCount\ne - отмена");
                    string input = Console.ReadLine();
                    if (_validator.ValidateCreateOrderInput(input))
                    {
                        CreateOrder(input);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                    }
                }

                if (op == "2")
                {
                    Console.WriteLine("Введите номер заказа, номер продукта и количество продуктов через пробел");
                    string input = Console.ReadLine();
                    string[] parts = input.Split(' ');
                    if (_validator.ValidateAddProductInput(parts))
                    {
                        _handler.AddProduct(parts[0], parts[1], parts[2]);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                    }
                }

                if (op == "3")
                {
                    Console.WriteLine("Введите номер заказа");
                    string input = Console.ReadLine();
                    _handler.SaveOrder(input);
                }

                if (op == "4")
                {
                    Console.WriteLine(
                        "1 - Показать заказ по id\n2 - Показать заказы от определённой суммы\n3 - Показать заказы до определённой суммы\n4 - Показать заказы с определённым товаром\n5 - Отсортировать заказы\n6 - Показать заказы с уникальными товарами\n7 - Показать товары отправленные до указанной даты\n8 - Показать товары из заказа по пути к файлу\ne - отмена");
                    string input = Console.ReadLine();
                    showOrder(input);
                }

                if (op == "5")
                {
                    Console.WriteLine(
                        "1 - Заказ и продукт\n2 - Заказ и число\n3 - Заказ и заказ\n4 - Продукт и продукт\ne - отмена");
                    string input = Console.ReadLine();
                    OrderCalc(input);
                }

                if (op == "e")
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз.");
            }
        } while (true);
    }

    private void CreateOrder(string input)
    {
        if (input == "1")
        {
            Console.WriteLine($"Номер пустого заказа: {_handler.CreateOrder()}");
        }

        if (input == "2")
        {
            _handler.GenerateOrder();
            Console.WriteLine("Новый заказ создан, он последний в списке заказов");
        }

        if (input == "3")
        {
            Console.WriteLine("Введите максимальную сумму");
            string sumInput = Console.ReadLine();
            _handler.GenerateOrderBySum(sumInput);
            Console.WriteLine("Новый заказ создан, он последний в списке заказов");
        }

        if (input == "4")
        {
            Console.WriteLine("Введите сумму от и сумму до через пробел");
            string sumInput = Console.ReadLine();
            string[] parts = sumInput.Split(' ');
            if (_validator.ValidateOrderBySum(parts))
            {
                _handler.GenerateOrderBySum(parts[0], parts[1]);
                Console.WriteLine("Новый заказ создан, он последний в списке заказов");
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз.");
            }
        }

        if (input == "5")
        {
            Console.WriteLine("Введите общее количество товаров");
            string countInput = Console.ReadLine();
            if (_validator.ValidateOrderByCount(countInput))
            {
                _handler.GenerateOrderByCount(countInput);
                Console.WriteLine("Новый заказ создан, он последний в списке заказов");
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз.");
            }
        }

        if (input == "e")
        {
            return;
        }
    }

    private void showOrder(string input)
    {
        if (input == "1")
        {
            Console.WriteLine("Введите id заказа");
            string idInput = Console.ReadLine();
            _handler.ShowOrder(idInput);
        }

        if (input == "2")
        {
            Console.WriteLine("Введите сумму от");
            string sumInput = Console.ReadLine();
            _handler.ShowExpensiveOrders(sumInput);
        }

        if (input == "3")
        {
            Console.WriteLine("Введите сумму до");
            string sumInput = Console.ReadLine();
            _handler.ShowCheapOrders(sumInput);
        }

        if (input == "4")
        {
            Console.WriteLine("Введите номер товара");
            string idInput = Console.ReadLine();
            _handler.ShowOrdersWith(idInput);
        }

        if (input == "5")
        {
            _handler.ShowSortedOrders();
        }

        if (input == "6")
        {
            _handler.ShowUniqueOrders();
        }

        if (input == "7")
        {
            Console.WriteLine("Введите дату");
            string dateInput = Console.ReadLine();
            _handler.ShowOrdersByDate(dateInput);
        }

        if (input == "8")
        {
            Console.WriteLine("Введите путь к файлу");
            string pathInput = Console.ReadLine();
            _handler.ShowProducts(pathInput);
        }

        if (input == "e")
        {
            return;
        }
    }

    private void OrderCalc(string input)
    {
        if (_validator.ValidateOrderCalcInput(input))
        {
            if (input == "1")
            {
                Console.WriteLine("Введите номер заказа знак операции и номер продукта через пробел");
                string calcInput = Console.ReadLine();
                string[] parts = calcInput.Split(' ');
                if (_validator.ValidateOrderAndProductInput(parts))
                {
                    _handler.OrderAndProduct(parts[0], parts[1], parts[2]);
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                }
            }

            if (input == "2")
            {
                Console.WriteLine("Введите номер заказа знак операции и число через пробел");
                string calcInput = Console.ReadLine();
                string[] parts = calcInput.Split(' ');
                if (_validator.ValidateOrderAndNumInput(parts))
                {
                    _handler.OrderAndNum(parts[0], parts[1], parts[2]);
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                }
            }

            if (input == "3")
            {
                Console.WriteLine("Введите номера заказов через пробел");
                string calcInput = Console.ReadLine();
                string[] parts = calcInput.Split(' ');
                if (_validator.ValidateOrderAndOrderInput(parts))
                {
                    _handler.OrderAndOrder(parts[0], parts[1]);
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                }
            }

            if (input == "4")
            {
                Console.WriteLine("Введите номера продуктов через пробел");
                string calcInput = Console.ReadLine();
                string[] parts = calcInput.Split(' ');
                if (_validator.ValidateProductAndProductInput(parts))
                {
                    _handler.ProductAndProduct(parts[0], parts[1]);
                    Console.WriteLine("Новый заказ будет последним в списке");
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                }
            }

            if (input == "e")
            {
                return;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод, попробуйте еще раз.");
        }
    }

    private void ProductOp()
    {
        Console.WriteLine("1 - Показать ассортимент\n2 - Изменить продукт\ne - отмена");
        string input = Console.ReadLine();
        if (_validator.ValidateProductOpInput(input))
        {
            if (input == "1")
            {
                _handler.ShowAssortment();
            }

            if (input == "2")
            {
                Console.WriteLine(
                    "Введите через пробел номер продукта, название параметра для изменения и новое значение");
                string changeInput = Console.ReadLine();
                string[] parts = changeInput.Split(' ');
                int id = Convert.ToInt32(parts[0]);
                string paramName = parts[1];
                string value = parts[2];
                if (_validator.ValidateProductChangeParams(paramName, value))
                {
                    switch (paramName)
                    {
                        case "name":
                            _handler.ChangeProduct(id, name: value);
                            break;
                        case "price":
                            Double priceValue = Double.Parse(value);
                            _handler.ChangeProduct(id, price: priceValue);
                            break;
                        case "weight":
                            Double weightValue = Double.Parse(value);
                            _handler.ChangeProduct(id, weight: weightValue);
                            break;
                        case "date":
                            DateOnly date = DateOnly.Parse(value);
                            _handler.ChangeProduct(id, date: date);
                            break;
                        case "parametr":
                            _handler.ChangeProduct(id, param: value);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                }
            }

            if (input == "e")
            {
                return;
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод, попробуйте еще раз.");
        }
    }
}
