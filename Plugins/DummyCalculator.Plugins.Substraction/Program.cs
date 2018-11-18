using DummyCalculator.Plugins.Abstractions;
using System;
using System.Linq;

namespace DummyCalculator.Plugins.Substraction
{
    public class Program : CalculatorOperation
    {
        public Program(params string[] args)
            : base(args)
        {

        }

        public override string Name => "Substraction";

        public override string OperatorSymbol => "-";

        static void Main(string[] args)
        {
            new Program(args).Start();
        }

        public override void Calculate()
        {
            double result = 0;
            var index = 0;
            foreach (var op in Operands)
            {
                if (index == 0)
                    result = op;
                else
                    result -= op;

                index++;
            }

            Result = result;
        }
    }
}
