using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace equations
{
    #region enums
    enum TokenTypes
    {
        Operator,
        Number,
        Parenthesis
    }
    enum Associativenesses
    {
        Left,
        Right
    }
    enum OperatorTypes { Plus, Minus, Multiply, Divide, Equals, Exclamation, Modulus }
    enum ParenthesisTypes { Open, Close }
    #endregion

    #region classes
    public class Token { }

    class Operator : Token
    {
        public OperatorTypes OperatorType { get; set; }
        public Operator(OperatorTypes operatorType) { OperatorType = operatorType; }
        public int Precedence
        {
            get
            {
                switch (this.OperatorType)
                {
                    case OperatorTypes.Exclamation:
                        return 4;
                    case OperatorTypes.Multiply:
                    case OperatorTypes.Divide:
                    case OperatorTypes.Modulus:
                        return 3;
                    case OperatorTypes.Plus:
                    case OperatorTypes.Minus:
                        return 2;
                    case OperatorTypes.Equals:
                        return 1;
                    default:
                        throw new Exception("Invalid Operator Type for Precedence get");
                }
            }
        }
        public Associativenesses Associativeness
        {
            get
            {
                switch (this.OperatorType)
                {
                    case OperatorTypes.Equals:
                    case OperatorTypes.Exclamation:
                        return Associativenesses.Right;
                    case OperatorTypes.Plus:
                    case OperatorTypes.Minus:
                    case OperatorTypes.Multiply:
                    case OperatorTypes.Divide:
                    case OperatorTypes.Modulus:
                        return Associativenesses.Left;
                    default:
                        throw new Exception("Invalid Operator Type for Associativeness get");
                }
            }
        }
        public override string ToString()
        {
            switch (OperatorType)
            {
                case OperatorTypes.Plus: return "+";
                case OperatorTypes.Minus: return "-";
                case OperatorTypes.Multiply: return "*";
                case OperatorTypes.Divide: return "/";
                case OperatorTypes.Equals: return "=";
                case OperatorTypes.Exclamation: return "!";
                case OperatorTypes.Modulus: return "%";
                default: return null;
            }
        }
        public static OperatorTypes? GetOperatorType(string operatorValue)
        {
            switch (operatorValue)
            {
                case "+": return OperatorTypes.Plus;
                case "-": return OperatorTypes.Minus;
                case "*": return OperatorTypes.Multiply;
                case "/": return OperatorTypes.Divide;
                case "=": return OperatorTypes.Equals;
                case "!": return OperatorTypes.Exclamation;
                case "%": return OperatorTypes.Modulus;
                default: return null;
            }
        }
    }

    class Parenthesis : Token
    {
        public ParenthesisTypes ParenthesisType { get; set; }
        public Parenthesis(ParenthesisTypes parenthesisType) { ParenthesisType = parenthesisType; }
        public override string ToString() { if (ParenthesisType == ParenthesisTypes.Open) return "("; else return ")"; }
        public static ParenthesisTypes? GetParenthesisType(string parenthesisValue)
        {
            switch (parenthesisValue)
            {
                case "(": return ParenthesisTypes.Open;
                case ")": return ParenthesisTypes.Close;
                default: return null;
            }
        }
    }

    class Numeric : Token
    {
        public decimal Value { get; set; }
        public Numeric(decimal value) { Value = value; }
        public override string ToString() { return Value.ToString(); }
    }

    public class Formula
    {
        public Stack<Token> InfixTokens { get; set; }
        public Stack<Token> PostfixTokens { get; set; }
        public string RawFormula { get; set; }

        public Formula(string rawFormula)
        {
            // store the raw formula
            RawFormula = rawFormula;
            InfixTokens = new Stack<Token>();
            PostfixTokens = new Stack<Token>();

            #region generate the InFix Stack
            Stack<Token> tokens = new Stack<Token>();
            string store = "";

            // parse the formula into a stack of tokens
            while (rawFormula.Length > 0)
            {
                string ThisChar = rawFormula.Substring(0, 1);

                if (Regex.IsMatch(ThisChar, "[0-9\\.]"))
                {
                    // a numeric char, so store it until the number is reached
                    store += ThisChar;

                }
                else if (Operator.GetOperatorType(ThisChar) != null)
                {
                    // a value is stored, so add it to the stack before processing the operator
                    if (store != "")
                    {
                        tokens.Push(new Numeric(Convert.ToDecimal(store)));
                        store = "";
                    }
                    tokens.Push(new Operator((OperatorTypes)Operator.GetOperatorType(ThisChar)));
                }
                else if (Parenthesis.GetParenthesisType(ThisChar) != null)
                {
                    // a value is stored, so add it to the stack before processing the parenthesis
                    if (store != "")
                    {
                        tokens.Push(new Numeric(Convert.ToDecimal(store)));
                        store = "";
                    }
                    tokens.Push(new Parenthesis((ParenthesisTypes)Parenthesis.GetParenthesisType(ThisChar)));
                }
                else
                {
                    // ignore blanks (unless between to numbers)
                    if (!(ThisChar == " " && !(store != "" && Regex.IsMatch(rawFormula.Substring(1, 1), "[0-9\\.]"))))
                    {
                        throw new Exception("Invalid character in Formula: " + ThisChar);
                    }
                }
                // move to the next position
                rawFormula = rawFormula.Substring(1);
            }

            // if there is still something in the numeric store, add it to the stack
            if (store != "")
            {
                tokens.Push(new Numeric(Convert.ToDecimal(store)));
            }

            // reverse the stack
            Stack<Token> reversedStack = new Stack<Token>();
            while (tokens.Count > 0) reversedStack.Push(tokens.Pop());

            // store in the Tokens property
            InfixTokens = reversedStack;
            #endregion

            #region generate the PostFix Stack
            // get a reversed copy of the tokens
            Stack<Token> infixTokens = new Stack<Token>(InfixTokens);
            Stack<Token> InFixStack = new Stack<Token>();
            while (infixTokens.Count > 0) InFixStack.Push(infixTokens.Pop());
            // new stacks
            Stack<Token> output = new Stack<Token>();
            Stack<Token> operators = new Stack<Token>();

            while (InFixStack.Count > 0)
            {
                Token currentToken = InFixStack.Pop();

                // if it's an operator
                if (currentToken.GetType() == typeof(Operator))
                {
                    // move precedent operators to output
                    while (operators.Count > 0 && operators.Peek().GetType() == typeof(Operator))
                    {
                        Operator currentOperator = (Operator)currentToken;
                        Operator nextOperator = (Operator)operators.Peek();
                        if ((currentOperator.Associativeness == Associativenesses.Left && currentOperator.Precedence <= nextOperator.Precedence)
                            || (currentOperator.Associativeness == Associativenesses.Right && currentOperator.Precedence < nextOperator.Precedence))
                        {
                            output.Push(operators.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    // add to operators
                    operators.Push(currentToken);
                }
                // if it's a bracket
                else if (currentToken.GetType() == typeof(Parenthesis))
                {
                    switch (((Parenthesis)currentToken).ParenthesisType)
                    {
                        // if it's an opening bracket, add it to operators
                        case ParenthesisTypes.Open:
                            operators.Push(currentToken);
                            break;
                        // if it's a closing bracket
                        case ParenthesisTypes.Close:
                            // shift operators in between opening to output 
                            while (operators.Count > 0)
                            {
                                Token nextOperator = operators.Peek();
                                if (nextOperator.GetType() == typeof(Parenthesis) && ((Parenthesis)nextOperator).ParenthesisType == ParenthesisTypes.Open) break;
                                output.Push(operators.Pop());
                            }
                            // add to operators
                            operators.Pop();
                            break;
                    }
                }
                // if it's numeric, add to output
                else if (currentToken.GetType() == typeof(Numeric))
                {
                    output.Push(currentToken);
                }

            }

            // for all remaining operators, move to output
            while (operators.Count > 0)
            {
                output.Push(operators.Pop());
            }

            // reverse the stack
            reversedStack = new Stack<Token>();
            while (output.Count > 0) reversedStack.Push(output.Pop());

            // store in the Tokens property
            PostfixTokens = reversedStack;
            #endregion
        }

        public decimal Calculate()
        {
            Stack<Numeric> EvaluationStack = new Stack<Numeric>();
            // get a reversed copy of the tokens
            Stack<Token> postFixStack = new Stack<Token>(PostfixTokens);
            Stack<Token> PostFixStack = new Stack<Token>();
            while (postFixStack.Count > 0) PostFixStack.Push(postFixStack.Pop());

            while (PostFixStack.Count > 0)
            {
                Token currentToken = PostFixStack.Pop();

                if (currentToken.GetType() == typeof(Numeric))
                {
                    EvaluationStack.Push((Numeric)currentToken);
                }
                else if (currentToken.GetType() == typeof(Operator))
                {
                    Operator currentOperator = (Operator)currentToken;
                    if (currentOperator.OperatorType == OperatorTypes.Plus
                || currentOperator.OperatorType == OperatorTypes.Minus
                || currentOperator.OperatorType == OperatorTypes.Multiply
                || currentOperator.OperatorType == OperatorTypes.Divide)
                    {
                        decimal FirstValue = EvaluationStack.Pop().Value;
                        // Handle -8
                        decimal SecondValue = EvaluationStack.Count > 0 ? EvaluationStack.Pop().Value : 0;
                        decimal Result;

                        if (currentOperator.OperatorType == OperatorTypes.Plus)
                        {
                            Result = SecondValue + FirstValue;
                        }
                        else if (currentOperator.OperatorType == OperatorTypes.Minus)
                        {
                            Result = SecondValue - FirstValue;
                        }
                        else if (currentOperator.OperatorType == OperatorTypes.Divide)
                        {
                            Result = SecondValue / FirstValue;
                        }
                        else if (currentOperator.OperatorType == OperatorTypes.Multiply)
                        {
                            Result = SecondValue * FirstValue;
                        }
                        else
                        {
                            throw new Exception("Unhandled operator in Formula.Calculate()");
                        }
                        EvaluationStack.Push(new Numeric(Result));
                        Console.WriteLine("EVAL: " + SecondValue.ToString() + " " + currentOperator.ToString() + " " + FirstValue.ToString() + " = " + Result.ToString());
                    }
                }
                else
                {
                    throw new Exception("Unexpected Token type in Formula.Calculate");
                }
            }

            if (EvaluationStack.Count != 1)
            {
                throw new Exception("Unexpected number of Tokens in Formula.Calculate");
            }
            return EvaluationStack.Peek().Value;
        }
    }
    #endregion

    class ExpressionResult
    {
        public string infixForm;
        public decimal leftValue;
        public decimal rightValue;
    }

    class EquationCorrection
    {

        public List<ExpressionResult> solutions = new List<ExpressionResult>();

        public EquationCorrection(string expr)
        {
            if (CheckFormula(expr))
            {
                return;
            }

            int idx = expr.IndexOf('=');
            string sNew = Replace(expr, idx, "-");
            for (int k = 0; k < sNew.Length; k++)
            {
                if (k == idx || sNew[k].ToString() != "-")
                {
                    continue;
                }

                string s2 = Replace(sNew, k, "=");
                if (CheckFormula(s2))
                {
                    return;
                }
            }


            for (int i = 0; i < expr.Length; i++)
            {
                foreach (string it in GetTransformation(expr[i].ToString()))
                {
                    sNew = Replace(expr, i, it);
                    if (CheckFormula(sNew))
                    {
                        return;
                    }
                }

                foreach (string it in GetTransformationRemove(expr[i].ToString()))
                {
                    string s = expr.Remove(i, 1);
                    s = s.Insert(i, it);

                    for (int j = i + 1; j < s.Length; j++)
                    {
                        foreach (string addT in GetTransformationAdd(s[j].ToString()))
                        {
                            sNew = Replace(s, j, addT);
                            if (CheckFormula(sNew))
                            {
                                return;
                            }
                        }
                    }

                    for (int j = i - 1; j >= 0; j--)
                    {
                        foreach (string addT in GetTransformationAdd(s[j].ToString()))
                        {
                            sNew = Replace(s, j, addT);
                            if (CheckFormula(sNew))
                            {
                                return;
                            }
                        }
                    }

                }
            }
        }

        string[] GetTransformationRemove(string digit)
        {
            switch (digit)
            {
                case "6":
                    return new string[1] { "5" };
                case "7":
                    return new string[1] { "1" };
                case "8":
                    return new string[3] { "0", "6", "9" };
                case "9":
                    return new string[2] { "3", "5" };
                default:
                    return new string[] { };
            }

        }

        string[] GetTransformationAdd(string digit)
        {
            switch (digit)
            {
                case "-":
                    return new string[1] { "+" };
                case "0":
                    return new string[1] { "8" };
                case "1":
                    return new string[1] { "7" };
                case "3":
                    return new string[1] { "9" };
                case "5":
                    return new string[2] { "6", "9" };
                case "6":
                    return new string[1] { "8" };
                case "9":
                    return new string[1] { "8" };
                default:
                    return new string[] { };
            }

        }

        string[] GetTransformation(string digit)
        {
            switch (digit)
            {
                case "+":
                    return new string[1] { "-" };
                case "0":
                    return new string[2] { "6", "9" };
                case "2":
                    return new string[1] { "3" };
                case "3":
                    return new string[2] { "2", "5" };
                case "5":
                    return new string[1] { "3" };
                case "6":
                    return new string[2] { "0", "9" };
                case "9":
                    return new string[2] { "0", "6" };
                default:
                    return new string[] { };
            }

        }

        bool CheckFormula(string s)
        {
            string[] d = s.Split('=');
            decimal v1 = new Formula(d[0]).Calculate();
            decimal v2 = new Formula(d[1]).Calculate();

            if (v1 == v2)
            {
                ExpressionResult r = new ExpressionResult
                {
                    infixForm = s,
                    leftValue = v1,
                    rightValue = v2,
                };
                solutions.Add(r);

                return true;
            }

            return false;
        }

        string Replace(string Base, int idx, string replVal)
        {
            string s = Base.Remove(idx, 1);
            s = s.Insert(idx, replVal);
            return s;
        }
    }

    class Digitizer
    {

        public Digitizer(string input)
        {
            EquationCorrection a = new EquationCorrection(input);

            if (a.solutions.Count > 0)
            {
                Console.WriteLine("sol " + a.solutions[0].infixForm);
            }
            else
            {
                Console.WriteLine("not foun");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "93+27-30+16=68",
                "78+23-89+82=94+2-18"
            };

            foreach (string item in inputs)
            {
                new Digitizer(item);
            }

            Console.ReadLine();
        }
    }
}
