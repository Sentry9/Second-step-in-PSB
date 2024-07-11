using Calculator.Calculator;
using Calculator.Validator;
using Logger;
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
        switch (op)
        {
            case "+":
                return order + product;
            case "-":
                return order - product;
            case "/":
                return order / product;
            default:
                return order;
        }
    }

    public Order Calculate(Order order, Order secondOrder)
    {
        return order - secondOrder;
    }

    public Order Calculate(Order order, string op, uint num)
    {
        switch (op)
        {
            case "/":
                return order / num;
            case "*":
                return order * num;
            default:
                return order;
        }
    }

    public Order Calculate(Product product, Product secondProduct)
    {
        return product + secondProduct;
    }
}