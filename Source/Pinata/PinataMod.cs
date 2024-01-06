using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Pinata
{
    public class PinataMod : Mod
    {
        /// <summary>
        /// A reference to our settings.
        /// </summary>
        PinataSettings settings;

        IEnumerable<ThingDef> allThingDefs = new List<ThingDef>();

        List<string> originalList = new List<string>()
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
                "ComponentSpacer"
        };

        /// <summary>
        /// Used as a static access point for milkable items.
        /// </summary>
        public static List<string> PossibleMilkableItems = new List<string>();

        public static float MinAmount;
        public static float MaxAmount;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public PinataMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<PinataSettings>();
            allThingDefs = PossibleThingDefs();

            PossibleMilkableItems = settings.listOfMilkableItems;
            MinAmount = settings.minAmount;
            MaxAmount = settings.maxAmount;
        }

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Standard
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            var displayMin = (int)settings.minAmount;
            listingStandard.Label("Minimum amount - " + displayMin.ToString());
            settings.minAmount = listingStandard.Slider(settings.minAmount, 1f, settings.maxAmount - 1f);

            var displayMax = (int)settings.maxAmount;
            listingStandard.Label("Maximum amount - " + displayMax.ToString());
            settings.maxAmount = listingStandard.Slider(settings.maxAmount, 1f, 300f);

            listingStandard.Label("NOTE: Only items that stacks is gathered as multiple items. For example a bionic spine would only be gathered as 1x");

            listingStandard.End();

            #region Add item

            Rect addItem = new Rect(0f, 172f, 200f, 32f);
            bool flag = Widgets.ButtonText(addItem, "Add to list", true, true, true);
            if (flag)
            {
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

            Rect removeRect = new Rect(0f, 204f, 200f, 32f);
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

            Rect resetList = new Rect(0f, 236f, 200f, 32f);
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

            Rect emptyList = new Rect(0f, 268f, 200f, 32f);
            bool emptyFlag = Widgets.ButtonText(emptyList, "Empty the list", true, true, true);
            if (emptyFlag)
            {
                settings.listOfMilkableItems.Clear();
            }

            #endregion Empty the list

            #region The list GUI

            var startHeight = 268f;
            Rect position = inRect.TopPart(0.90f);
            GUI.BeginGroup(position);
            Rect outRect = new Rect(0f, 268f, position.width, position.height);
            Rect viewRect = new Rect(0f, 268f, position.width, this.scrollViewHeight);
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

            #endregion The list GUI

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
            return "Bowa_PinataModSettings".Translate();
        }

        /// <summary>
        /// Final method that runs
        /// </summary>
        public override void WriteSettings()
        {
            // Make sure we have atleast one possible item.
            if (settings.listOfMilkableItems.Count <= 0)
            {
                foreach (var item in this.originalList)
                {
                    settings.listOfMilkableItems.Add(item);
                }
            }


            // If minAmount by accident is higher than maxAmount, lower it!

            if (settings.minAmount >= (settings.maxAmount - 1f))
                settings.minAmount = settings.maxAmount - 1f;



            // Set them static values always.
            MinAmount = this.settings.minAmount;
            MaxAmount = this.settings.maxAmount;
            PossibleMilkableItems = this.settings.listOfMilkableItems;

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
