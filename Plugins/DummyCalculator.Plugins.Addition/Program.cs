using DummyCalculator.Plugins.Abstractions;

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

        public override OperationType Type => OperationType.Addition;

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
