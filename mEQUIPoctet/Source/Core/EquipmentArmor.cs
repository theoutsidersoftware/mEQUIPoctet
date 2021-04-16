using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Core
{
    /// <summary>
    /// Armor specific stats of the equipment.
    /// </summary>
    public partial class Equipment
    {
        /// <summary>
        /// The physical defense granted by the armor.
        /// </summary>
        public int ArmorPhysicalDefense { get; set; } = 0;

        /// <summary>
        /// The evasion granted by the armor.
        /// </summary>
        public int ArmorEvasion { get; set; } = 0;

        /// <summary>
        /// The MP granted by the armor.
        /// </summary>
        public int MP { get; set; } = 0;

        /// <summary>
        /// The HP granted by the armor.
        /// </summary>
        public int HP { get; set; } = 0;

        /// <summary>
        /// The Metal Defense granted by the armor.
        /// </summary>
        public int ArmorMetalDefense { get; set; } = 0;

        /// <summary>
        /// The Wood Defense granted by the armor.
        /// </summary>
        public int ArmorWoodDefense { get; set; } = 0;

        /// <summary>
        /// The Water Defense granted by the armor.
        /// </summary>
        public int ArmorWaterDefense { get; set; } = 0;

        /// <summary>
        /// The Fire Defense granted by the armor.
        /// </summary>
        public int ArmorFireDefense { get; set; } = 0;

        /// <summary>
        /// The Earth Defense granted by the armor.
        /// </summary>
        public int ArmorEarthDefense { get; set; } = 0;
    }
}
