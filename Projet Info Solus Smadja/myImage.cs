using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TD2
{
    class MyImage
    {
        private string type;
        private int taille;
        private int offset;
        private int hauteur;
        private int largeur;
        private int nbBits;
        private Pixel[,] matriceRGB;
        private byte[] header;

        public MyImage(string type, int taille, int offset, int hauteur, int largeur, int nbBits, Pixel[,] matriceRGB, byte[] header)
        {
            this.type = type;
            this.taille = taille;
            this.offset = offset;
            this.hauteur = hauteur;
            this.largeur = largeur;
            this.nbBits = nbBits;
            this.matriceRGB = matriceRGB;
        }

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    type = null;
                }
                else
                {
                    type = value;
                }
            }
        }

        public int Taille
        {
            get
            {
                return this.taille;
            }
            set
            {
                taille = value;
            }
        }

        public int Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                offset = value;
            }
        }

        public int Largeur
        {
            get
            {
                return this.largeur;
            }
            set
            {
                largeur = value;
            }
        }

        public int Hauteur
        {
            get
            {
                return this.hauteur;
            }
            set
            {
                hauteur = value;
            }
        }

        public int NbBits
        {
            get
            {
                return this.nbBits;
            }
            set
            {
                nbBits = value;
            }
        }

        public Pixel[,] MatriceRGB
        {
            get
            {
                return this.matriceRGB;
            }
            set
            {
                matriceRGB = value;
            }
        }

        public byte[] Header
        {
            get
            {
                return this.header;
            }
            set
            {
                if (value == null)
                {
                    header = null;
                }
                else
                {
                    header = value;
                }
            }
        }

        public MyImage(string myFile)
        {
            byte[] fileByte = File.ReadAllBytes(myFile);


            //type
            type = Convert.ToString(Convert.ToChar(fileByte[0]) + Convert.ToChar(fileByte[1]));

            //taille
            byte[] tailleByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tailleByte[i] = fileByte[i + 2];
            }
            taille = Convertir_Endian_To_Int(tailleByte);

            //offset
            byte[] offsetByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                offsetByte[i] = fileByte[i + 10];
            }
            offset = Convertir_Endian_To_Int(offsetByte);

            //largeur
            byte[] largeurByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                largeurByte[i] = fileByte[i + 18];
            }
            largeur = Convertir_Endian_To_Int(largeurByte);

            //hauteur
            byte[] hauteurByte = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                hauteurByte[i] = fileByte[i + 22];
            }
            hauteur = Convertir_Endian_To_Int(hauteurByte);

            //nbBits
            byte[] nbBitsByte = new byte[4];
            for (int i = 0; i < 2; i++)
            {
                tailleByte[i] = fileByte[i + 28];
            }
            nbBits = Convertir_Endian_To_Int(tailleByte);

            //image
            for (int i = 54; i < fileByte.Length; i = i + 3)
            {
                Pixel pix = new Pixel(fileByte[i + 2], fileByte[i + 1], fileByte[i]);
                matriceRGB[(i - 54) % largeur, (i - 54) / 54] = pix;
            }

        }

        public byte[] From_Image_To_File()
        {
            byte[] file = new byte[54 + 3 * (hauteur + largeur)];
            for (int i = 0; i < 54; i++)
            {
                file[i] = 0;
            }
            for (int i = 0; i < 2; i++)
            {
                file[i] = Convert.ToByte(type[i]);
            }

            //taille
            byte[] tailleByte = Convertir_Int_To_Endian(taille);
            for (int i = 0; i < 4; i++)
            {
                file[i + 3] = tailleByte[i];
            }

            //offset
            byte[] offsetByte = Convertir_Int_To_Endian(offset);
            for (int i = 0; i < 4; i++)
            {
                file[i + 10] = offsetByte[i];
            }

            //largeur
            byte[] largeurByte = Convertir_Int_To_Endian(largeur);
            for (int i = 0; i < 4; i++)
            {
                file[i + 18] = largeurByte[i];
            }

            //hauteur
            byte[] hauteurByte = Convertir_Int_To_Endian(hauteur);
            for (int i = 0; i < 4; i++)
            {
                file[i + 22] = hauteurByte[i];
            }

            //nbBits
            byte[] nbBitsByte = Convertir_Int_To_Endian(nbBits);
            for (int i = 0; i < 2; i++)
            {
                file[i + 28] = nbBitsByte[i];
            }

            for (int i = 54; i < (54 + hauteur * largeur); i = i + 3)
            {
                file[i] = matriceRGB[(i - 54) % largeur, (i - 54) / 54].Blue;
                file[i + 1] = matriceRGB[(i - 54) % largeur, (i - 54) / 54].Green;
                file[i + 2] = matriceRGB[(i - 54) % largeur, (i - 54) / 54].Red;
            }

            return file;
        }

        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int valeur = 0;
            double valeurDouble = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                valeurDouble = valeurDouble + Convert.ToDouble(tab[i]) * Math.Pow(256, i);
            }
            valeur = Convert.ToInt32(valeurDouble);
            return valeur;
        }

        public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] tab = new byte[4];
            int i = 3;
            while (val != 0)
            {
                int div = Convert.ToInt32(Math.Pow(256, i));
                if (div > val)
                {
                    tab[i] = 0;
                }
                else
                {
                    tab[i] = Convert.ToByte((val / div));
                    val = val % Convert.ToInt32(Math.Pow(256, i));
                }
                i--;
            }
            return tab;
        }

        public void toStringMatriceRGB()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Console.Write(matriceRGB[i, j].Red);
                    Console.Write(matriceRGB[i, j].Green);
                    Console.Write(matriceRGB[i, j].Blue);
                }
                Console.WriteLine();
            }
        }

        public void ModifierLaTaille(int coef)
        {
            this.taille = hauteur * largeur * coef * coef * 3 + 54;
            this.hauteur = hauteur * coef;
            this.largeur = largeur * coef;
            //this.header = header;
        }

        public void DetectionContour()
        {
            MyImage imageContour = new MyImage(type, taille, offset, hauteur, largeur, nbBits, matriceRGB, header);
            int[,] matriceNoyau = new int[,] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
            for(int i = 0; i < largeur; i++)
            {
                for(int j = 0; j < hauteur; j++)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if ((i+k <0)||(i+k>hauteur)||(j+l<0)||(j+l>largeur))
                            {

                            }
                            else
                            {
                                red = Convert.ToByte(Convert.ToInt32(red) + matriceNoyau[k + 1, l + 1] *Convert.ToInt32(matriceRGB[i + k, i + l].Red));
                                green = Convert.ToByte(Convert.ToInt32(green) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Green));
                                blue = Convert.ToByte(Convert.ToInt32(blue) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Blue));
                            }
                        }
                    }
                    imageContour.matriceRGB[i, j].ChangerCouleur(red, green, blue);
                }
            }
            this.matriceRGB = imageContour.matriceRGB;
        }

        public void RenforcementBords()
        {
            MyImage imageRenforcementBords = new MyImage(type, taille, offset, hauteur, largeur, nbBits, matriceRGB, header);
            int[,] matriceNoyau = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if ((i + k < 0) || (i + k > hauteur) || (j + l < 0) || (j + l > largeur))
                            {

                            }
                            else
                            {
                                red = Convert.ToByte(Convert.ToInt32(red) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Red));
                                green = Convert.ToByte(Convert.ToInt32(green) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Green));
                                blue = Convert.ToByte(Convert.ToInt32(blue) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Blue));
                            }
                        }
                    }
                    imageRenforcementBords.matriceRGB[i, j].ChangerCouleur(red, green, blue);
                }
            }
            this.matriceRGB = imageRenforcementBords.matriceRGB;
        }

        public void Flou()
        {
            MyImage imageFlou = new MyImage(type, taille, offset, hauteur, largeur, nbBits, matriceRGB, header);
            int[,] matriceNoyau = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if ((i + k < 0) || (i + k > hauteur) || (j + l < 0) || (j + l > largeur))
                            {

                            }
                            else
                            {
                                red = Convert.ToByte(Convert.ToInt32(red) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Red));
                                green = Convert.ToByte(Convert.ToInt32(green) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Green));
                                blue = Convert.ToByte(Convert.ToInt32(blue) + matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Blue));
                            }
                        }
                    }
                    red = Convert.ToByte(Convert.ToInt32((1 / 9) * red));
                    green = Convert.ToByte(Convert.ToInt32((1 / 9) * green));
                    blue = Convert.ToByte(Convert.ToInt32((1 / 9) * blue));
                    imageFlou.matriceRGB[i, j].ChangerCouleur(red, green, blue);
                }
            }
            this.matriceRGB = imageFlou.matriceRGB;
        }

        public void Repoussage()
        {
            MyImage imageRepoussBord = new MyImage(type, taille, offset, hauteur, largeur, nbBits, matriceRGB, header);
            int[,] matriceNoyau = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if ((i + k < 0) || (i + k > hauteur) || (j + l < 0) || (j + l > largeur))
                            {

                            }
                            else
                            {
                                red = Convert.ToByte(Convert.ToInt32(red) + (1 / 9) * matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Red));
                                green = Convert.ToByte(Convert.ToInt32(green) + (1 / 9) * matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Green));
                                blue = Convert.ToByte(Convert.ToInt32(blue) + (1 / 9) * matriceNoyau[k + 1, l + 1] * Convert.ToInt32(matriceRGB[i + k, i + l].Blue));
                            }
                        }
                    }
                    imageRepoussBord.matriceRGB[i, j].ChangerCouleur(red, green, blue);
                }
            }

            this.matriceRGB = imageRepoussBord.matriceRGB;
        }
    }
}
