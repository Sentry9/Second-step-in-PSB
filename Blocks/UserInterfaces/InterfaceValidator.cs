namespace UserInterfaces;

public class InterfaceValidator
{
    public bool ValidateMainInput(string input)
        {
            return input == "1" || input == "2" || input == "e";
        }

        public bool ValidateOrderOpInput(string input)
        {
            return input == "1" || input == "2" || input == "3" || input == "4" || input == "5" || input == "e";
        }

        public bool ValidateCreateOrderInput(string input)
        {
            return input == "1" || input == "2" || input == "3" || input == "4" || input == "5" || input == "e";
        }

        public bool ValidateProductOpInput(string input)
        {
            return input == "1" || input == "2" || input == "e";
        }

        public bool ValidateOrderCalcInput(string input)
        {
            return input == "1" || input == "2" || input == "3" || input == "4" || input == "e";
        }

        public bool ValidateProductChangeParams(string paramName, string value)
        {
            switch (paramName)
            {
                case "name":
                    return !string.IsNullOrEmpty(value);
                case "price":
                case "weight":
                    return double.TryParse(value, out _);
                case "date":
                    return DateOnly.TryParse(value, out _);
                case "parametr":
                    return !string.IsNullOrEmpty(value);
                default:
                    return false;
            }
        }
        public bool ValidateLogControl(string answer)
        {
            if (answer == "y" || answer == "n")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.\n");
                return false;
            }
        }

        public bool ValidateOrderAndProductInput(string[] parts)
        {
            return parts.Length == 3 && int.TryParse(parts[0], out _) && int.TryParse(parts[2], out _);
        }

        public bool ValidateOrderAndNumInput(string[] parts)
        {
            return parts.Length == 3 && int.TryParse(parts[0], out _) && double.TryParse(parts[2], out _);
        }

        public bool ValidateOrderAndOrderInput(string[] parts)
        {
            return parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
        }

        public bool ValidateProductAndProductInput(string[] parts)
        {
            return parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
        }

        public bool ValidateOrderBySum(string[] parts)
        {
            return parts.Length == 2 && double.TryParse(parts[0], out _) && double.TryParse(parts[1], out _);
        }

        public bool ValidateOrderByCount(string value)
        {
            return int.TryParse(value, out _);
        }

        public bool ValidateAddProductInput(string[] parts)
        {
            return parts.Length == 3 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _) && int.TryParse(parts[2], out _);
        }
}