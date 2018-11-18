using System;
using System.Linq;

namespace DummyCalculator.Plugins.Abstractions
{
    public abstract class CalculatorOperation
    {
        public CalculatorOperation(params string[] args)
        {
            Operands = args
                .Select(a => double.TryParse(a, out var a1) ? a1 : 0)
                .Where(a => a > 0)
                .ToArray();
        }

        public abstract string Name { get; }

        public abstract string OperatorSymbol { get; }

        public abstract void Calculate();

        public double[] Operands { get; private set; }

        public double Result { get; protected set; }

        public virtual void Start()
        {
            Console.WriteLine($"Invoking {Name} operation");
            
            Calculate();

            Console.WriteLine(ToString());
        }
        
        public override string ToString()
        {
            var opr = string.Join($" {OperatorSymbol} ", Operands);

            return $"{opr} = {Result}";
        }
    }
}
