using mEQUIPoctet.Source.Config;
using System;
using System.Collections.Generic;

namespace mEQUIPoctet.Source.Core
{
    /// <summary>
    /// Parser for a skills octet.
    /// </summary>
    public class OctetParser
    {
        /// <summary>
        /// The octet to parse.
        /// </summary>
        private readonly string _octet;

        /// <summary>
        /// The current position the parser is at.
        /// </summary>
        private int _position;

        /// <summary>
        /// The parsed equipment.
        /// </summary>
        private Equipment _equipment;

        public OctetParser(string octet)
        {
            _octet = octet;
            Reset();
        }

        /// <summary>
        /// Resets the parser's position in the octet string.
        /// </summary>
        private void Reset()
        {
            _position = 0;
            _equipment = null;
        }

        public Equipment Parse()
        {
            Reset();
            ParseEquipment();
            return _equipment;
        }

        /// <summary>
        /// Parses the octet string into an equipment objects.
        /// </summary>
        /// <returns>An equipment object represented by the octet.</returns>
        private void ParseEquipment()
        {
            _equipment = new Equipment();

            _equipment.Level = ReadShort();
            _equipment.Classes = ReadShort();
            _equipment.Strength = ReadShort();
            _equipment.Vitality = ReadShort();
            _equipment.Dexterity = ReadShort();
            _equipment.Magic = ReadShort();
            _equipment.DurabilityCurrentRatio = ReadInt();
            _equipment.DurabilityMaxRatio = ReadInt();

            _equipment.Type = (EquipmentType)ReadShort();

            ParseSignature();

            if (_equipment.Type == EquipmentType.Weapon)
            {
                ParseWeapon();
            }
            else
            {
                ParseArmorOrAccessory();
            }

            int numSockets = ReadShort();
            _equipment.Sockets = new List<Socket>(numSockets);
            _equipment.GFX = ReadShort();
            for (int i = 0; i < numSockets; i++)
            {
                // Set HasAddon to false here and identify the addon later, then set to true if identified.
                _equipment.Sockets.Add(new Socket { Soulgem = ReadInt(), HasAddon = false });
            }

            int numAddons = ReadInt();
            ParseAddons(numAddons);

            AdjustForAddons();
        }

        /// <summary>
        /// Parses the next bytes as a signature.
        /// </summary>
        private void ParseSignature()
        {
            _equipment.SignatureType = ReadByte();
            byte signatureLength = ReadByte();

            // None.
            if (_equipment.SignatureType  <= 3)
            {
                return;
            }

            // Ink.
            if (_equipment.SignatureType == 5)
            {
                // For some reason the length is overestimated by 1 char in this case.
                signatureLength -= 2;
                _equipment.InkColor = ReadShort();
            }

            _equipment.Signature = ReadString16(signatureLength);
        }

        /// <summary>
        /// Parses the next bytes as a weapon.
        /// </summary>
        private void ParseWeapon()
        {
            ReadInt(); // use projectile or not, implied by projectile type.
            _equipment.WeaponMajorType = ReadInt();
            _equipment.Grade = ReadInt();
            _equipment.Projectile = ReadInt();
            _equipment.PhysicalAttackLow = ReadInt();
            _equipment.PhysicalAttackHigh = ReadInt();
            _equipment.MagicalAttackLow = ReadInt();
            _equipment.MagicalAttackHigh = ReadInt();
            _equipment.AttackRateRatio = ReadInt();
            _equipment.Range = ReadFloat();
            _equipment.RangeMin = ReadFloat();
        }

