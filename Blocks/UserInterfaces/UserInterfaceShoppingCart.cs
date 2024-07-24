using ShoppingCart;

namespace UserInterfaces;

public class UserInterfaceShoppingCart : UserInterface
{
    
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
        ;
    }

    public void ProductOp(Handler handler)
    {
        Console.WriteLine("Доступые операции:\n1 - Показать ассортимент продуктов\n2 - Изменить продукт\n");
        
    }
}