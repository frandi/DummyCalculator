using DummyCalculator.Plugins.Abstractions;
using System;

namespace DummyCalculator.Plugins.Addition
{
    public class Program : CalculatorOperation
    {
        public Program(params string[] args)
            :base(args)
        {

        }

        public override string Name => "Addition";

        public override string OperatorSymbol => "+";

        static void Main(string[] args)
        {
            new Program(args).Start();
        }

        public override void Calculate()
        {
            double result = 0;
            foreach (var op in Operands)
            {
                result += op;
            }

            Result = result;
        }
    }
}
