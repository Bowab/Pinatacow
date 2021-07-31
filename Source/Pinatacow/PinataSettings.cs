using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;
using SettingsHelper;

namespace Pinatacow
{
    public class PinataSettings : ModSettings
    {
        /// <summary>
        /// The three settings our mod has.
        /// </summary>
        public List<ThingDef> listOfMilkableItems = new List<ThingDef>();
        public List<string> listOfMilkableItemsAsString = new List<string>();

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref listOfMilkableItemsAsString, "listOfMilkableItems", LookMode.Value);

            /*Scribe_Collections.Look(ref exampleListOfPawns, "exampleListOfPawns", LookMode.Reference);*/ // Remove

            //bool flag = Scribe.mode == LoadSaveMode.LoadingVars;
            //if (flag)
            //{
            //    List<ThingDef> list = new List<ThingDef>();
            //    bool flag2 = false;
            //    for (int i = 0; i < listOfMilkableItems.Count; i++)
            //    {
            //        bool flag3 = listOfMilkableItems[i] != null;
            //        Log.Message("Vad är flag3? " + flag3.ToString());
            //        if (flag3)
            //        {
            //            list.Add(listOfMilkableItems[i]);
            //        }
            //        else
            //        {
            //            bool flag4 = !flag2;
            //            if (flag4)
            //            {
            //                flag2 = true;
            //                Log.Warning("Pinata:: Found 1 or more null entries in listOfMilkableItems. This is most likely due to an uninstalled mod. Removing entries from list.");
            //            }
            //        }
            //    }
            //    listOfMilkableItems = list;
            //}

        }
    }

    public class PinataMod : Mod
    {
        /// <summary>
        /// A reference to our settings.
        /// </summary>
        PinataSettings settings;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public PinataMod(ModContentPack content) : base(content)
        {
            //LongEventHandler.ExecuteWhenFinished(new Action(this.GetSettings));
            this.settings = GetSettings<PinataSettings>();

        }

        //public void GetSettings()
        //{
        //    this.settings = base.GetSettings<PinataSettings>();
        //}

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);


            // Eget experiment
            // Rect skit
            Rect rect1 = inRect.TopPart(0.25f);
            bool flag = Widgets.ButtonText(rect1, "Add to list", true, true, true);
            Log.Message("ngn flag: " + flag.ToString());
            if (flag)
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                var possibleMilkedItems = PossibleThingDefs();

                foreach (var item in possibleMilkedItems)
                {

                    //if (!settings.listOfMilkableItems.Contains(item))
                    //{
                    //    list.Add(new FloatMenuOption(item.label, delegate ()
                    //    {
                    //        settings.listOfMilkableItems.Add(item); // This is what adds the ThingDef to the list.

                    //    }));
                    //}

                    if (!settings.listOfMilkableItemsAsString.Contains(item.label))
                    {
                        list.Add(new FloatMenuOption(item.label, delegate ()
                        {
                            settings.listOfMilkableItemsAsString.Add(item.label);
                        }));
                    }
                }
                FloatMenu window = new FloatMenu(list);
                Find.WindowStack.Add(window);
            }



            // Standard
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localisation.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory()
        {
            return "Pinata".Translate();
        }


        public static IEnumerable<ThingDef> PossibleThingDefs()
        {
            return from d in DefDatabase<ThingDef>.AllDefs
                   where d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop && !d.MadeFromStuff && (d.GetCompProperties<CompProperties_Rottable>() == null)
                   select d;
        }
    }
}
