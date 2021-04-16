using mEQUIPoctet.Source.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mEQUIPoctet.Source.Config
{
    public static class Presets
    {
        #region statics
        public static IDictionary<AddonType, string> AddonTypes { get; } = new Dictionary<AddonType, string> {
            { AddonType.Normal, "Normal" },
            { AddonType.UniqueOffensive, "Offensive Unique" },
            { AddonType.UniqueDefensive, "Defensive Unique" },
            { AddonType.Unidentified, "Unidentified" }
        };

        public static IDictionary<byte, string> SignatureTypes { get; } = new Dictionary<byte, string> {
            { 0, "None 0" },
            { 1, "None 1" },
            { 2, "None 2" },
            { 3, "None 3" },
            { 4, "Manufacturer" },
            { 5, "Ink" },
        };
        #endregion statics

        #region config.ini

        /// <summary>
        /// Know classes.
        /// </summary>
        /// <remarks>Classes are represented as a bitfield.</remarks>
        /// <value>
        /// Key: Class bit<br/>
        /// Value: Class name
        /// </value>
        public static IDictionary<string, string> Classes { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Known weapon major types.
        /// </summary>
        /// <value>
        /// Key: the type id<br/>
        /// Value: the type name
        /// </value>
        public static IDictionary<string, string> WeaponMajorType { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Known projectile types.
        /// </summary>
        /// <value>
        /// Key: the type id<br/>
        /// Value: the type name
        /// </value>
        public static IDictionary<string, string> Projectile { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// All equipment addons (excludes soulgem addons).
        /// </summary>
        /// <value>
        /// Key: the addon id<br/>
        /// Value: an array containing the addon's effect string, and the default Value, Param2, and Param3
        /// </value>
        public static IDictionary<string, string[]> Addon { get; private set; } = new Dictionary<string, string[]>();

        public static IDictionary<string, string[]> PhysicalAttackAddon { get; private set; } = new Dictionary<string, string[]>();
        public static IDictionary<string, string[]> PhysicalDefenseAddon { get; private set; } = new Dictionary<string, string[]>();
        public static IDictionary<string, string[]> HPAddon { get; private set; } = new Dictionary<string, string[]>();
        public static IDictionary<string, string[]> RangeAddon { get; private set; } = new Dictionary<string, string[]>();



        /// <summary>
        /// All equipment addons (excludes soulgem addons), but addons that give the same effects are included only
        /// once.
        /// </summary>
        /// <value>
        /// Key: the addon id<br/>
        /// Value: an array containing the addon's effect string, and the default Value, Param2, and Param3
        /// </value>
        public static IDictionary<string, string[]> AddonSource { get; private set; } = new Dictionary<string, string[]>();

        /// <summary>
        /// Addons used in soulgems.
        /// </summary>
        /// <value>
        /// Key: the addon id<br/>
        /// Value: an array containing the addon's effect string, and the default Value, Param2, and Param3
        /// </value>
        public static IDictionary<string, string[]> SoulgemAddon { get; private set; } = new Dictionary<string, string[]>();

        /// <summary>
        /// Known soulgems.
        /// </summary>
        /// <value>
        /// Key: the soulgem id<br/>
        /// Value: an array containing the soulgem gem name, and the soulgem addon id for weapon, armor, accessory.
        /// </value>
        public static IDictionary<string, string[]> Soulgem { get; private set; } = new Dictionary<string, string[]>();

        /// <summary>
        /// Known refines.
        /// </summary>
        /// <value>
        /// Key: the refine addon id<br/>
        /// Value: the refine effect string
        /// </value>
        public static IDictionary<string, string> Refine { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Known soulgem graphics effects for Weapon.
        /// </summary>
        /// <value>Key is the GFX id, Value is the GFX name.</value>
        public static IDictionary<string, string> GFX { get; private set; } = new Dictionary<string, string>();
        #endregion config.ini

        /// <summary>
        /// Load the presets from a given file path.
        /// </summary>
        /// <param name="path">The path of the file</param>
        public static void Load(string path)
        {
            IniParser parser = new IniParser(path);
            IDictionary<string, IDictionary<string, string>> ini = parser.Parse();

            foreach (KeyValuePair<string, IDictionary<string, string>> section in ini)
            {
                string sectionName = section.Key;
                IDictionary<string, string> sectionEntries = section.Value;
                //var sectionEntries = new SortedDictionary<string, string>(section.Value, new IntegerStringComparer());

                switch (sectionName)
                {
                    case @"WeaponMajorType":
                        WeaponMajorType = new SortedDictionary<string, string>(sectionEntries, new IntegerStringComparer());
                        break;
                    case @"Projectile":
                        Projectile = new SortedDictionary<string, string>(sectionEntries, new IntegerStringComparer());
                        break;
                    case @"Classes":
                        Classes = new SortedDictionary<string, string>(sectionEntries, new IntegerStringComparer());
                        break;
                    case @"Addon":
                        Addon = SplitValues(sectionEntries);
                        Addon = new SortedDictionary<string, string[]>(Addon, new IntegerStringComparer());
                        break;
                    case @"PhysicalAttackAddon":
                        PhysicalAttackAddon = SplitValues(sectionEntries);
                        break;
                    case @"PhysicalDefenseAddon":
                        PhysicalDefenseAddon = SplitValues(sectionEntries);
                        break;
                    case @"HPAddon":
                        HPAddon = SplitValues(sectionEntries);
                        break;
                    case @"RangeAddon":
                        RangeAddon = SplitValues(sectionEntries);
                        break;
                    case @"SoulgemAddon":
                        SoulgemAddon = SplitValues(sectionEntries);
                        break;
                    case @"Soulgem":
                        Soulgem = SplitValues(sectionEntries);
                        Soulgem.Add("0", new string[] { "Empty" });
                        Soulgem = new SortedDictionary<string, string[]>(Soulgem, new IntegerStringComparer());
                        break;
                    case @"Refine":
                        Refine = new SortedDictionary<string, string>(sectionEntries, new IntegerStringComparer());
                        break;
                    case @"GFX":
                        GFX = new SortedDictionary<string, string>(sectionEntries, new IntegerStringComparer());
                        break;
                }
            }

            MergeDictionaries(Addon, PhysicalAttackAddon);
            MergeDictionaries(Addon, PhysicalDefenseAddon);
            MergeDictionaries(Addon, HPAddon);
            MergeDictionaries(Addon, RangeAddon);

            AddonSource = UniqueValue(Addon);
        }

        /// <summary>
        /// Split the value of each kvp in a dictionary into an array of strings.
        /// </summary>
        /// <param name="dictionary">Dictionary to split.</param>
        /// <returns>New dictionary with value of each kvp split into an array.</returns>
        private static IDictionary<string, string[]> SplitValues(IDictionary<string, string> dictionary)
        {
            IDictionary<string, string[]> newDictionary = new Dictionary<string, string[]>();

            foreach (KeyValuePair<string, string> item in dictionary)
            {
                string[] newValue = item.Value.Split(',');
                newDictionary.Add(item.Key, newValue);
            }

            return newDictionary;
        }

        /// <summary>
        /// Merge items in-place from the source dictionary into the destionary dictionary.
        /// </summary>
        /// <param name="dst">The destionary dictionary to merge into.</param>
        /// <param name="src">The source dictionary to merge from.</param>
        private static void MergeDictionaries(IDictionary<string, string[]> dst, IDictionary<string, string[]> src)
        {
            foreach (KeyValuePair<string, string[]> item in src)
            {
                if (!dst.ContainsKey(item.Key))
                {
                    dst.Add(item);
                }
            }
        }

        /// <summary>
        /// Put only unique addons of the dictionary into a new dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to </param>
        /// <returns></returns>
        private static IDictionary<string, string[]> UniqueValue(IDictionary<string, string[]> dictionary)
        {
            IDictionary<string, string[]> newDictionary = new SortedDictionary<string, string[]>(new IntegerStringComparer());

            ISet<string[]> values = new HashSet<string[]>(new ArrayStringEqualityComparer());
            
            foreach (KeyValuePair<string, string[]> item in dictionary)
            {
                string[] value = item.Value;
                if (value.Length > 0 && !values.Contains(value))
                {
                    values.Add(value);
                    newDictionary.Add(item);
                }
            }

            return newDictionary;
        }
    }
}
