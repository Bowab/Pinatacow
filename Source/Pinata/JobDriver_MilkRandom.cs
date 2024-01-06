using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Pinata
{

    public class JobDriver_MilkRandom : JobDriver_GatherAnimalBodyResources
    {
        protected override float WorkTotal
        {
            get
            {
                return 400f;
            }
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            // Should not be required here since this code should not run for other animals.
            if (animal.def != Bowa_Pinata_ThingDefOf.Bowab_Pinata)
            {
                return animal.TryGetComp<CompMilkable>();
            }

            // We have to run the random item selector here because of Gathered method in CompHasGatherableBodyResource.cs, it sometimes loop.
            var itemToSpawnAsString = PinataMod.PossibleMilkableItems.RandomElement();
            var itemToSpawn = PinataMod.PossibleThingDefs().FirstOrDefault(x => x.defName == itemToSpawnAsString);

            // Spawn milk as a backup.
            if (itemToSpawn == null)
                itemToSpawn = PinataMod.PossibleThingDefs().FirstOrDefault(x => x.defName == "Milk");

            var compMilkableRandom = animal.TryGetComp<CompMilkableRandom>();

            compMilkableRandom.ItemToSpawn = itemToSpawn;

            return compMilkableRandom;
        }

    }
}
