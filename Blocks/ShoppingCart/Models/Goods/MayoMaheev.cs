namespace ShoppingCart.Models.Goods;

public class MayoMaheev : Product
{
    public MayoMaheev()
    {
        productId = 5;
        price = 149.99;
        weight = 0.8;
        this.name = "Mayo Maheev";
        date = new DateOnly(2024, 10, 30);
        parametr = "50.5%";
    }
    public override string ToString()
    {
        return $"{name}, {price}, {weight}, {parametr}";
    }
}