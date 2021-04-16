using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Core
{
    public enum EquipmentType : short
    {
        Weapon = 44,
        Armor = 36,
        // Accessory is serialized to the same value as Armor, but internally we represent it with a different value.
        Accessory
    }

    /// <summary>
    /// Represents a equipment.
    /// </summary>
    public partial class Equipment
    {
        /// <summary>
        /// The level required to use the equipment.
        /// </summary>
        public short Level { get; set; } = 0;

        /// <summary>
        /// The classes that can use the equipment, represented as a bitfield.
        /// </summary>
        /// <value>
        /// Blademaster=1<br/>
        /// Wizard=2<br/>
        /// Psychic=4<br/>
        /// Venomancer=8<br/>
        /// Barbarian=16<br/>
        /// Assassin=32<br/>
        /// Archer=64<br/>
        /// Cleric=128<br/>
        /// Seeker=256<br/>
        /// Mystic=512<br/>
        /// Duskblade=1024<br/>
        /// Stormbringer=2048
        /// </value>
        public short Classes { get; set; } = 4095;

        /// <summary>
        /// The strength required to use the equipment.
        /// </summary>
        public short Strength { get; set; } = 0;

        /// <summary>
        /// The vitality required to use the equipment.
        /// </summary>
        public short Vitality { get; set; } = 0;

        /// <summary>
        /// The dexterity required to use the equipment.
        /// </summary>
        public short Dexterity { get; set; } = 0;

        /// <summary>
        /// The magic required to use the equipment.
        /// </summary>
        public short Magic { get; set; } = 0;


        /// <summary>
        /// The true value of the current durability of the equipment.
        /// </summary>
        public int DurabilityCurrentRatio { get; set; } = 100;

        /// <summary>
        /// The humnan readable value of the current durability of the equipment.
        /// </summary>
        public float DurabilityCurrent 
        {
            get
            {
                return (float)Math.Round(DurabilityCurrentRatio / 100.0f, 2);
            }

            set
            {
                DurabilityCurrentRatio = (int)Math.Round(value * 100);
            }
        }

        /// <summary>
        /// The true value of the max durability of the equipment.
        /// </summary>
        public int DurabilityMaxRatio { get; set; } = 100;

        /// <summary>
        /// The human readable value of the max durability of the equipment.
        /// </summary>
        public float DurabilityMax 
        {
            get
            {
                return (float)Math.Round(DurabilityMaxRatio / 100.0f);
            }

            set
            {
                DurabilityMaxRatio = (int)Math.Round(value * 100);
            }
        }

        /// <summary>
        /// Whether the equipment is a weapon or armor/accessory.
        /// </summary>
        public EquipmentType Type { get; set; } = EquipmentType.Weapon;

        /// <summary>
        /// The equipment's signature type.
        /// </summary>
        /// <value>
        /// None <= 3<br/>
        /// Manufacturer = 4<br/>
        /// Ink = 5
        /// </value>
        public byte SignatureType { get; set; } = 0;

        /// <summary>
        /// 16-bit Ink Color. No effect if Signature Type is not Ink.
        /// </summary>
        public short InkColor { get; set; } = 0;

        /// <summary>
        /// The equipment's signature in UTF-16.
        /// Max length is ubyte.MaxValue.
        /// </summary>
        public string Signature { get; set; } = "";

        /// <summary>
        /// The equipment's soulgem graphics effect.
        /// </summary>
        public short GFX { get; set; } = 0;

        /// <summary>
        /// Sockets on the equipment.
        /// </summary>
        public IList<Socket> Sockets { get; set; } = new List<Socket>(0);

        /// <summary>
        /// All of the equipment's addons excluding identified soulgem addons and refinements.
        /// </summary>
        public IList<Addon> Addons { get; set; } = new List<Addon>(0);

        /// <summary>
        /// The equipment's refinement.
        /// </summary>
        /// <remarks>
        /// Refinement is represented as a special addon where the Id should correspond to List 3 Field 29 of the
        /// equipment in elements.data, the Value represents the additional stats, and Param2 represents the refinement
        /// level.
        /// </remarks>
        public Addon Refine { get; set; } = new Addon() { Type = AddonType.UniqueOffensive };

        /// <summary>
        /// Creates a shallow clone of this Equipment.
        /// </summary>
        /// <returns>The cloned equipment.</returns>
        public Equipment Clone()
        {
            Equipment equipment = new Equipment();

            equipment.Level = Level;
            equipment.Classes = Classes;
            equipment.Strength = Strength;
            equipment.Vitality = Vitality;
            equipment.Dexterity = Dexterity;
            equipment.Magic = Magic;
            equipment.DurabilityCurrent = DurabilityCurrent;
            equipment.DurabilityMax = DurabilityMax;
            equipment.Type = Type;
            equipment.SignatureType = SignatureType;
            equipment.InkColor = InkColor;
            equipment.Signature = Signature;

            #region Weapon only
            equipment.Grade = Grade;
            equipment.WeaponMajorType = WeaponMajorType;
            equipment.Projectile = Projectile;
            equipment.PhysicalAttackLow = PhysicalAttackLow;
            equipment.PhysicalAttackHigh = PhysicalAttackHigh;
            equipment.MagicalAttackLow = MagicalAttackLow;
            equipment.MagicalAttackHigh = MagicalAttackHigh;
            equipment.AttackRate = AttackRate;
            equipment.Range = Range;
            equipment.RangeMin = RangeMin;
            #endregion Weapon only

            #region Armor only
            equipment.ArmorPhysicalDefense = ArmorPhysicalDefense;
            equipment.ArmorEvasion = ArmorEvasion;
            equipment.MP = MP;
            equipment.HP = HP;
            equipment.ArmorMetalDefense = ArmorMetalDefense;
            equipment.ArmorWoodDefense = ArmorWoodDefense;
            equipment.ArmorWaterDefense = ArmorWaterDefense;
            equipment.ArmorFireDefense = ArmorFireDefense;
            equipment.ArmorEarthDefense = ArmorEarthDefense;
            #endregion Armor only

            #region Accessory only
            equipment.PhysicalAttackFlat = PhysicalAttackFlat;
            equipment.MagicalAttackFlat = MagicalAttackFlat;
            equipment.AccessoryPhysicalDefense = AccessoryPhysicalDefense;
            equipment.AccessoryEvasion = AccessoryEvasion;
            equipment.AccessoryMetalDefense = AccessoryMetalDefense;
            equipment.AccessoryWoodDefense = AccessoryWoodDefense;
            equipment.AccessoryWaterDefense = AccessoryWaterDefense;
            equipment.AccessoryFireDefense = AccessoryFireDefense;
            equipment.AccessoryEarthDefense = AccessoryEarthDefense;
            #endregion Accessory only

            equipment.GFX = GFX;
            equipment.Sockets = Sockets;
            equipment.Addons = Addons;
            equipment.Refine = Refine;

            return equipment;
        }
    }
}
