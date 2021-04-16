using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.Core
{
    public enum AddonType : int
    {
        Unidentified = 0x0000,
        Normal = 0x2000,
        UniqueOffensive = 0x4000,
        UniqueDefensive = 0x6000,
    }

    public class Addon
    {
        /// <summary>
        /// The id of the Addon.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// The value that the Addon gives.
        /// </summary>
        /// <remarks>
        /// Some addons are represented as an int, some are represented as a float. So store as decimal, then cast to
        /// the correct type when serializing.
        /// </remarks>
        public decimal Value { get; set; } = 0;

        /// <summary>
        /// The type of addon.
        /// </summary>
        public AddonType Type { get; set; } = AddonType.Normal;

        /// <summary>
        /// Parameter used by unique offensive and unique defensive addons.
        /// </summary>
        public int Param2 { get; set; } = 0;

        /// <summary>
        /// Parameter used only by unique defensive addons.
        /// </summary>
        /// <remarks>No effect if addon is Hidden.</remarks>
        public int Param3 { get; set; } = 0;

        /// <summary>
        /// Whether the addon is hidden. This should be set if the addon belongs to a soulgem.
        /// </summary>
        /// <remarks>The bit 0x8000 determines if the addon is hidden.</remarks>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// Sets the id of the addon, and return whether it was successful.
        /// </summary>
        /// <param name="id">The string representation of the id.</param>
        /// <returns>Whether the id was set successfully.</returns>
        public bool SetId(string id)
        {
            int parsedId;
            if (int.TryParse(id, out parsedId))
            {
                Id = parsedId;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value of the addon, and return whether it was successful.
        /// </summary>
        /// <param name="value">The string representation of the value.</param>
        /// <returns>Whether the value was set successfully.</returns>
        public bool SetValue(string value)
        {
            int parsedValue;
            if (int.TryParse(value, out parsedValue))
            {
                Value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the param2 of the addon, and return whether it was successful.
        /// </summary>
        /// <param name="param2">The string representation of the param2.</param>
        /// <returns>Whether the param2 was set successfully.</returns>
        public bool SetParam2(string param2)
        {
            int parsedParam2;
            if (int.TryParse(param2, out parsedParam2))
            {
                Param2 = parsedParam2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the param2 of the addon, and return whether it was successful.
        /// </summary>
        /// <param name="param3">The string representation of the param2.</param>
        /// <returns>Whether the param2 was set successfully.</returns>
        public bool SetParam3(string param3)
        {
            int parsedParam3;
            if (int.TryParse(param3, out parsedParam3))
            {
                Param3 = parsedParam3;
                return true;
            }

            return false;
        }
    }
}
