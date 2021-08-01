using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace Pinatacow
{
    public class PinataSettings : ModSettings
    {
        /// <summary>
        /// The three settings our mod has.
        /// </summary>
        public List<string> listOfMilkableItems = new List<string>()
        {
                "Milk",
                "Hay",
                "Steel",
                "Plasteel",
                "WoodLog",
                "Uranium",
                "Jade",
                "Cloth",
                "Synthread",
                "DevilstrandCloth",
                "Hyperweave",
                "Silver",
                "Gold",
                "RawPotatoes",
                "RawFungus",
                "RawRice",
                "RawAgave",
                "MedicineHerbal",
                "MedicineIndustrial",
                "MedicineUltratech",
                "ComponentIndustrial",
        };

        public float minAmount = 1f;
        public float maxAmount = 24f;

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref listOfMilkableItems, "listOfMilkableItems", LookMode.Value);
            Scribe_Values.Look(ref minAmount, "minAmount", 1f);
            Scribe_Values.Look(ref maxAmount, "maxAmount", 24f);
        }
    }
}
