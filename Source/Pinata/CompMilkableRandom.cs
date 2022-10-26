using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Pinata
{
    public class CompMilkableRandom : CompHasGatherableBodyResource
    {
        protected override int GatherResourcesIntervalDays
        {
            get
            {
                return this.Props.intervalDays;
            }
        }

        /// <summary>
        /// This is set in <see cref="JobDriver_MilkRandom"/>
        /// </summary>
        public ThingDef ItemToSpawn { get; set; }

        protected override int ResourceAmount
        {
            get
            {
                if (ItemToSpawn != null && ItemToSpawn.stackLimit > 1)
                {
                    var rng = new Random();
                    return rng.Next((int)PinataMod.MinAmount, (int)PinataMod.MaxAmount + 1);
                }
                return 1;
            }
        }

        // This is what returns what object is being gathered.
        protected override ThingDef ResourceDef
        {
            get
            {
                if (ItemToSpawn == null)
                    ItemToSpawn = PinataMod.PossibleThingDefs().FirstOrDefault(x => x.defName == "Milk"); // Backup.

                return ItemToSpawn;
            }
        }

        protected override string SaveKey
        {
            get
            {
                return "Bowab_pinataFullness";
            }
        }

        public CompProperties_MilkableRandom Props
        {
            get
            {
                return (CompProperties_MilkableRandom)this.props;
            }
        }

        protected override bool Active
        {
            get
            {
                if (!base.Active)
                {
                    return false;
                }
                Pawn pawn = this.parent as Pawn;
                return (!this.Props.FemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
            }
        }

        public override string CompInspectStringExtra()
        {
            if (!this.Active)
            {
                return null;
            }
            return "Bowab_PinataFullness".Translate() + ": " + base.Fullness.ToStringPercent();
        }
    }
}
