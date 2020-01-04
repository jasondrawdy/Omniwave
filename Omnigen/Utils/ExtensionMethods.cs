/*
==============================================================================
Copyright © Jason Drawdy (CloneMerge)

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

using System;
using System.Text;
using System.Collections.Generic;

#endregion
namespace Omnigen.Utils
{
    /// <summary>
    /// Common extension methods to help with data manipulation.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a string into a byte array.
        /// </summary>
        /// <param name="input">The string to convert into a byte array.</param>
        public static byte[] ToBytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
        /// <summary>
        /// Encodes a byte array into its string representation.
        /// </summary>
        /// <param name="input">The array to encode into a string.</param>
        public static string FromBytes(this byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
        /// <summary>
        /// Remove all null strings from a string array.
        /// </summary>
        /// <param name="input">The array to remove null string from.</param>
        public static string[] RemoveNulls(this string[] input)
        {
            try
            {
                List<string> output = new List<string>();
                foreach (string item in input)
                {
                    if (item != null && item != "")
                        output.Add(item);
                }
                return output.ToArray();
            }
            catch { return null; }
        }
        /// <summary>
        /// Captializes the first letter of a given string.
        /// </summary>
        /// <param name="s">The string to capitalize.</param>
        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter.");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
