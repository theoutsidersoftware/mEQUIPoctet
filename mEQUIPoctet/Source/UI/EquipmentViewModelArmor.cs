using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.UI
{
    /// <summary>
    /// The armor specific portion of EquipmentViewModel.
    /// </summary>
    public partial class EquipmentViewModel
    {
        public string ArmorPhysicalDefense
        {
            get
            {
                return _equipment.ArmorPhysicalDefense.ToString();
            }

            set
            {
                _equipment.ArmorPhysicalDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorEvasion
        {
            get
            {
                return _equipment.ArmorEvasion.ToString();
            }

            set
            {
                _equipment.ArmorEvasion = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string MP
        {
            get
            {
                return _equipment.MP.ToString();
            }

            set
            {
                _equipment.MP = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string HP
        {
            get
            {
                return _equipment.HP.ToString();
            }

            set
            {
                _equipment.HP = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorMetalDefense
        {
            get
            {
                return _equipment.ArmorMetalDefense.ToString();
            }

            set
            {
                _equipment.ArmorMetalDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorWoodDefense
        {
            get
            {
                return _equipment.ArmorWoodDefense.ToString();
            }

            set
            {
                _equipment.ArmorWoodDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorWaterDefense
        {
            get
            {
                return _equipment.ArmorWaterDefense.ToString();
            }

            set
            {
                _equipment.ArmorWaterDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorFireDefense
        {
            get
            {
                return _equipment.ArmorFireDefense.ToString();
            }

            set
            {
                _equipment.ArmorFireDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string ArmorEarthDefense
        {
            get
            {
                return _equipment.ArmorEarthDefense.ToString();
            }

            set
            {
                _equipment.ArmorEarthDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }
    }
}
