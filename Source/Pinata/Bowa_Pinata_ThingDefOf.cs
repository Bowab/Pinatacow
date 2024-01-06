using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Pinata
{
    [DefOf]
    public static class Bowa_Pinata_ThingDefOf
    {
        public static ThingDef Bowab_Pinata;

        static Bowa_Pinata_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(Bowa_Pinata_ThingDefOf));
        }
    }
}
