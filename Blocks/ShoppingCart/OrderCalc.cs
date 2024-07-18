using Calculator.Calculator;
using Calculator.Validator;
using Logger;
using ShoppingCart.Exceptions;
using ShoppingCart.Models;

namespace ShoppingCart;

public class OrderCalc : Calculator.Calculator.Calculator
{
    private readonly ILogger _logger;
    private readonly IValidator _validator;
    public OrderCalc(ILogger logger, IValidator validator) : base(logger, validator)
    {
        _logger = logger;
        _validator = validator;
    }
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

    public Order Calculate(Order order, Order secondOrder)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Order and Order");
        }
        return order - secondOrder;
    }

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

    public Order Calculate(Product product, Product secondProduct)
    {
        if (_logger != null)
        {
            _logger.Log("Now starting operation with Product and Product");
        }
        return product + secondProduct;
    }
}