        /// <summary>
        /// Parses the next bytes as an armor or accessory.
        /// </summary>
        /// <remarks>
        /// Armor and accessories are indistinguishable from each other by looking at octet alone. Instead we must use
        /// a heuristic to guess whether it's more likely the octet represents an armor or accessory.
        /// </remarks>
        private void ParseArmorOrAccessory()
        {
            int physdefOrAtk = ReadInt();
            int evasionOrMagAtk = ReadInt();
            int mpOrPhysDef = ReadInt();
            int hpOrEvasion = ReadInt();
            int metalDef = ReadInt();
            int woodDef = ReadInt();
            int waterDef = ReadInt();
            int fireDef = ReadInt();
            int earthDef = ReadInt();

            int magicDef = metalDef + woodDef + waterDef + fireDef + earthDef;
            _equipment.Type = GuessArmorOrAccessory(physdefOrAtk, evasionOrMagAtk, mpOrPhysDef, hpOrEvasion, magicDef);

            // Set the stats for both armor and accessory in case the guess is wrong.

            // Armor.
            _equipment.ArmorPhysicalDefense = physdefOrAtk;
            _equipment.ArmorEvasion = evasionOrMagAtk;
            _equipment.MP = mpOrPhysDef;
            _equipment.HP = hpOrEvasion;
            _equipment.ArmorMetalDefense = metalDef;
            _equipment.ArmorWoodDefense = woodDef;
            _equipment.ArmorWaterDefense = waterDef;
            _equipment.ArmorFireDefense = fireDef;
            _equipment.ArmorEarthDefense = earthDef;

            // Accessory.
            _equipment.PhysicalAttackFlat = physdefOrAtk;
            _equipment.MagicalAttackFlat = evasionOrMagAtk;
            _equipment.AccessoryPhysicalDefense = mpOrPhysDef;
            _equipment.AccessoryEvasion = hpOrEvasion;
            _equipment.AccessoryMetalDefense = metalDef;
            _equipment.AccessoryWoodDefense = woodDef;
            _equipment.AccessoryWaterDefense = waterDef;
            _equipment.AccessoryFireDefense = fireDef;
            _equipment.AccessoryEarthDefense = earthDef;
        }

        /// <summary>
        /// Guess if the equipment is an armor or accessory.
        /// </summary>
        /// <param name="physdefOrAtk">The value of physical defense or physical attack.</param>
        /// <param name="evasionOrMagAtk">The value of evasion or magical attack.</param>
        /// <param name="mpOrPhysDef">The value of MP or physical defense.</param>
        /// <param name="hpOrEvasion">The value of HP or evasion.</param>
        /// <param name="magicDef">The sum of all magical defenses.</param>
        /// <returns>The guessed equipment type.</returns>
        private EquipmentType GuessArmorOrAccessory(int physdefOrAtk,
                                                    int evasionOrMagAtk,
                                                    int mpOrPhysDef,
                                                    int hpOrEvasion,
                                                    int magicDef)
        {
            // Assume armor if nothing is set.
            if (physdefOrAtk == 0 && evasionOrMagAtk == 0 && mpOrPhysDef == 0 && hpOrEvasion == 0 && magicDef == 0)
            {
                return EquipmentType.Armor;
            }

            // Armor always have both physical defense and magical defense while accessories do not,
            // so assume armor if this is the case.
            if (physdefOrAtk != 0 && magicDef != 0)
            {
                return EquipmentType.Armor;
            }

            return EquipmentType.Accessory;
        }

