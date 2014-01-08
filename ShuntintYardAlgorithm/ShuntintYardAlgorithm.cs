/* Converts a regular mathematical expression to Reverse Polish Notation
 * Included functions:
 * pow - lift a number to power X. Takes 2 arguments - the number and the power.
 * ln - logarithm of a number. Takes 1 argument.
 * sqrt - square root of a number. Takes 1 argument. */

using System;
using System.Collections.Generic;
using System.Text;

class ShuntintYardAlgorithm
{

    // Lists of all used operators, expression separators and functions
    static List<char> operators = new List<char>() { '+', '-', '*', '/' };
    static List<char> brackets = new List<char>() { '(', ')' };
    static List<char> numbers = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    static List<string> functions = new List<string>() { "pow", "ln", "sqrt" };

    // First trim all white spaces
    static string TrimWhitespace(string input)
    {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] != ' ')
            {
                result.Append(input[i].ToString());
            }
        }

        return result.ToString();
    }

    // Split the trimmed input into a List of strings to be passed to the shunting yard converter
    static List<string> SplitInput(string input)
    {
        List<string> result = new List<string>();
        bool isFloatingPoint = false;
        bool isFunction = false;
        bool isMultipartNumber = false;
        StringBuilder holder = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '-' && (i == 0 || input[i - 1] == ',' || input[i - 1] == '('))
            {
                holder.Append('-');
                if (input[i + 2] == '.')
                {
                    isFloatingPoint = true;
                    holder.Append(input[i + 1]);
                    i++;
                }
                else
                {
                    isMultipartNumber = true;
                }
            }
            else if (isFloatingPoint)
            {
                if ((numbers.Contains(input[i]) || input[i] == '.') && i != input.Length - 1 && numbers.Contains(input[i + 1]))
                {
                    holder.Append(input[i].ToString());
                }
                else
                {
                    holder.Append(input[i].ToString());
                    result.Add(holder.ToString());
                    holder.Clear();
                    isFloatingPoint = false;
                }
            }
            else if (isMultipartNumber)
            {
                if (i != input.Length - 1 && numbers.Contains(input[i + 1]))
                {
                    holder.Append(input[i].ToString());
                }
                else
                {
                    holder.Append(input[i]);
                    result.Add(holder.ToString());
                    holder.Clear();
                    isMultipartNumber = false;
                }
            }
            else if (isFunction)
            {
                if (i != input.Length - 1 && !operators.Contains(input[i + 1]) && !brackets.Contains(input[i + 1]))
                {
                    holder.Append(input[i].ToString().ToLower());
                }
                else
                {
                    holder.Append(input[i].ToString().ToLower());
                    result.Add(holder.ToString());
                    holder.Clear();
                    isFunction = false;
                }
            }
            else if (operators.Contains(input[i]))
            {
                result.Add(input[i].ToString());
            }
            else if (brackets.Contains(input[i]))
            {
                result.Add(input[i].ToString());
            }
            else if (numbers.Contains(input[i]))
            {
                if (i != input.Length - 1 && input[i + 1] == '.')
                {
                    holder.Append(input[i]);
                    isFloatingPoint = true;
                }
                else if (i != input.Length - 1 && numbers.Contains(input[i + 1]))
                {
                    holder.Append(input[i]);
                    isMultipartNumber = true;
                }
                else
                {
                    result.Add(input[i].ToString());
                }
            }
            else if (input[i] == ',')
            {
                result.Add(",");
            }
            else
            {
                isFunction = true;
                holder.Append(input[i].ToString().ToLower());
            }
        }

        return result;
    }

    // The Shunting Yard converter to RPN itself
    // http://en.wikipedia.org/wiki/Shunting_yard_algorithm
    public static Queue<string> ConvertToRPNQueue(List<string> input)
    {
        Stack<string> stack = new Stack<string>();
        Queue<string> queue = new Queue<string>();
        for (int i = 0; i < input.Count; i++)
        {
            var token = input[i];
            double number;

            if (double.TryParse(token, out number))
            {
                queue.Enqueue(number.ToString());
            }
            else if (functions.Contains(token))
            {
                stack.Push(token);
            }
            else if (token == "," && stack.Count > 0)
            {
                if (!stack.Contains("("))
                {
                    throw new ArgumentException("Invalid expression!");
                }
                while (stack.Peek() != "(" && stack.Count > 0)
                {
                    queue.Enqueue(stack.Pop());
                }
            }
            else if (operators.Contains(token[0]))
            {
                while (stack.Count != 0)
                {
                    if ((stack.Peek() == "+" || stack.Peek() == "-") && (token == "*" || token == "/"))
                    {
                        break;
                    }
                    else if (operators.Contains(stack.Peek()[0]))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                stack.Push(token);
            }
            else if (token == "(")
            {
                stack.Push(token);
            }
            else if (token == ")")
            {
                if (!stack.Contains("("))
                {
                    throw new ArgumentException("Invalid expression!");
                }
                while (stack.Peek() != "(" && stack.Count > 0)
                {
                    queue.Enqueue(stack.Pop());
                }
                stack.Pop();
                if (stack.Count != 0 && functions.Contains(stack.Peek()))
                {
                    queue.Enqueue(stack.Pop());
                }
            }
        }
        if (stack.Contains("(") || stack.Contains(")"))
        {
            throw new ArgumentException("Invalid expression!");
        }
        while (stack.Count > 0)
        {
            queue.Enqueue(stack.Pop());
        }

        return queue;
    }

    // Convert the RPN queue to a comma seperated string.
    public static string ConvertToRPNString(Queue<string> input)
    {
        StringBuilder sb = new StringBuilder();
        while (input.Count > 0)
        {
            sb.Append(input.Dequeue());
            sb.Append(',');
        }

        return sb.ToString();
    }
    static void Main()
    {
        string expression = "(3+5.3) * 2.7 - ln(22) / pow(2.2, -1.7)";
        string trimmedExpression = TrimWhitespace(expression);
        var splitExpression = SplitInput(trimmedExpression);
        var RPNQueue = ConvertToRPNQueue(splitExpression);
        string RPNString = ConvertToRPNString(RPNQueue);

        Console.WriteLine(RPNString);
    }
}
