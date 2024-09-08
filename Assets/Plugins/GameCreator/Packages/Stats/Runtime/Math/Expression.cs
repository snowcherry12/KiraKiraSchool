using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    public class Expression
    {
        private const int INIT_CAPACITY = 32;

        private static readonly List<string> CreateTokensList = new List<string>(INIT_CAPACITY);
        private static readonly Stack<string> OperatorStack = new Stack<string>(INIT_CAPACITY);
        private static readonly Queue<string> OutputQueue = new Queue<string>(INIT_CAPACITY);
        
        private static readonly Stack<string> EvaluationStack = new Stack<string>(INIT_CAPACITY);
        private static readonly List<double> EvaluationValues = new List<double>(INIT_CAPACITY);
        
        private static readonly char[] NUMBER_DESIGNATORS = {'f', 'd', 'l'};

        private static readonly Dictionary<string, Operator> Operators = new Dictionary<string, Operator>
        {
            { "-",               new Operator(Function.Sub,             2, 2, Associativity.L) },
            { "+",               new Operator(Function.Add,             2, 2, Associativity.L) },
            { "/",               new Operator(Function.Div,             3, 2, Associativity.L) },
            { "*",               new Operator(Function.Mul,             3, 2, Associativity.L) },
            { "%",               new Operator(Function.Mod,             3, 2, Associativity.L) },
            { "^",               new Operator(Function.Pow,             5, 2, Associativity.R) },
            { "_",               new Operator(Function.Neg,             5, 1, Associativity.L) },
            { "sqrt",            new Operator(Function.Sqrt,            4, 1, Associativity.L) },
            { "cos",             new Operator(Function.Cos,             4, 1, Associativity.L) },
            { "sin",             new Operator(Function.Sin,             4, 1, Associativity.L) },
            { "tan",             new Operator(Function.Tan,             4, 1, Associativity.L) },
            { "floor",           new Operator(Function.Floor,           4, 1, Associativity.L) },
            { "ceil",            new Operator(Function.Ceil,            4, 1, Associativity.L) },
            { "min",             new Operator(Function.Min,             4, 2, Associativity.L) },
            { "max",             new Operator(Function.Max,             4, 2, Associativity.L) },
            { "round",           new Operator(Function.Round,           4, 1, Associativity.L) },
            { "random",          new Operator(Function.Random,          4, 2, Associativity.L) },
            { "dice",            new Operator(Function.Dice,            4, 2, Associativity.L) },
            { "chance",          new Operator(Function.Chance,          4, 1, Associativity.L) },
            { "table.level",     new Operator(Function.Table_Level,     4, 1, Associativity.L) },
            { "table.value",     new Operator(Function.Table_Value,     4, 1, Associativity.L) },
            { "table.increment", new Operator(Function.Table_Increment, 4, 1, Associativity.L) },
            { "table.current",   new Operator(Function.Table_Current,   4, 1, Associativity.L) },
            { "table.next",      new Operator(Function.Table_Next,      4, 1, Associativity.L) },
            { "table.ratio",     new Operator(Function.Table_Ratio,     4, 1, Associativity.L) },
        };

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly string[] m_RPNTokens;
        [NonSerialized] private readonly Dictionary<PropertyName, int> m_InputDecoder;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        internal Expression(string input)
        {
            input = CleanInput(input);
            string[] infixTokens = InputToTokens(input);

            infixTokens = SetupVariables(infixTokens);
            infixTokens = FixUnaryOperators(infixTokens);

            this.m_RPNTokens = FormatToRPN(infixTokens);

            this.m_InputDecoder = new Dictionary<PropertyName, int>();

            foreach (string token in this.m_RPNTokens)
            {
                if (!IsVariable(token)) continue;
                
                int index = VariableIndex(token);
                if (index == -1) continue;
                
                this.m_InputDecoder[token] = index;
            }
        }

        // INTERNAL METHODS: ------------------------------------------------------------------------
        
        internal bool Evaluate(Domain domain, double[] inputs, out double value)
        {
            value = default;
            return EvaluateTokens(this.m_RPNTokens, domain, inputs, ref value);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static string CleanInput(string input)
        {
            string result = input.ToLowerInvariant();
            result = result.Trim();

            if (result.Length == 0)
            {
                return result;
            }

            char lastChar = result[^1];
            
            if (IsOperator(lastChar.ToString()))
            {
                result = result.TrimEnd(lastChar);
            }

            return result;
        }

        private static string[] InputToTokens(string input)
        {
            CreateTokensList.Clear();
            string currentString = "";

            foreach (char currentCharacter in input)
            {
                if (IsCommand(currentCharacter.ToString()))
                {
                    if (currentString.Length > 0)
                    {
                        CreateTokensList.Add(currentString);
                    }

                    CreateTokensList.Add(currentCharacter.ToString());
                    currentString = string.Empty;
                }
                else
                {
                    if (currentCharacter != ' ')
                    {
                        currentString += currentCharacter;
                    }
                    else
                    {
                        if (currentString.Length > 0)
                        {
                            CreateTokensList.Add(currentString);
                        }
                        
                        currentString = string.Empty;
                    }
                }
            }

            if (currentString.Length > 0)
            {
                CreateTokensList.Add(currentString);
            }
            
            return CreateTokensList.ToArray();
        }

        private static string[] SetupVariables(string[] tokens)
        {
            int index = 0;

            for (int i = 0; i < tokens.Length; ++i)
            {
                if (IsVariable(tokens[i]))
                {
                    tokens[i] = $"#{index}";
                    index += 1;
                }
            }

            return tokens;
        }
        
        private static string[] FixUnaryOperators(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                return tokens;
            }

            if (tokens[0] == "-")
            {
                tokens[0] = "_";
            }

            for (int i = 1; i < tokens.Length - 1; i++)
            {
                string token = tokens[i];
                string previousToken = tokens[i - 1];
                if (token == "-" && IsCommand(previousToken) && !IsCloseBracket(previousToken))
                {
                    tokens[i] = "_";
                }
            }
            
            return tokens;
        }
        
        // +--------------------------------------------------------------------------------------+
        // | Implementation of Shunting Yard algorithm that converts tokens to Reverse Polish     |
        // | Notation (RPN). See https://en.wikipedia.org/wiki/Shunting-yard_algorithm.           |
        // +--------------------------------------------------------------------------------------+
        
        private static string[] FormatToRPN(IEnumerable<string> tokens)
        {
            OperatorStack.Clear();
            OutputQueue.Clear();

            foreach (string token in tokens)
            {
                if (IsCommand(token))
                {
                    if (IsOpenBracket(token[0]))
                    {
                        OperatorStack.Push(token);
                    }
                    else if (IsCloseBracket(token[0]))
                    {
                        while (OperatorStack.Count > 0 && !IsOpenBracket(OperatorStack.Peek()))
                        {
                            OutputQueue.Enqueue(OperatorStack.Pop());
                        }

                        if (OperatorStack.Count > 0)
                        {
                            OperatorStack.Pop();
                        }

                        if (OperatorStack.Count > 0 && IsFunction(OperatorStack.Peek()))
                        {
                            OutputQueue.Enqueue(OperatorStack.Pop());
                        }
                    }
                    else if (token[0] == ',')
                    {
                        while (OperatorStack.Count > 0 && !IsOpenBracket(OperatorStack.Peek()))
                        {
                            OutputQueue.Enqueue(OperatorStack.Pop());
                        }
                    }
                    else
                    {
                        Operator tokenOperator = TokenToOperator(token);

                        while (NeedToPop(OperatorStack, tokenOperator))
                        {
                            OutputQueue.Enqueue(OperatorStack.Pop());
                        }

                        OperatorStack.Push(token);
                    }
                }
                else if (IsFunction(token))
                {
                    OperatorStack.Push(token);
                }
                else if (IsVariable(token))
                {
                    OutputQueue.Enqueue(token);
                }
                else
                {
                    OutputQueue.Enqueue(token);
                }
            }

            while (OperatorStack.Count > 0)
            {
                OutputQueue.Enqueue(OperatorStack.Pop());
            }

            return OutputQueue.ToArray();
        }
        
        // EVALUATION METHODS: --------------------------------------------------------------------

        private bool EvaluateTokens(IEnumerable<string> tokens, Domain domain, double[] inputs, ref double value)
        {
            EvaluationStack.Clear();

            foreach (string token in tokens)
            {
                if (IsOperator(token))
                {
                    Operator tokenOperator = TokenToOperator(token);
                    EvaluationValues.Clear();
                    bool parsed = true;

                    while (EvaluationStack.Count > 0 && 
                           !IsCommand(EvaluationStack.Peek()) && EvaluationValues.Count < tokenOperator.Inputs)
                    {
                        parsed &= this.TryParse(
                            EvaluationStack.Pop(),
                            inputs,
                            out double newValue
                        );
                        
                        EvaluationValues.Add(newValue);
                    }

                    EvaluationValues.Reverse();

                    if (parsed && EvaluationValues.Count == tokenOperator.Inputs)
                    {
                        double result = EvaluateOp(domain, EvaluationValues, tokenOperator.Type);
                        EvaluationStack.Push(result.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (IsVariable(token))
                {
                    EvaluationStack.Push(token);
                }
                else
                {
                    EvaluationStack.Push(token);
                }
            }

            return EvaluationStack.Count == 1 && this.TryParse(
                EvaluationStack.Pop(),
                inputs,
                out value
            );
        }

        private bool TryParse(string token, IReadOnlyList<double> inputs, out double result)
        {
            if (token.Length > 1 && char.IsDigit(token[^2]))
            {
                token = token.TrimEnd(NUMBER_DESIGNATORS);
            }

            result = default;

            if (token.Length == 0)
            {
                return true;
            }

            if (IsVariable(token))
            {
                int inputIndex = this.m_InputDecoder[token];
                result = inputIndex < inputs.Count 
                    ? inputs[inputIndex]
                    : default;
                
                return true;
            }

            if (token == "pi")
            {
                result = Math.PI;
                return true;
            }
            
            return double.TryParse(
                token, 
                NumberStyles.Float,
                CultureInfo.InvariantCulture.NumberFormat,
                out result
            );
        }
        
        private static double EvaluateOp(Domain domain, IReadOnlyList<double> values, Function function)
        {
            double a = values.Count >= 1 ? values[0] : 0;
            double b = values.Count >= 2 ? values[1] : 0;

            return function switch
            {
                Function.Neg => -a,
                Function.Add => a + b,
                Function.Sub => a - b,
                Function.Mul => a * b,
                Function.Div => a / b,
                Function.Mod => a % b,
                Function.Pow => Math.Pow(a, b),
                Function.Sqrt => a <= 0 ? 0 : Math.Sqrt(a),
                Function.Floor => Math.Floor(a),
                Function.Ceil => Math.Ceiling(a),
                Function.Round => Math.Round(a),
                Function.Min => Math.Min(a, b),
                Function.Max => Math.Max(a, b),
                Function.Cos => Math.Cos(a),
                Function.Sin => Math.Sin(a),
                Function.Tan => Math.Tan(a),
                Function.Random => RandomPCG.Range(a, b),
                Function.Dice => RandomPCG.Dice(a, b),
                Function.Chance => RandomPCG.Chance(a),
                Function.Table_Level => Functions.TableLevel(domain, a),
                Function.Table_Value => Functions.TableValue(domain, a),
                Function.Table_Increment => Functions.TableIncrement(domain, a),
                Function.Table_Current => Functions.TableExpForCurrentLevel(domain, a),
                Function.Table_Next => Functions.TableExpToNextLevel(domain, a),
                Function.Table_Ratio => Functions.TableRatioForCurrentLevel(domain, a),
                _ => throw new ArgumentOutOfRangeException(nameof(function), function, null)
            };
        }
        
        private static bool NeedToPop(Stack<string> operatorStack, Operator newOperator)
        {
            if (operatorStack.Count <= 0 || newOperator == null) return false;
            
            Operator topOfStack = TokenToOperator(operatorStack.Peek());
            if (topOfStack == null) return false;

            return newOperator.Associativity switch
            {
                Associativity.L when newOperator.Precedence <= topOfStack.Precedence => true,
                Associativity.R when newOperator.Precedence < topOfStack.Precedence => true,
                _ => false
            };
        }
        
        // PRIVATE HELPERS: -----------------------------------------------------------------------
        
        private static bool IsOperator(string token)
        {
            return Operators.ContainsKey(token);
        }
        
        private static bool IsCommand(string token)
        {
            if (token.Length == 1)
            {
                char firstCharacter = token[0];
                if (IsOpenBracket(firstCharacter)) return true;
                if (IsCloseBracket(firstCharacter)) return true;
                if (firstCharacter == ',') return true;
            }
            
            return IsOperator(token);
        }
        
        private static bool IsFunction(string token)
        {
            Operator symbol = TokenToOperator(token);
            if (symbol == null) return false;
            
            return symbol.Type switch
            {
                Function.Min => true,
                Function.Max => true,
                Function.Random => true,
                Function.Dice => true,
                Function.Chance => true,
                Function.Table_Level => true,
                Function.Table_Value => true,
                Function.Table_Increment => true,
                Function.Table_Current => true,
                Function.Table_Next => true,
                Function.Table_Ratio => true,
                _ => false
            };
        }
        
        private static bool IsVariable(string token)
        {
            return token.Length != 0 && token[0] == '#';
        }

        private static int VariableIndex(string token)
        {
            return token.Length > 1 && int.TryParse(
                token[1..],
                NumberStyles.Integer,
                CultureInfo.InvariantCulture.NumberFormat,
                out int index
            ) ? index : -1;
        }
        
        private static bool IsOpenBracket(string token) => token.Length == 1 && IsOpenBracket(token[0]);
        private static bool IsCloseBracket(string token) => token.Length == 1 && IsCloseBracket(token[0]);

        private static bool IsOpenBracket(char character)
        {
            return character switch
            {
                '(' => true,
                '[' => true,
                '{' => true,
                _ => false
            };
        }
        
        private static bool IsCloseBracket(char character)
        {
            return character switch
            {
                ')' => true,
                ']' => true,
                '}' => true,
                _ => false
            };
        }
        
        private static Operator TokenToOperator(string token)
        {
            return Operators.TryGetValue(token, out Operator tokenOperator)
                ? tokenOperator
                : null;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(" ", this.m_RPNTokens);
            
            return stringBuilder.ToString();
        }
    }
}