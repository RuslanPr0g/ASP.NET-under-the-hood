using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Services
{
    public class RandomCounter : ICounter
    {
        static readonly Random Rnd = new Random();

        private readonly int _value;

        public RandomCounter()
        {
            _value = Rnd.Next(0, 1000000);
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
