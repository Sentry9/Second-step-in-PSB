namespace ShoppingCart.Models.Goods;

public class CucumberAthlet : Product
{
    public CucumberAthlet()
    {
        productId = 6;
        price = 214.99;
        weight = 1.00;
        name = "Cucumber Athlet";
        date = new DateOnly(2024, 10, 10);
        parametr = "Georgia";
    }
    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}