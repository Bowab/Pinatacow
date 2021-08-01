using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Pinatacow
{
    public class CompProperties_MilkableRandom : CompProperties
    {
        public CompProperties_MilkableRandom()
        {
            this.compClass = typeof(CompMilkableRandom);
        }

        public int intervalDays;
        public bool FemaleOnly = true;
    }
}
