using mEQUIPoctet.Source.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace mEQUIPoctet.Source.UI
{
    public partial class EquipmentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Equipment _equipment = new Equipment();

        public string Level
        {
            get
            {
                return _equipment.Level.ToString();
            }

            set
            {
                _equipment.Level = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string Classes
        {
            get
            {
                return _equipment.Classes.ToString();
            }

            set
            {
                _equipment.Classes = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string Strength
        {
            get
            {
                return _equipment.Strength.ToString();
            }

            set
            {
                _equipment.Strength = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string Vitality
        {
            get
            {
                return _equipment.Vitality.ToString();
            }

            set
            {
                _equipment.Vitality = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string Dexterity
        {
            get
            {
                return _equipment.Dexterity.ToString();
            }

            set
            {
                _equipment.Dexterity = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string Magic
        {
            get
            {
                return _equipment.Magic.ToString();
            }

            set
            {
                _equipment.Magic = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public string DurabilityCurrent
        {
            get
            {
                return _equipment.DurabilityCurrent.ToString();
            }

            set
            {
                _equipment.DurabilityCurrent = ParseFloat(value);
                NotifyPropertyChanged();
            }
        }

        public string DurabilityMax
        {
            get
            {
                return _equipment.DurabilityMax.ToString();
            }

            set
            {
                _equipment.DurabilityMax = ParseFloat(value);
                NotifyPropertyChanged();
            }
        }

        public EquipmentType Type
        {
            get
            {
                return _equipment.Type;
            }

            set
            {
                _equipment.Type = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The equipment's signature type.
        /// </summary>
        /// <value>
        /// None <= 3<br/>
        /// Manufacturer = 4<br/>
        /// Ink = 5
        /// </value>
        public byte SignatureType
        {
            get
            {
                return _equipment.SignatureType;
            }

            set
            {
                _equipment.SignatureType = value;             
                NotifyPropertyChanged();
            }
        }

        public short InkColor
        {
            get
            {
                return _equipment.InkColor;
            }

            set
            {
                _equipment.InkColor = value;
                NotifyPropertyChanged();
            }
        }

        public string Signature
        {
            get
            {
                return _equipment.Signature;
            }

            set
            {
                // Max length of signature is 126 characters, because length of signature is represented as a byte.
                // Each character is 2 bytes, and Ink signature type adds +2 bytes.
                _equipment.Signature = value.Length <= 126 ? value : value.Substring(0, 126);
                NotifyPropertyChanged();
            }
        }

        public string GFX
        {
            get
            {
                return _equipment.GFX.ToString();
            }

            set
            {
                _equipment.GFX = ParseShort(value);
                NotifyPropertyChanged();
            }
        }

        public IList<Socket> Sockets
        {
            get
            {
                return _equipment.Sockets;
            }

            set
            {
                _equipment.Sockets = value;
                NotifyPropertyChanged();
            }
        }

        public IList<Addon> Addons
        {
            get
            {
                return _equipment.Addons;
            }

            set
            {
                _equipment.Addons = value;
                NotifyPropertyChanged();
            }
        }

        public string RefineId
        {
            get
            {
                return _equipment.Refine.Id.ToString();
            }

            set
            {
                _equipment.Refine.Id = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string RefineValue
        {
            get
            {
                return _equipment.Refine.Value.ToString();
            }

            set
            {
                _equipment.Refine.Value = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string RefineParam2
        {
            get
            {
                return _equipment.Refine.Param2.ToString();
            }

            set
            {
                _equipment.Refine.Param2 = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Import the values of an Equipment into this ViewModel.
        /// </summary>
        /// <param name="equipment">The equipment to import.</param>
        public void Import(Equipment equipment)
        {
            Level = equipment.Level.ToString();
            Classes = equipment.Classes.ToString();
            Strength = equipment.Strength.ToString();
            Vitality = equipment.Vitality.ToString();
            Dexterity = equipment.Dexterity.ToString();
            Magic = equipment.Magic.ToString();
            DurabilityCurrent = equipment.DurabilityCurrent.ToString();
            DurabilityMax = equipment.DurabilityMax.ToString();
            Type = equipment.Type;
            SignatureType = equipment.SignatureType;
            InkColor = equipment.InkColor;
            Signature = equipment.Signature;

            #region Weapon only
            Grade = equipment.Grade.ToString();
            WeaponMajorType = equipment.WeaponMajorType.ToString();
            Projectile = equipment.Projectile.ToString();
            PhysicalAttackLow = equipment.PhysicalAttackLow.ToString();
            PhysicalAttackHigh = equipment.PhysicalAttackHigh.ToString();
            MagicalAttackLow = equipment.MagicalAttackLow.ToString();
            MagicalAttackHigh = equipment.MagicalAttackHigh.ToString();
            AttackRate = equipment.AttackRate.ToString();
            Range = equipment.Range.ToString();
            RangeMin = equipment.RangeMin.ToString();
            #endregion Weapon only

            #region Armor only
            ArmorPhysicalDefense = equipment.ArmorPhysicalDefense.ToString();
            ArmorEvasion = equipment.ArmorEvasion.ToString();
            MP = equipment.MP.ToString();
            HP = equipment.HP.ToString();
            ArmorMetalDefense = equipment.ArmorMetalDefense.ToString();
            ArmorWoodDefense = equipment.ArmorWoodDefense.ToString();
            ArmorWaterDefense = equipment.ArmorWaterDefense.ToString();
            ArmorFireDefense = equipment.ArmorFireDefense.ToString();
            ArmorEarthDefense = equipment.ArmorEarthDefense.ToString();
            #endregion Armor only

            #region Accessory only
            PhysicalAttackFlat = equipment.PhysicalAttackFlat.ToString();
            MagicalAttackFlat = equipment.MagicalAttackFlat.ToString();
            AccessoryPhysicalDefense = equipment.AccessoryPhysicalDefense.ToString();
            AccessoryEvasion = equipment.AccessoryEvasion.ToString();
            AccessoryMetalDefense = equipment.AccessoryMetalDefense.ToString();
            AccessoryWoodDefense = equipment.AccessoryWoodDefense.ToString();
            AccessoryWaterDefense = equipment.AccessoryWaterDefense.ToString();
            AccessoryFireDefense = equipment.AccessoryFireDefense.ToString();
            AccessoryEarthDefense = equipment.AccessoryEarthDefense.ToString();
            #endregion Accessory only

            GFX = equipment.GFX.ToString();
            Sockets = equipment.Sockets;
            Addons = equipment.Addons;
            RefineId = equipment.Refine.Id.ToString();
            RefineValue = equipment.Refine.Value.ToString();
            RefineParam2 = equipment.Refine.Param2.ToString();
        }

        /// <summary>
        /// Export the underlying Equipment managed by this ViewModel.
        /// </summary>
        /// <returns>The underlying Equipment.</returns>
        public Equipment Export()
        {
            return _equipment;
        }

        private static short ParseShort(string value)
        {
            try
            {
                return short.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands);
            }
            catch (OverflowException)
            {
                if (value.Trim().StartsWith("-"))
                {
                    return short.MinValue;
                }

                return short.MaxValue;
            }
            catch
            {
                return 0;
            }
        }

        private static int ParseInt(string value)
        {
            try
            {
                return int.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands);
            }
            catch (OverflowException)
            {
                if (value.Trim().StartsWith("-"))
                {
                    return int.MinValue;
                }

                return int.MaxValue;
            }
            catch
            {
                return 0;
            }
        }

        private static float ParseFloat(string value)
        {
            try
            {
                return float.Parse(value, NumberStyles.Float | NumberStyles.AllowThousands);
            }
            catch (OverflowException)
            {
                if (value.Trim().StartsWith("-"))
                {
                    return float.MinValue;
                }

                return float.MaxValue;
            }
            catch
            {
                return 0;
            }
        }
    }
}
