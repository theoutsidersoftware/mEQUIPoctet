using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace mEQUIPoctet.Source.Config
{
    public class IniParser
    {
        /// <summary>
        /// The path to the .ini file.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Initializes the parser for .ini file located at the given path.
        /// </summary>
        /// <param name="path">The path to .ini file.</param>
        public IniParser(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Reads a .ini file and parses it into an dictionary, where each key represents a section, and each value is
        /// a key value pair representing an entry in the section.
        /// </summary>
        /// <returns>A dictionary representing the .ini file.</returns>
        public IDictionary<string, IDictionary<string, string>> Parse()
        {
            var ini = new Dictionary<string, IDictionary<string, string>>();
            IDictionary<string, string> currentSection = null;

            StreamReader file = new StreamReader(_path);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                // Comments.
                if (line.StartsWith(";") || line.StartsWith("#"))
                {
                    // Ignore.
                    continue;
                }

                if (line.StartsWith(@"[") && line.EndsWith(@"]"))
                {
                    // Parsing a section.
                    string sectionName = TryParseSection(line);

                    if (sectionName == null)
                    {
                        // Fail gracefully.
                        continue;
                    }

                    if (!ini.ContainsKey(sectionName))
                    {
                        IDictionary<string, string> section = new Dictionary<string, string>();
                        ini.Add(sectionName, section);
                        currentSection = section;
                    }
                    else
                    {
                        currentSection = ini[sectionName];
                    }
                }
                else
                {
                    if (currentSection == null)
                    {
                        // Fail gracefully.
                        continue;
                    }

                    KeyValuePair<string, string>? wrapper = TryParseKeyValue(line);

                    if (!wrapper.HasValue)
                    {
                        // Fail gracefully.
                        continue;
                    }

                    KeyValuePair<string, string> kvp = wrapper.Value;

                    currentSection.Add(kvp);
                }
            }

            return ini;
        }

        /// <summary>
        /// Try to parse the name of a section.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The name of the section, or null if unable to parse.</returns>
        private string TryParseSection(string line)
        {
            Regex regex = new Regex(@"^\[(.+?)\]$", RegexOptions.None);
            Match match = regex.Match(line);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// Try to parse a key value pair.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>The key value pair, or null if unable to parse.</returns>
        private KeyValuePair<string, string>? TryParseKeyValue(string line)
        {
            // Try to match a setting.
            Regex regex = new Regex(@"^(.+?)=(.+?)$", RegexOptions.None);
            Match match = regex.Match(line);

            if (match.Success)
            {
                string key = match.Groups[1].Value.Trim();
                string value = match.Groups[2].Value.Trim();

                return new KeyValuePair<string, string>(key, value);
            }

            return null;
        }
    }
}
