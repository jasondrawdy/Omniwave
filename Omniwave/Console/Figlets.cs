/*
==============================================================================
Copyright © Jason Tanner (Antebyte)

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using Colorful;

#endregion
namespace Omniwave
{
    #region Enums

    /// <summary>
    /// Types of fonts available for logging purposes.
    /// </summary>
    public enum FigletName
    {
        Acrobatic = 0,
        Alligator = 1,
        Alligator2 = 2,
        Alphabet = 3,
        Avatar = 4,
        Banner = 5,
        Banner3 = 6,
        Banner3D = 7,
        Banner4 = 8,
        Barbwire = 9,
        Basic = 10,
        Bell = 11,
        Big = 12,
        Bigchief = 13,
        Binary = 14,
        Block = 15,
        Bubble = 16,
        Bulbhead = 17,
        Calgphy2 = 18,
        Caligraphy = 19,
        Catwalk = 20,
        Chunky = 21,
        Coinstak = 22,
        Colossal = 23,
        Computer = 24,
        Contessa = 25,
        Contrast = 26,
        Cosmic = 27,
        Cosmike = 28,
        Cricket = 29,
        Cursive = 30,
        Cyberlarge = 31,
        Cybermedium = 32,
        Cybersmall = 33,
        Diamond = 34,
        Digital = 35,
        Doh = 36,
        Doom = 37,
        Dotmatrix = 38,
        Drpepper = 39,
        Eftichess = 40,
        Eftifont = 41,
        Eftipiti = 42,
        Eftirobot = 43,
        Eftitalic = 44,
        Eftiwall = 45,
        Eftiwater = 46,
        Epic = 47,
        Fender = 48,
        Fourtops = 49,
        Fuzzy = 50,
        Goofy = 51,
        Gothic = 52,
        Graffiti = 53,
        Hollywood = 54,
        Invita = 55,
        Isometric1 = 56,
        Isometric2 = 57,
        Isometric3 = 58,
        Isometric4 = 59,
        Italic = 60,
        Ivrit = 61,
        Jazmine = 62,
        Jerusalem = 63,
        Katakana = 64,
        Kban = 65,
        Larry3d = 66,
        Lcd = 67,
        Lean = 68,
        Letters = 69,
        Linux = 70,
        Lockergnome = 71,
        Madrid = 72,
        Marquee = 73,
        Maxfour = 74,
        Mike = 75,
        Mini = 76,
        Mirror = 77,
        Mnemonic = 78,
        Morse = 79,
        Moscow = 80,
        NancyjFancy = 81,
        NancyjUnderlined = 82,
        Nancyj = 83,
        Nipples = 84,
        Ntgreek = 85,
        O8 = 86,
        Ogre = 87,
        Pawp = 88,
        Peaks = 89,
        Pebbles = 90,
        Pepper = 91,
        Poison = 92,
        Puffy = 93,
        Pyramid = 94,
        Rectangles = 95,
        Relief = 96,
        Relief2 = 97,
        Rev = 98,
        Roman = 99,
        Rot13 = 100,
        Rounded = 101,
        Rowancap = 102,
        Rozzo = 103,
        Runic = 104,
        Runyc = 105,
        Sblood = 106,
        Script = 107,
        Serifcap = 108,
        Shadow = 109,
        Short = 110,
        Slant = 111,
        Slide = 112,
        Slscript = 113,
        Small = 114,
        Smisome1 = 115,
        Smkeyboard = 116,
        Smscript = 117,
        Smshadow = 118,
        Smslant = 119,
        Smtengwar = 120,
        Speed = 121,
        Stampatello = 122,
        Standard = 123,
        Starwars = 124,
        Stellar = 125,
        Stop = 126,
        Straight = 127,
        Tanja = 128,
        Tengwar = 129,
        Term = 130,
        Thick = 131,
        Thin = 132,
        Threepoint = 133,
        Ticks = 134,
        Ticksslant = 135,
        TinkerToy = 136,
        Tombstone = 137,
        Trek = 138,
        Tsalagi = 139,
        Twopoint = 140,
        Univers = 141,
        Usaflag = 142,
        Wavy = 143,
        Weird = 144,
        X3d = 145,
        X3x5 = 146,
        X5lineoblique = 147,
    }

    #endregion
    /// <summary>
    /// Contains fonts used for styled logging.
    /// </summary>
    public static class Figlets
    {
        #region Methods

        /// <summary>
        /// Obtains a font given the name of the font.
        /// </summary>
        /// <param name="name">The name of the font.</param>
        public static Figlet GetFiglet(FigletName name)
        {
            string objectName = ("_" + ((int)name).ToString());
            byte[] font = (byte[])Properties.Resources.ResourceManager.GetObject(objectName);
            FigletFont figlet = FigletFont.Load(font);
            return new Figlet(figlet);
        }

        #endregion
    }
}
