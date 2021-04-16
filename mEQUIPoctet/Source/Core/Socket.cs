using mEQUIPoctet.Source.Config;

namespace mEQUIPoctet.Source.Core
{
    public class Socket
    {
        /// <summary>
        /// The id of the soulgem in the socket, if any.
        /// </summary>
        public int Soulgem { get; set; } = 0;

        /// <summary>
        /// Whether the soulgem grants an addon.
        /// </summary>
        /// <remarks>
        /// A soulgem doesn't automatically grant addons effects. Instead, if this is true, then an addon from the
        /// SoulgemAddon Presets will be explicitly added to the list of addons.
        /// </remarks>
        public bool HasAddon { get; set; } = true;

        /// <summary>
        /// Sets the soulgem of the socket, and return whether it was successful.
        /// </summary>
        /// <param name="soulgem">The string representation of the id.</param>
        /// <returns>Whether the id was set successfully.</returns>
        public bool SetSoulgem(string soulgem)
        {
            int parsedSoulgem;
            if (int.TryParse(soulgem, out parsedSoulgem))
            {
                Soulgem = parsedSoulgem;
                return true;
            }

            return false;
        }
    }
}
