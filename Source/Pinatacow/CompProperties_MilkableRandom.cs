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
            //this.compClass = typeof(PinatacowComp); // TODO: Might need when mod is extenden, this will run PinatacowComp.cs 

            this.compClass = typeof(CompMilkableRandom);
        }

        public int IntervalDays;
        public int Amount = 1;
        public List<ThingDef> defList;
        public bool FemaleOnly = true;

    }
}
