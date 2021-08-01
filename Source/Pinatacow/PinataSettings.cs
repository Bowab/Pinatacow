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

        IEnumerable<ThingDef> allThingDefs = new List<ThingDef>();

        List<string> originalList = new List<string>();


        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public PinataMod(ModContentPack content) : base(content)
        {
            //LongEventHandler.ExecuteWhenFinished(new Action(this.GetSettings));
            this.settings = GetSettings<PinataSettings>();
            this.allThingDefs = PossibleThingDefs();

            List<string> resetList = new List<string>()
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

            this.originalList = resetList;
        }

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


            #region Add item

            Rect rect1 = inRect.TopPart(0.10f);
            bool flag = Widgets.ButtonText(rect1, "Add to list", true, true, true);
            if (flag)
            {
                Log.Message("flag knappen är öppen: " + flag.ToString());

                List<FloatMenuOption> list = new List<FloatMenuOption>();

                var possibleMilkedItems = this.allThingDefs;

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

            #endregion Add item

            #region Remove item

            Rect removeRect = new Rect(0f, 96f, 200f, 32f);
            bool removeFlag = Widgets.ButtonText(removeRect, "Remove item", true, true, true);
            if (removeFlag)
            {
                var possibleMilkedItems = this.allThingDefs;
                List<FloatMenuOption> listOfItems = new List<FloatMenuOption>();
                var loopMe = settings.listOfMilkableItems;
                foreach (var item in loopMe)
                {
                    var thingDef = possibleMilkedItems.FirstOrDefault(x => x.defName == item);
                    listOfItems.Add(new FloatMenuOption(thingDef.label, delegate ()
                    {
                        settings.listOfMilkableItems.Remove(thingDef.defName);

                    }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
                }
                FloatMenu window2 = new FloatMenu(listOfItems);
                Find.WindowStack.Add(window2);
            }

            #endregion Remove item

            #region Reset list of items

            Rect resetList = new Rect(0f, 128f, 200f, 32f);
            bool resetFlag = Widgets.ButtonText(resetList, "Reset list", true, true, true);
            if (resetFlag)
            {
                settings.listOfMilkableItems.Clear();
                foreach (var item in originalList)
                {
                    settings.listOfMilkableItems.Add(item);
                }
            }

            #endregion Reset list of items


            #region Empty the list

            Rect emptyList = new Rect(0f, 160f, 200f, 32f);
            bool emptyFlag = Widgets.ButtonText(emptyList, "Empty the list", true, true, true);
            if (emptyFlag)
            {
                settings.listOfMilkableItems.Clear();
            }

            #endregion Empty the list

            #region The list

            var startHeight = 160f;
            Rect position = inRect.TopPart(0.75f);
            GUI.BeginGroup(position);
            Rect outRect = new Rect(0f, 160f, position.width, position.height);
            Rect viewRect = new Rect(0f, 160f, position.width, this.scrollViewHeight);
            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);

            foreach (var item in settings.listOfMilkableItems)
            {
                Rect rectList = new Rect(0f, startHeight, viewRect.width, 32f);

                var thingDef = this.allThingDefs.FirstOrDefault(x => x.defName == item);
                if (thingDef != null)
                {
                    Widgets.Label(rectList, thingDef.label);
                    Widgets.ThingIcon(rectList, thingDef);

                }
                startHeight = startHeight + 32f; // Increase everytime to make the new row in the list.
                this.scrollViewHeight = startHeight;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();

            #endregion The list

            base.DoSettingsWindowContents(inRect);
        }

        private float scrollViewHeight = 96f;
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

        /// <summary>
        /// Final method that runs
        /// </summary>
        public override void WriteSettings()
        {
            // Make sure we have atleast one possible item.
            if (settings.listOfMilkableItems.Count <= 0)
                settings.listOfMilkableItems.Add("Milk");

            base.WriteSettings();
        }

        public static IEnumerable<ThingDef> PossibleThingDefs()
        {
            return from d in DefDatabase<ThingDef>.AllDefs
                   where d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop && !d.MadeFromStuff
                   select d;
        }
    }
}
