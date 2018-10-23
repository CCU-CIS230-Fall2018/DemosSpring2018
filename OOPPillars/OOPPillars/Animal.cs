using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPillars
{
    class Animal
    {
        protected int whatever;

        public virtual int NumberOfAppendages
        {
            get { return whatever; }
            set
            {
                if (value > 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(NumberOfAppendages));
                }

                whatever = value;
            }
        }

        public string Name { get; set; }
    }
}