        private void ParseAddons(int numAddons)
        {
            _equipment.Addons = new List<Addon>(numAddons);

            for (int i = 0; i < numAddons; i++)
            {
                // Soulgems and Refines are serialized as addons that are mixed in to the list of all addons, but we
                // try to seperate them so they can be better represented in the editor.
                bool addToAddonsList = true;

                Addon addon = new Addon();

                addon.Id = ReadInt();

                // The bit 0x8000 determines if the addon is hidden (e.g. intended to be used with soulgems). 
                if ((addon.Id & 0x8000) == 0x8000)
                {
                    addon.Id = addon.Id & ~0x8000;
                    addon.Hidden = true;
                }

                // The bits  0x2000, 0x4000, or 0x6000 determines the addon type. Set these bits to 0 to get the true
                // id of the addon that corresponds to List 1 in elements.data. It's unclear why addon ids are
                // serialized this way.
                if ((addon.Id & (int)(AddonType.UniqueDefensive)) == (int)(AddonType.UniqueDefensive))
                {
                    addon.Id = addon.Id & ~(int)(AddonType.UniqueDefensive);
                    addon.Type = AddonType.UniqueDefensive;

                    addon.Value = ReadAddonValue(addon.Id);
                    addon.Param2 = ReadInt();
                    addon.Param3 = ReadInt();
                }
                else if ((addon.Id & (int)(AddonType.UniqueOffensive)) == (int)(AddonType.UniqueOffensive))
                {
                    addon.Id = addon.Id & ~(int)(AddonType.UniqueOffensive);
                    addon.Type = AddonType.UniqueOffensive;

                    addon.Value = ReadAddonValue(addon.Id);
                    addon.Param2 = ReadInt();
                    
                    // Refine addons are guaranteed to be UniqueOffensive. Try to associate the addon with a refine.
                    if (addToAddonsList &&
                        (_equipment.Refine?.Id ?? 0) == 0 &&
                        Presets.Refine.ContainsKey(addon.Id.ToString()))
                    {
                        _equipment.Refine = addon;
                        addToAddonsList = false;
                    }
                }
                else if ((addon.Id & (int)(AddonType.Normal)) == (int)(AddonType.Normal))
                {
                    addon.Id = addon.Id & ~(int)(AddonType.Normal);
                    addon.Type = AddonType.Normal;

                    addon.Value = ReadAddonValue(addon.Id);
                }
                else
                {
                    addon.Type = AddonType.Unidentified;
                }

                // Soulgem addons are guaranteed to be hidden, so check if the addon is associated with a soulgem.
                if (addon.Hidden)
                {
                    if (addToAddonsList && GuessSoulgem(addon))
                    {
                        addToAddonsList = false;
                    }
                }

                if (addToAddonsList)
                {
                    _equipment.Addons.Add(addon);
                }
            }
        }

        /// <summary>
        /// Reads the next 4 bytes of the octet as the value of an addon.
        /// </summary>
        /// <param name="id">The id of the addon that's already been read.</param>
        /// <returns>The addon value of the next 4 bytes.</returns>
        /// <remarks>
        /// Some addons are represented as an int, some are represented as a float.
        /// </remarks>
        private decimal ReadAddonValue(int id)
        {
            if (Presets.RangeAddon.ContainsKey(id.ToString()))
            {
                return (decimal)ReadFloat();
            }

            return ReadInt();
        }

        /// <summary>
        /// Guess the socket/soulgem that the addon is associated with, and then associate it.
        /// </summary>
        /// <returns>true if the association was successfully made; otherwise false.</returns>
        private bool GuessSoulgem(Addon addon)
        {
            foreach (Socket socket in _equipment.Sockets)
            {
                // Check if the socket has a soulgem, but we haven't identified an associated addon yet.
                if (socket.Soulgem != 0 &&
                    !socket.HasAddon &&
                    Presets.Soulgem.ContainsKey(socket.Soulgem.ToString()))
                {
                    string[] soulgemPreset = Presets.Soulgem[socket.Soulgem.ToString()];
                    string[] soulgemAddonPreset;

                    // Check that the addon matches exactly what the soulgem should give.
                    // Since armor and accessories are indistinguishable from each other, we must check for both.
                    if (_equipment.Type == EquipmentType.Weapon &&
                        soulgemPreset.Length > 1 &&
                        Presets.SoulgemAddon.ContainsKey(soulgemPreset[1]))
                    {
                        soulgemAddonPreset = Presets.SoulgemAddon[soulgemPreset[1]];
                    }
                    else if (soulgemPreset.Length > 2 && Presets.SoulgemAddon.ContainsKey(soulgemPreset[2]))
                    {
                        soulgemAddonPreset = Presets.SoulgemAddon[soulgemPreset[2]];
                    }
                    else if (soulgemPreset.Length > 3 && Presets.SoulgemAddon.ContainsKey(soulgemPreset[3]))
                    {
                        soulgemAddonPreset = Presets.SoulgemAddon[soulgemPreset[3]];
                    }
                    else
                    {
                        return false;
                    }

                    bool valueIsGood = soulgemAddonPreset.Length <= 1 ||
                                       soulgemAddonPreset[1] == addon.Value.ToString();
                    bool param2IsGood = soulgemAddonPreset.Length <= 2 ||
                                        soulgemAddonPreset[2] == addon.Param2.ToString();
                    bool param3IsGood = soulgemAddonPreset.Length <= 3 ||
                                        soulgemAddonPreset[3] == addon.Param3.ToString();

                    if (valueIsGood && param2IsGood && param3IsGood)
                    {
                        socket.HasAddon = true;
                        return true;
                    }
                }
            }

            return false;
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
                        _equipment.PhysicalAttackLow -= (int)addon.Value;
                        _equipment.PhysicalAttackHigh -= (int)addon.Value;
                        continue;
                    }

