using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.UI
{
    /// <summary>
    /// The weapon specific portion of EquipmentViewModel.
    /// </summary>
    public partial class EquipmentViewModel
    {
        public string Grade
        {
            get
            {
                return _equipment.Grade.ToString();
            }

            set
            {
                _equipment.Grade = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string WeaponMajorType
        {
            get
            {
                return _equipment.WeaponMajorType.ToString();
            }

            set
            {
                _equipment.WeaponMajorType = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string Projectile
        {
            get
            {
                return _equipment.Projectile.ToString();
            }

            set
            {
                _equipment.Projectile = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string PhysicalAttackLow
        {
            get
            {
                return _equipment.PhysicalAttackLow.ToString();
            }

            set
            {
                _equipment.PhysicalAttackLow = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string PhysicalAttackHigh
        {
            get
            {
                return _equipment.PhysicalAttackHigh.ToString();
            }

            set
            {
                _equipment.PhysicalAttackHigh = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string MagicalAttackLow
        {
            get
            {
                return _equipment.MagicalAttackLow.ToString();
            }

            set
            {
                _equipment.MagicalAttackLow = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string MagicalAttackHigh
        {
            get
            {
                return _equipment.MagicalAttackHigh.ToString();
            }

            set
            {
                _equipment.MagicalAttackHigh = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AttackRate
        {
            get
            {
                return _equipment.AttackRate.ToString();
            }

            set
            {
                _equipment.AttackRate = ParseFloat(value);
                NotifyPropertyChanged();
            }
        }

        public string Range
        {
            get
            {
                return _equipment.Range.ToString();
            }

            set
            {
                _equipment.Range = ParseFloat(value);
                NotifyPropertyChanged();
            }
        }

        public string RangeMin
        {
            get
            {
                return _equipment.RangeMin.ToString();
            }

            set
            {
                _equipment.RangeMin = ParseFloat(value);
                NotifyPropertyChanged();
            }
        }
    }
}
