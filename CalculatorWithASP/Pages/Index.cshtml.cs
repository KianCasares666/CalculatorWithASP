using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CalculatorWithASP.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public string Display { get; set; } = "0";
        [BindProperty] public string PreviousValue { get; set; } = "";
        [BindProperty] public string CurrentOperator { get; set; } = "";

        public void OnPost(string button, string display, string previousValue, string currentOperator)
        {
            // Sync values from the form
            Display = display;
            PreviousValue = previousValue;
            CurrentOperator = currentOperator;

            if (int.TryParse(button, out _)) // Number buttons (0-9)
            {
                if (Display == "0" || Display == "Error")
                    Display = button;
                else
                    Display += button;
            }
            else if (button == "C") // Cancel/Clear
            {
                Display = "0";
                PreviousValue = "";
                CurrentOperator = "";
            }
            else if (button == "=") // Equals (The Math)
            {
                if (!string.IsNullOrEmpty(PreviousValue) && !string.IsNullOrEmpty(CurrentOperator))
                {
                    double val1 = double.Parse(PreviousValue);
                    double val2 = double.Parse(Display);

                    double result = CurrentOperator switch
                    {
                        "+" => val1 + val2,
                        "-" => val1 - val2,
                        "*" => val1 * val2,
                        "/" => val2 != 0 ? val1 / val2 : double.NaN,
                        _ => val2
                    };

                    Display = double.IsNaN(result) ? "Error" : result.ToString();
                    PreviousValue = "";
                    CurrentOperator = "";
                }
            }
            else // Operators (+, -, *, /)
            {
                PreviousValue = Display;
                CurrentOperator = button;
                Display = "0"; // Reset screen for the second number
            }
        }
    }
}