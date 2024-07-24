using Calculator.Calculator;
using Calculator.Validator;
using Logger;
using ShoppingCart.Exceptions;
using ShoppingCart.Models;

namespace ShoppingCart;

public class OrderCalc : Calculator.Calculator.Calculator
{
    private readonly ILogger _logger;
    private readonly IValidatorCalc _validatorCalc;
    public OrderCalc(ILogger logger, IValidatorCalc validatorCalc) : base(logger, validatorCalc)
    {
        _logger = logger;
        _validatorCalc = validatorCalc;
    }
    /// <summary>
    /// Метод для сложения/вычитания/днления заказа и продукта перегрузка этих операций находится в классе Order
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="op">Знак операции</param>
    /// <param name="product">Продукт</param>
    /// <returns>Изменённый заказ</returns>
    /// <exception cref="CalcException">Выбрасывается при любом другом знаке операции</exception>
    public Order Calculate(Order order, string op, Product product)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Order and Product");
        }
        switch (op)
        {
            case "+":
                return order + product;
            case "-":
                return order - product;
            case "/":
                return order / product;
            default:
                throw new CalcException("Такая операция не поддерживается");
        }
    }
    /// <summary>
    /// Метод для вычитания заказа из заказа
    /// </summary>
    /// <param name="order">Заказ из которого вычитают</param>
    /// <param name="secondOrder">Заказ который вычитают</param>
    /// <returns>Изменённый заказ</returns>
    public Order Calculate(Order order, Order secondOrder)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Order and Order");
        }
        return order - secondOrder;
    }
    /// <summary>
    /// Метод для деления/умножения заказа на число
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="op">Знак операции</param>
    /// <param name="num">Число</param>
    /// <returns>Изменённый заказ</returns>
    /// <exception cref="CalcException">Выбрасывается при любом другом знаке операции</exception>
    public Order Calculate(Order order, string op, int num)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Order and Num");
        }
        switch (op)
        {
            case "/":
                return order / num;
            case "*":
                return order * num;
            default:
                throw new CalcException("Такая операция не поддерживается");
        }
    }
    /// <summary>
    /// Метод для сложения двух продуктов в заказ
    /// </summary>
    /// <param name="product">Первый продукт</param>
    /// <param name="secondProduct">Второй продукт</param>
    /// <returns>Заказ содержащий оба продукта</returns>
    public Order Calculate(Product product, Product secondProduct)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Product and Product");
        }
        return product + secondProduct;
    }
}