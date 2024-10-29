using CalculatorLibrary;
using System.Text.RegularExpressions;
namespace UserInterfaceLibrary;


public class UserInterface
{
    public void ShowMenu()
    {
        bool endApp = false;
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");
        Calculator calculator = new Calculator();
        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? readResult = "";
            string? numInput1;
            string? numInput2;
            double calculationResult = 0;

            Console.WriteLine("Choose an operation from the following list:");
            Console.WriteLine("\tc - Calculator");
            Console.WriteLine("\th - History");
            Console.WriteLine("\td - Delete History");
            Console.Write("Your option? ");
            readResult = Console.ReadLine();
            if (readResult == null || !Regex.IsMatch(readResult, "[c|h|d]"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                switch(readResult)
                {
                    case "c":
                        // Ask the user to type the first number.
                        Console.Write("Type a number, and then press Enter: ");
                        numInput1 = Console.ReadLine();

                        double cleanNum1 = 0;
                        while (!double.TryParse(numInput1, out cleanNum1))
                        {
                            Console.Write("This is not valid input. Please enter a numeric value: ");
                            numInput1 = Console.ReadLine();
                        }

                        // Ask the user to type the second number.
                        Console.Write("Type another number, and then press Enter: ");
                        numInput2 = Console.ReadLine();

                        double cleanNum2 = 0;
                        while (!double.TryParse(numInput2, out cleanNum2))
                        {
                            Console.Write("This is not valid input. Please enter a numeric value: ");
                            numInput2 = Console.ReadLine();
                        }

                        // Ask the user to choose an operator.
                        Console.WriteLine("Choose an operator from the following list:");
                        Console.WriteLine("\ta - Add");
                        Console.WriteLine("\ts - Subtract");
                        Console.WriteLine("\tm - Multiply");
                        Console.WriteLine("\td - Divide");
                        Console.Write("Your option? ");

                        string? op = Console.ReadLine();

                        // Validate input is not null, and matches the pattern
                        if (op == null || !Regex.IsMatch(op, "[a|s|m|d]"))
                        {
                            Console.WriteLine("Error: Unrecognized input.");
                        }
                        else
                        {
                            try
                            {
                                calculationResult = calculator.DoOperation(cleanNum1, cleanNum2, op);
                                if (double.IsNaN(calculationResult))
                                {
                                    Console.WriteLine("This operation will result in a mathematical error.\n");
                                }
                                else Console.WriteLine("Your result: {0:0.##}\n", calculationResult);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                            }
                        }
                        calculator.UsageCounter();
                        Console.WriteLine("------------------------\n");

                        // Wait for the user to respond before closing.
                        Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                        if (Console.ReadLine() == "n") endApp = true;

                        Console.WriteLine("\n"); // Friendly linespacing.

                        break;
                    case "h":
                        calculator.ShowHistory();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "d":
                        calculator.DeleteHistory();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                }
            }
            
        }
        
        calculator.Finish();
        calculator.ShowUsage();
        return;
    }
}
