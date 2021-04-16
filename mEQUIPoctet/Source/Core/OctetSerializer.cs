using mEQUIPoctet.Source.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace mEQUIPoctet.Source.Core
{
    /// <summary>
    /// Serializes a set of skills into octet.
    /// </summary>
    public class OctetSerializer
    {
        /// <summary>
        /// The Equipment to serialize.
        /// </summary>
        private readonly Equipment _equipment;

        /// <summary>
        /// The serialized octet string.
        /// </summary>
        private StringBuilder _octet;

        public OctetSerializer(Equipment equipment)
        {
            _equipment = equipment.Clone();
            _octet = new StringBuilder();
        }

        /// <summary>
        /// Serialize the equipment into an octet string.
        /// </summary>
        /// <returns>The octet string representing the equipment.</returns>
        public string Serialize()
        {
            _octet = new StringBuilder();
            WriteEquipment();
            return _octet.ToString();
        }

        /// <summary>
        /// Writes the equipment to the octet string.
        /// </summary>
        private void WriteEquipment()
        {
            AdjustForAddons();

            WriteShort(_equipment.Level);
            WriteShort(_equipment.Classes);
            WriteShort(_equipment.Strength);
            WriteShort(_equipment.Vitality);
            WriteShort(_equipment.Dexterity);
            WriteShort(_equipment.Magic);
            WriteInt(_equipment.DurabilityCurrentRatio);
            WriteInt(_equipment.DurabilityMaxRatio);

            // Accessory is serialized to the same value as Armor, but internally represented with a different value.
            if (_equipment.Type == EquipmentType.Accessory)
            {
                WriteShort((short)EquipmentType.Armor);
            }
            else
            {
                WriteShort((short)_equipment.Type);
            }            

            WriteSignature();

            if (_equipment.Type == EquipmentType.Weapon)
            {
                WriteWeapon();
            }
            else if (_equipment.Type == EquipmentType.Armor)
            {
                WriteArmor();
            }
            else
            {
                WriteAccessory();
            }

            WriteShort((short)(_equipment.Sockets?.Count ?? 0));
            WriteShort(_equipment.GFX);
            foreach (Socket socket in _equipment.Sockets)
            {
                WriteInt(socket.Soulgem);
            }

            WriteAddons();
        }

        private void WriteSignature()
        {
            byte signatureLength = (byte)(_equipment.Signature?.Length ?? 0);

            // Ink.
            if (_equipment.SignatureType == 5)
            {
                // For some reason the length is overestimated by 1 char in this case.
                signatureLength += 1;
            }

            WriteByte(_equipment.SignatureType);

            // Each char is 2 bytes in UTF-16.
            WriteByte((byte)(signatureLength * 2));

            if (_equipment.SignatureType <= 3)
            {
                return;
            }

            // Ink.
            if (_equipment.SignatureType == 5)
            {
                WriteShort(_equipment.InkColor);
            }

            WriteString16(_equipment.Signature);
        }

        private void WriteWeapon()
        {
            // Use projectile or not, implied by projectile type being non-zero.
            if (_equipment.Projectile == 0)
            {
                WriteInt(0);
            }
            else
            {
                WriteInt(1);
            }

            WriteInt(_equipment.WeaponMajorType);
            WriteInt(_equipment.Grade);
            WriteInt(_equipment.Projectile);
            WriteInt(_equipment.PhysicalAttackLow);
            WriteInt(_equipment.PhysicalAttackHigh);
            WriteInt(_equipment.MagicalAttackLow);
            WriteInt(_equipment.MagicalAttackHigh);
            WriteInt(_equipment.AttackRateRatio);
            WriteFloat(_equipment.Range);
            WriteFloat(_equipment.RangeMin);
        }

        private void WriteArmor()
        {
            WriteInt(_equipment.ArmorPhysicalDefense);
            WriteInt(_equipment.ArmorEvasion);
            WriteInt(_equipment.MP);
            WriteInt(_equipment.HP);
            WriteInt(_equipment.ArmorMetalDefense);
            WriteInt(_equipment.ArmorWoodDefense);
            WriteInt(_equipment.ArmorWaterDefense);
            WriteInt(_equipment.ArmorFireDefense);
            WriteInt(_equipment.ArmorEarthDefense);
        }

        private void WriteAccessory()
        {
            WriteInt(_equipment.PhysicalAttackFlat);
            WriteInt(_equipment.MagicalAttackFlat);
            WriteInt(_equipment.AccessoryPhysicalDefense);
            WriteInt(_equipment.AccessoryEvasion);
            WriteInt(_equipment.AccessoryMetalDefense);
            WriteInt(_equipment.AccessoryWoodDefense);
            WriteInt(_equipment.AccessoryWaterDefense);
            WriteInt(_equipment.AccessoryFireDefense);
            WriteInt(_equipment.AccessoryEarthDefense);
        }

        /// <summary>
        /// Writes all addons including soulgem, refinement, and standard addons to the octet string.
        /// </summary>
        private void WriteAddons()
        {
            int numAddons = _equipment.Addons?.Count ?? 0;

            // Soulgems and refinement are considered addons. So we must increment the addon total first, and then
            // write the addons afterwards.
            foreach (Socket socket in _equipment.Sockets)
            {
                // Socket addons are loaded from presets so we must check that we have the preset.
                if (!socket.HasAddon || !Presets.Soulgem.ContainsKey(socket.Soulgem.ToString()))
                {
                    continue;
                }

                string[] soulgemPreset = Presets.Soulgem[socket.Soulgem.ToString()];
                string addonId = null;

                if (_equipment.Type == EquipmentType.Weapon && soulgemPreset.Length > 1)
                {
                    addonId = soulgemPreset[1];
                }
                else if (_equipment.Type == EquipmentType.Armor && soulgemPreset.Length > 2)
                {
                    addonId = soulgemPreset[2];
                }
                else if (soulgemPreset.Length > 3)
                {
                    addonId = soulgemPreset[3];
                }

                if (string.IsNullOrWhiteSpace(addonId) || !Presets.SoulgemAddon.ContainsKey(addonId))
                {
                    continue;
                }

                numAddons += 1;
            }

            if ((_equipment.Refine?.Id ?? 0) != 0)
            {
                numAddons += 1;
            }

            WriteInt(numAddons);

            foreach (Addon addon in _equipment.Addons)
            {
                WriteAddon(addon);
            }

            foreach (Socket socket in _equipment.Sockets)
            {
                // Socket addons are loaded from presets so we must check that we have the preset.
                if (!socket.HasAddon || !Presets.Soulgem.ContainsKey(socket.Soulgem.ToString()))
                {
                    continue;
                }

                string[] soulgemPreset = Presets.Soulgem[socket.Soulgem.ToString()];
                string addonId = null;

                if (_equipment.Type == EquipmentType.Weapon && soulgemPreset.Length > 1)
                {
                    addonId = soulgemPreset[1];
                }
                else if (_equipment.Type == EquipmentType.Armor && soulgemPreset.Length > 2)
                {
                    addonId = soulgemPreset[2];
                }
                else if (soulgemPreset.Length > 3)
                {
                    addonId = soulgemPreset[3];
                }

                if (string.IsNullOrWhiteSpace(addonId) || !Presets.SoulgemAddon.ContainsKey(addonId))
                {
                    continue;
                }

                string[] addonPreset = Presets.SoulgemAddon[addonId];
                Addon addon = new Addon()
                {
                    Hidden = true,
                    Type = AddonType.Normal
                };
                addon.SetId(addonId);
                        
                if (addonPreset.Length > 1)
                {
                    addon.SetValue(addonPreset[1]);
                }

                if (addonPreset.Length > 2)
                {
                    addon.SetParam2(addonPreset[2]);
                    addon.Type = AddonType.UniqueOffensive;
                }

                if (addonPreset.Length > 3)
                {
                    addon.SetParam3(addonPreset[3]);
                    addon.Type = AddonType.UniqueDefensive;
                }

                WriteAddon(addon);
            }

            if ((_equipment.Refine?.Id ?? 0) != 0)
            {
                WriteAddon(_equipment.Refine);
            }
        }

        /// <summary>
        /// Writes an addon to the octet string.
        /// </summary>
        /// <param name="addon">The addon to serialize</param>
        private void WriteAddon(Addon addon)
        {
            // The addon type 0x2000, 0x4000, or 0x6000 is bitwise OR'd into the addon id before being serialized.
            // It's unclear why addon ids are serialized this way.
            int serializedId = addon.Id | (int)(addon.Type);

            if (addon.Hidden)
            {
                serializedId = serializedId | 0x8000;
            }

            WriteInt(serializedId);

            if (addon.Type == AddonType.Normal || 
                addon.Type == AddonType.UniqueOffensive || 
                addon.Type == AddonType.UniqueDefensive)
            {
                // Certain addons are presented as a float, others are represented as an int.
                if (Presets.RangeAddon.ContainsKey(addon.Id.ToString()))
                {
                    WriteFloat(unchecked((float)addon.Value));
                }
                else
                {
                    WriteInt(unchecked((int)addon.Value));
                }
            }

            if (addon.Type == AddonType.UniqueOffensive || 
                addon.Type == AddonType.UniqueDefensive)
            {
                WriteInt(addon.Param2);
            }

            if (addon.Type == AddonType.UniqueDefensive)
            {
                WriteInt(addon.Param3);
            }
        }

        /// <summary>
        /// Certain addons have effects that are cosmetic only. Adjust the base stats with these addons accounted for.
        /// </summary>
        /// <remarks>
        /// Certain addons such as physical attack, or physical defense addons are cosmetic only. In order to get the
        /// addon's actual effects, the equipment's base stat must be modified. Then, the game client would calculate
        /// the stats to display by subtracting the addon's stat from the base stat, to pretend that the addon actually
        /// has an effect.
        /// </remarks>
        private void AdjustForAddons()
        {
            foreach (Addon addon in _equipment.Addons)
            {
                unchecked
                {
                    string id = addon.Id.ToString();

                    if (Presets.PhysicalAttackAddon.ContainsKey(id))
                    {
                        _equipment.PhysicalAttackLow += (int)addon.Value;
                        _equipment.PhysicalAttackHigh += (int)addon.Value;
                        continue;
                    }

                    if (Presets.PhysicalDefenseAddon.ContainsKey(id))
                    {
                        _equipment.ArmorPhysicalDefense += (int)addon.Value;
                        _equipment.AccessoryPhysicalDefense += (int)addon.Value;
                        continue;
                    }

                    if (Presets.HPAddon.ContainsKey(id))
                    {
                        _equipment.HP += (int)addon.Value;
                        continue;
                    }

                    if (Presets.RangeAddon.ContainsKey(id))
                    {
                        _equipment.Range += (float)addon.Value;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Writes an int to the octet string.
        /// </summary>
        /// <param name="value">The int to serialize.</param>
        private void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            string octet = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            _octet.Append(octet);
        }

        /// <summary>
        /// Writes a short to the octet string.
        /// </summary>
        /// <param name="value">The short to serialize.</param>
        private void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            string octet = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            _octet.Append(octet);
        }

        /// <summary>
        /// Writes a byte to the octet string.
        /// </summary>
        /// <param name="value">The byte to serialize.</param>
        private void WriteByte(byte value)
        {
            byte[] bytes = { value };
            string octet = BitConverter.ToString(bytes).ToLower();
            _octet.Append(octet);
        }

        /// <summary>
        /// Writes a float to the octet string.
        /// </summary>
        /// <param name="value">The float to serialize.</param>
        private void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            string octet = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            _octet.Append(octet);
        }

        /// <summary>
        /// Writes a UTF-16 string to the octet string.
        /// </summary>
        /// <param name="value">The string to serialize.</param>
        private void WriteString16(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            string octet = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
            _octet.Append(octet);
        }
    }
}
