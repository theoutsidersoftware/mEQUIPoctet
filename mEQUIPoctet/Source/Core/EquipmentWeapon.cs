using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Core
{
    /// <summary>
    /// Weapon specifc stats of the equipment.
    /// </summary>
    public partial class Equipment
    {
        /// <summary>
        /// The grade of the weapon.
        /// </summary>
        public int Grade { get; set; } = 0;

        /// <summary>
        /// The weapon's major type.
        /// </summary>
        /// <value>
        /// Sword=1<br/>
        /// Polearm=5<br/>
        /// A/H=9<br/>
        /// Ranged=13<br/>
        /// Fists=182<br/>
        /// Magic=292<br/>
        /// Daggers=23749<br/>
        /// Spheres=25333<br/>
        /// Saber=44878<br/>
        /// Scythe=44879
        /// </value>
        public int WeaponMajorType { get; set; } = 1;

        /// <summary>
        /// The weapon's projectile type. Corresponds to id of Projectile Types in elements.data.
        /// </summary>
        /// <value>
        /// None=0<br/>
        /// Bow=8546<br/>
        /// Crossbow=8547<br/>
        /// Slingshot=8548
        /// </value>
        public int Projectile { get; set; } = 0;

        /// <summary>
        /// The low end of the weapon's physical attack.
        /// </summary>
        public int PhysicalAttackLow { get; set; } = 1;

        /// <summary>
        /// The high end of the weapon's physical attack.
        /// </summary>
        public int PhysicalAttackHigh { get; set; } = 1;

        /// <summary>
        /// The low end of the weapon's magical attack.
        /// </summary>
        public int MagicalAttackLow { get; set; } = 0;

        /// <summary>
        /// The high end of the weapon's magical attack.
        /// </summary>
        public int MagicalAttackHigh { get; set; } = 0;

        /// <summary>
        /// The APS of the weapon.
        /// </summary>
        public float AttackRate
        {
            get
            {
                return 20.0f / AttackRateRatio;
            }

            set
            {
                AttackRateRatio = (int)(20.0f / value);
            }
        }

        /// <summary>
        /// The attack rate ratio of the weapon, used to derive the APS.
        /// </summary>
        public int AttackRateRatio { get; set; } = 16;

        /// <summary>
        /// The attack range of the weapon.
        /// </summary>
        public float Range { get; set; } = 3.0f;

        /// <summary>
        /// The minimum effective range, below which the weapon does half damage.
        /// </summary>
        public float RangeMin { get; set; } = 0.0f;
    }
}
