namespace UserInterfaces;

public abstract class UserInterface
{
    public virtual LoggerStatus LoggerControl(string input)
    {
        if (input == "y")
        {
            Console.WriteLine("Хорошо, методы будут отслеживаться");
            return LoggerStatus.On;
        }
        else
        {
            Console.WriteLine("Отслеживание методов отключено");
            return LoggerStatus.Off;
        }
    }

    public abstract void ChooseMode();
}