using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPillars
{
    class Octopus : Animal
    {
        public override int NumberOfAppendages
        {
            get => base.NumberOfAppendages;
            set
            {
                if (value > 8)
                    throw new ArgumentOutOfRangeException(nameof(NumberOfAppendages));

                base.whatever = value;
            }
        }
    }
}
