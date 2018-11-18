using DummyCalculator.Plugins.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DummyCalculator.Plugins.Fibonacci
{
    class Program : CalculatorOperation
    {
        private List<int> _numbers;

        public Program(params string[] args)
            : base(args)
        {
            _numbers = new List<int>();
        }

        public override string Name => "Fibonacci";

        public override string OperatorSymbol => "|";

        static void Main(string[] args)
        {
            new Program(args).Start();
        }

        public override void Start()
        {
            Console.WriteLine($"Invoking {Name} operation");

            var count = Operands.FirstOrDefault();
            if (count < 1)
                count = 100;

            for (int i = 0; i < count - 1; i++)
            {
                Calculate();

                Console.WriteLine(ToString());

                Thread.Sleep(1000);
            }
        }

        public override void Calculate()
        {
            if (_numbers.Count < 2)
            {
                _numbers.Add(1);
            }
            else
            {
                var newNumber = _numbers.Last() + _numbers.ElementAt(_numbers.Count - 2);
                _numbers.Add(newNumber);
            }
        }

        public override string ToString()
        {
            return string.Join($" {OperatorSymbol} ", _numbers);
        }
    }
}
