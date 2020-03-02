using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD2
{
    class Pixel
    {
        private byte red;
        private byte green;
        private byte blue;

        public Pixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public byte Red
        {
            get
            {
                return this.red;
            }
            set
            {
                red = value;
            }
        }

        public byte Green
        {
            get
            {
                return this.green;
            }
            set
            {
                green = value;
            }
        }

        public byte Blue
        {
            get
            {
                return this.blue;
            }
            set
            {
                blue = value;
            }
        }

        public void ConvertToGrey(byte red, byte green, byte blue)
        {
            this.red = Convert.ToByte(Math.Round(0.2125 * red));
            this.green = Convert.ToByte(Math.Round(0.7154 * green));
            this.blue = Convert.ToByte(Math.Round(0.0721 * blue));
        }

        public void ChangerCouleur(byte red, byte green, byte blue)
        {
            this.red = Convert.ToByte(red);
            this.green = Convert.ToByte(green);
            this.blue = Convert.ToByte(blue);
        }
    }
}
