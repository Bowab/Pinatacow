using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Pinata
{
    public class CompProperties_MilkableRandom : CompProperties
    {
        public CompProperties_MilkableRandom()
        {
            compClass = typeof(CompMilkableRandom);
        }

        public int intervalDays;
        public bool FemaleOnly = true;
    }
}