                    if (Presets.PhysicalDefenseAddon.ContainsKey(id))
                    {
                        _equipment.ArmorPhysicalDefense -= (int)addon.Value;
                        _equipment.AccessoryPhysicalDefense -= (int)addon.Value;
                        continue;
                    }

                    if (Presets.HPAddon.ContainsKey(id))
                    {
                        _equipment.HP -= (int)addon.Value;
                        continue;
                    }

                    if (Presets.RangeAddon.ContainsKey(id))
                    {
                        _equipment.Range -= (float)addon.Value;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Reads the next 4 bytes of the octet as an int.
        /// </summary>
        /// <returns>The int value of the next 4 bytes.</returns>
        private int ReadInt()
        {
            // 2 chars = 1 byte.
            string hex = _octet.Substring(_position, 8);
            byte[] bytes = HexStringToBytes(hex);

            _position += 8;

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reads the next 2 bytes of the octet as a short.
        /// </summary>
        /// <returns>The short value of the next 2 bytes.</returns>
        private short ReadShort()
        {
            // 2 chars = 1 byte.
            string hex = _octet.Substring(_position, 4);
            byte[] bytes = HexStringToBytes(hex);

            _position += 4;

            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reads the next byte of the octet.
        /// </summary>
        /// <returns>The byte value of the next byte.</returns>
        private byte ReadByte()
        {
            // 2 chars = 1 byte.
            string hex = _octet.Substring(_position, 2);
            byte[] bytes = HexStringToBytes(hex);

            _position += 2;

            return bytes[0];
        }

        /// <summary>
        /// Reads the next 4 bytes of the octet as a float.
        /// </summary>
        /// <returns>The float value of the next 4 bytes.</returns>
        private float ReadFloat()
        {
            // 2 chars = 1 byte.
            string hex = _octet.Substring(_position, 8);
            byte[] bytes = HexStringToBytes(hex);

            _position += 8;

            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Reads the next length bytes of the octet as a UTF-16 string, where each char is 2 bytes.
        /// </summary>
        /// <param name="length">Number of bytes.</param>
        /// <returns>The string value of the next length bytes.</returns>
        private string ReadString16(int length)
        {
            // 2 chars = 1 byte.
            string hex = _octet.Substring(_position, length * 2);
            byte[] bytes = HexStringToBytes(hex);

            _position += length * 2;

            return System.Text.Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Converts a hex string to a byte array.
        /// </summary>
        /// <remarks>
        /// https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        /// </remarks>
        /// <param name="hex">The hex string.</param>
        /// <returns>The equivalent byte array.</returns>
        private byte[] HexStringToBytes(string hex)
        {
            if (hex.Length % 2 == 1)
            {
                throw new Exception("Invalid hex string");
            }

            byte[] bytes = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                bytes[i] = (byte)((HexToVal(hex[i << 1]) << 4) + (HexToVal(hex[(i << 1) + 1])));
            }

            return bytes;
        }

        public static int HexToVal(char hex)
        {
            int val = (int)(hex);
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
