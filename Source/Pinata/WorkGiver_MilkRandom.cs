using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Pinata
{
    public class WorkGiver_MilkRandom : WorkGiver_GatherAnimalBodyResources
    {
        protected override JobDef JobDef
        {
            get
            {
                return Bowa_Pinata_JobDefOf.Bowa_Milk;
            }
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            if (animal.def.defName == "Bowab_Pinata")
            {
                return animal.TryGetComp<CompMilkableRandom>();
            }

            return animal.TryGetComp<CompMilkable>();
        }
    }
}
