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
        //public List<ThingDef> listOfMilkableItems = new List<ThingDef>();
        public List<string> listOfMilkableItems = new List<string>();

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref listOfMilkableItems, "listOfMilkableItems", LookMode.Value);

            //bool flag = Scribe.mode == LoadSaveMode.LoadingVars;
            //if (flag)
            //{
            //    List<string> list = new List<string>();
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
            // Standard
            //Listing_Standard listingStandard = new Listing_Standard();
            //listingStandard.Begin(inRect);
            //listingStandard.End();

            // Eget experiment
            // Rect skit
            Rect rect1 = inRect.TopPart(0.10f);
            bool flag = Widgets.ButtonText(rect1, "Add to list", true, true, true);
            if (flag)
            {
                Log.Message("flag knappen är öppen: " + flag.ToString());

                List<FloatMenuOption> list = new List<FloatMenuOption>();

                var possibleMilkedItems = PossibleThingDefs();

                foreach (var item in possibleMilkedItems)
                {
                    if (!settings.listOfMilkableItems.Contains(item.defName))
                    {
                        // Show label of item, but save it as defName, since these are unique.
                        list.Add(new FloatMenuOption(item.label, delegate ()
                        {
                            settings.listOfMilkableItems.Add(item.defName); // This is what adds the item to the list.
                        }));
                    }
                }
                FloatMenu window = new FloatMenu(list);
                Find.WindowStack.Add(window);
            }


            // Lista med saker som man ser.
            var thingDefs = PossibleThingDefs(); // TODO: Lägg på bättre ställe?
            Log.Message("Jag spammar, inte bra!");


            var startHeight = 128f;
            Rect position = rect1.BottomPart(0.32f).Rounded();

            //GUI.BeginGroup(position);

            Rect viewRect = new Rect(0f, 0f, position.width, 0f);
            
            foreach (var item in settings.listOfMilkableItems)
            {
                Rect rect2 = new Rect(0f, startHeight, viewRect.width, 32f);

                var thingDef = thingDefs.FirstOrDefault(x => x.defName == item);
                if (thingDef != null)
                {
                    Widgets.Label(rect2, thingDef.label);
                    Widgets.ThingIcon(rect2, thingDef);

                }
                startHeight = startHeight + 32f; // öka på så att varje item i listan hamnar på egen rad.
            }
            //GUI.EndGroup();




            base.DoSettingsWindowContents(inRect);
        }

        private float scrollViewHeight = 0f;
        private Vector2 scrollPosition = Vector2.zero;

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
                   where d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop && !d.MadeFromStuff
                   select d;
        }
    }
}
