using System;
using System.Collections.Generic;

class ReversePolishNotation
{

    // Input the Reverse Polish Notation expression you want calculated
    // Elements must be sepparated by ","
    static double RPNCalculate(string rpn)
    {
        string[] split = rpn.Split(',');
        Stack<double> operands = new Stack<double>();
        Queue<string> operators = new Queue<string>();

        for (int i = 0; i < split.Length; i++)
        {
            double currentNumber;
            string operatorString;
            if (double.TryParse(split[i], out currentNumber))
            {
                operands.Push(currentNumber);
            }
            else
            {
                operatorString = split[i];
                switch (operatorString)
                {
                    case "+":
                        if (operands.Count < 2)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop(), secondOperand = operands.Pop();
                            double result = firstOperand + secondOperand;
                            operands.Push(result);
                        }
                        break;
                    case "-":
                        if (operands.Count < 2)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop(), secondOperand = operands.Pop();
                            double result = secondOperand - firstOperand;
                            operands.Push(result);
                        }
                        break;
                    case "*":
                        if (operands.Count < 2)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop(), secondOperand = operands.Pop();
                            double result = secondOperand * firstOperand;
                            operands.Push(result);
                        }
                        break;
                    case "/":
                        if (operands.Count < 2)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop(), secondOperand = operands.Pop();
                            double result = secondOperand / firstOperand;
                            operands.Push(result);
                        }
                        break;
                    case "ln":
                        if (operands.Count < 1)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop();
                            double result = Math.Log(firstOperand);
                            operands.Push(result);
                        }
                        break;
                    case "sqrt":

                        if (operands.Count < 1)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop();
                            double result = Math.Sqrt(firstOperand);
                            operands.Push(result);
                        }
                        break;
                    case "pow":
                        if (operands.Count < 2)
                        {
                            throw new ArgumentOutOfRangeException("Insufficient operands!");
                        }
                        else
                        {
                            double firstOperand = operands.Pop(), secondOperand = operands.Pop();
                            double result = Math.Pow(secondOperand, firstOperand);
                            operands.Push(result);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return operands.Pop();
    }

    static void Main()
    {
        string rpn = "5,1,2,+,4,*,+,3,-";
        double result = RPNCalculate(rpn);
        Console.WriteLine(result);
    }
}