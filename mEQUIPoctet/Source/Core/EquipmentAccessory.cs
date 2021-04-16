using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Core
{
    /// <summary>
    /// Accessory specific stats of the equipment.
    /// </summary>
    public partial class Equipment
    {
        /// <summary>
        /// The flat physical attack added by the accessory.
        /// </summary>
        public int PhysicalAttackFlat { get; set; } = 0;

        /// <summary>
        /// The flat magical attack added by the accessory.
        /// </summary>
        public int MagicalAttackFlat { get; set; } = 0;

        /// <summary>
        /// The physical defense granted by the accessory.
        /// </summary>
        public int AccessoryPhysicalDefense { get; set; } = 0;

        /// <summary>
        /// The evasion granted by the accessory.
        /// </summary>
        public int AccessoryEvasion { get; set; } = 0;

        /// <summary>
        /// The Metal Defense granted by the accessory.
        /// </summary>
        public int AccessoryMetalDefense { get; set; } = 0;

        /// <summary>
        /// The Wood Defense granted by the accessory.
        /// </summary>
        public int AccessoryWoodDefense { get; set; } = 0;

        /// <summary>
        /// The Water Defense granted by the accessory.
        /// </summary>
        public int AccessoryWaterDefense { get; set; } = 0;

        /// <summary>
        /// The Fire Defense granted by the accessory.
        /// </summary>
        public int AccessoryFireDefense { get; set; } = 0;

        /// <summary>
        /// The Earth Defense granted by the accessory.
        /// </summary>
        public int AccessoryEarthDefense { get; set; } = 0;
    }
}
