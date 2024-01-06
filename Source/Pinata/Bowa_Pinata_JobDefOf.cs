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
        public static class Bowa_Pinata_JobDefOf
    {
            public static JobDef Bowa_Milk;

            static Bowa_Pinata_JobDefOf()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(Bowa_Pinata_JobDefOf));
            }
        }

    
}
