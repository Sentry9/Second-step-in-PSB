namespace ShoppingCart.Models.Goods;

public class CucumberSmooth : Product
{
    public CucumberSmooth()
    {
        productId = 2;
        price = 109.99;
        weight = 1.00;
        name = "Cucumber Smooth";
        date = new DateOnly(2024, 10, 27);
        parametr = "Russia";
    }
    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}