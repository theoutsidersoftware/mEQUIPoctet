using System.Windows.Media;

namespace mEQUIPoctet.Source.UI
{
    /// <summary>
    /// Represents a 15-bit RGB555 color.
    /// </summary>
    class Color15
    {
        /// <summary>
        /// The 16-bit color value.
        /// </summary>
        public short Color { get; private set; } = 0;

        private byte _r = 0;
        private byte _g = 0;
        private byte _b = 0;

        private Color15()
        {
            // do nothing.
        }

        /// <summary>
        /// Create a Color15 using a value representing a 15-bit RGB555 color.
        /// </summary>
        /// <param name="color">The value representing a 15-bit RGB555 color.</param>
        /// <returns>The equivalent Color15 object.</returns>
        public Color15(short color)
        {
            Color = color;

            // Isolate the bits for each value.
            int r = color & 0b0111110000000000;
            int g = color & 0b0000001111100000;
            int b = color & 0b0000000000011111;

            // Shift them to the base position.
            _r = (byte)(r >> 10);
            _g = (byte)(g >> 5);
            _b = (byte)(b);
        }

        /// <summary>
        /// Convert from a 24-bit RGB888 color to 15-bit RGB555 color.
        /// </summary>
        /// <param name="color24">The 24-bit RGB888 color.</param>
        /// <returns>The nearest 15-bit RGB555 color.</returns>
        static public Color15 ConvertFrom24(Color color24)
        {
            Color15 color15 = new Color15();

            // Convert 8-bits to 5-bits.
            int r = color24.R >> 3;
            int g = color24.G >> 3;
            int b = color24.B >> 3;

            color15._r = (byte)r;
            color15._g = (byte)g;
            color15._b = (byte)b;

            // Shift to the correct positions.
            r = r << 10;
            g = g << 5;

            // Combine.
            color15.Color = (short)(r | g | b);

            return color15;
        }

        /// <summary>
        /// Converts this 15-bit RGB555 color to a 24-bit RGB888 color.
        /// </summary>
        /// <returns>The equivalent RGB888 color.</returns>
        public Color ConvertTo24()
        {
            // Get the high bits.
            int rh = _r >> 2;
            int gh = _g >> 2;
            int bh = _b >> 2;

            // Duplicate the high bits to the low bits.
            byte r = (byte)((_r << 3) | rh);
            byte g = (byte)((_g << 3) | gh);
            byte b = (byte)((_b << 3) | bh);

            return System.Windows.Media.Color.FromRgb(r, g, b);
        }
    }
}
