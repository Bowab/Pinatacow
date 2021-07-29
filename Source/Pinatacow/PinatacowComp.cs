using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Pinatacow
{
    /// <summary>
    /// Currently not in use.
    /// </summary>
    public class PinatacowComp : ThingComp
    {
        public override void CompTick()
        {
            base.CompTick();
        }

        public CompProperties_MilkableRandom Props
        {
            get
            {
                return (CompProperties_MilkableRandom)this.props;
            }
        }

    }
}
