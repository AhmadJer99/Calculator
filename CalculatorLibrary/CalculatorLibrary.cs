using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CalculatorLibrary
{
    public class Calculator
    {
        protected  string _pastResults = "";
        
        JsonWriter writer;
        JsonWriter countWriter;
        protected int _counter { get; set; } = 0;
        protected readonly string _countLogFilePath = "UsageCountLog.json";
        public void ShowUsage()
        {
            
            Console.WriteLine(string.Format("You've used the calculator {0} {1}",_counter,(_counter > 1 ? "times":"time")));
        }

        public void UsageCounter()
        {
            _counter++;
        }
        public Calculator()
        {
             StreamWriter countLogFile = File.CreateText(_countLogFilePath);
            countLogFile.AutoFlush = true;
            countWriter = new JsonTextWriter(countLogFile);
            countWriter.Formatting = Formatting.Indented;
            countWriter.WriteStartObject();
            countWriter.WritePropertyName("Usage count");
            countWriter.WriteStartArray();
            countWriter.WriteStartObject();
            countWriter.WritePropertyName("count");

             StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
        public  double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" which we use if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            var currentTime = System.DateTime.Now.ToString();
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    _pastResults += $"{num1} + {num2} = {result},";
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    _pastResults += $"{num1} - {num2} = {result},";
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    _pastResults += $"{num1} * {num2} = {result},";
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    _pastResults += $"{num1} / {num2} = {result},";
                    break;
                // Return text for an incorrect option entry.
  
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();

            countWriter.WriteValue(_counter);
            countWriter.WriteEndObject();
            countWriter.WriteEndArray();
            countWriter.WriteEndObject();
            countWriter.Close();
        }
        public void ShowHistory()
        {
            Console.WriteLine("Previous calculations: ");
            string[] pastResults = _pastResults.Split(',');
            foreach (string result in pastResults)
            {
                Console.WriteLine(result);
            }
            Console.WriteLine("\n");
        }
        public void DeleteHistory()
        {
            Console.WriteLine("Deleting history...");
            _pastResults = "";
        }
    }
}
