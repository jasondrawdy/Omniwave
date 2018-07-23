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

// Default system libraries.
using System;
using System.Drawing;
using System.Threading.Tasks;

// Internal API imports.
using Omnigen.Generators;

// External API imports.
using Colorful;
using Console = Colorful.Console;

#endregion
namespace Omniwave
{
    class Program
    {
        #region Initialization

        static void Main(string[] args)
        {
            Toolbox.Logging.PrintGreeting();
            WaveProperties properties = GetWaveProperties(args);
            if (properties.Error == null)
                Task.Run(() => CalculateWave(properties.Singularity, properties.Bailout, properties.Interval, properties.WaveFactor));
            else
                Toolbox.Logging.PrintUsage(properties.Error.Message);
            Console.Read();
        }

        #endregion
        #region Methods

        static WaveProperties GetWaveProperties(string[] args = null)
        {
            try
            {
                DateTime zeroPointDate = new DateTime(2018, 7, 8, 0, 0, 0, 0);
                DateTime currentDate = DateTime.Now;
                double daysBefore = 31; //(zeroPointDate - currentDate).Days;
                double daysAfter = 0.01; // The amount of days after the initial zero-point to calculate the wave.
                double timeInterval = 60; // Represented in minutes.
                int waveFactor = 64; // Can be anywhere between 2 & 10,000.

                // Check if we should set our variables using the provided arguments, if any.
                if (args != null)
                {
                    if (CheckArguments(args))
                    {
                        daysBefore = double.Parse(args[0]);
                        daysAfter = double.Parse(args[1]);
                        timeInterval = double.Parse(args[2]);
                        waveFactor = int.Parse(args[3]);
                    }
                }
                return new WaveProperties(daysBefore, daysAfter, timeInterval, waveFactor);
            }
            catch (Exception ex) { return new WaveProperties(0, 0, 0, 0, new Exception(ex.Message)); }
        }

        static bool CheckArguments(string[] args)
        {
            if (args.Length == 0)
                return false;
            else
            {
                // Check if we have the appropriate number of arguments.
                if (args.Length > 0 && args.Length < 4) throw new Exception("All arguments are required in order for Omniwave to function properly.");
                if (args.Length > 4) throw new Exception("Omniwave only accepts a total of 4 arguments.");

                // Check all arguments for negative numbers.
                if (int.Parse(args[0]) < 0) throw new Exception("The singularity cannot be a negative number.");
                if (int.Parse(args[1]) < 0) throw new Exception("The bailout cannot be a negative number.");
                if (int.Parse(args[2]) < 0) throw new Exception("The time interval cannot be a negative number.");
                if (int.Parse(args[3]) < 0) throw new Exception("The wave factor cannot be a negative number.");

                // Check our wave factor's value to see if it falls within an acceptable range.
                if (int.Parse(args[3]) < 2) throw new Exception("The wave factor must be an integer within 2 - 10,000.");
                if (int.Parse(args[3]) > 10000) throw new Exception("The wave factor must be an integer within 2 - 10,000.");
                return true;
            }
        }

        static async void CalculateWave(double daysBeforeZeroPoint, double daysAfterZeroPoint, double timeInterval, int waveFactor)
        {
            WaveGenerator wave = new WaveGenerator(daysBeforeZeroPoint, daysAfterZeroPoint, timeInterval, waveFactor);
            wave.OnWavePointGenerated += Toolbox.Events.OnWavePointGenerated;
            wave.OnWaveGenerationComplete += Toolbox.Events.OnWaveGenerationComplete;
            await wave.GenerateAsync();
        }

        #endregion
    }

    /// <summary>
    /// A common collection of tools used in applications such as logging, events, and etc.
    /// </summary>
    class Toolbox
    {
        /// <summary>
        /// A collection of common logging methods typical of a CLI application.
        /// </summary>
        internal class Logging
        {
            #region Variables

            /// <summary>
            /// Main output logging styles.
            /// </summary>
            internal enum OutputType { None, General, Notice, Success, Warning, Error }

            #endregion
            #region Methods

            internal static void PrintTimestamp()
            {
                string timestamp = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString();
                Console.Write("[", Color.White);
                Console.Write(timestamp, ColorTranslator.FromHtml("#FAD6FF"));
                Console.Write("] ", Color.White);
            }

            internal static void PrintGreeting()
            {
                Figlet figlet = Figlets.GetFiglet(FigletName.Basic);
                Console.WriteLine("===============================\n\n\n", Color.White);
                Console.WriteLine(figlet.ToAscii("Omniwave"), ColorTranslator.FromHtml("#B8DBFF"));
                Console.WriteLine("===============================", Color.White);
                Console.Write("Written by: ", ColorTranslator.FromHtml("#efefef"));
                Console.WriteLine("Jason Tanner", ColorTranslator.FromHtml("#8AFFEF"));
                Console.Write("Original by: ", ColorTranslator.FromHtml("#efefef"));
                Console.WriteLine("Terence McKenna & Peter Meyer", ColorTranslator.FromHtml("#8AFFEF"));
                Console.Write("Ported by: ", ColorTranslator.FromHtml("#efefef"));
                Console.WriteLine("John A. Phelps", ColorTranslator.FromHtml("#8AFFEF"));
                Console.Write("Date: ", ColorTranslator.FromHtml("#efefef"));
                Console.WriteLine("6.9.2018", ColorTranslator.FromHtml("#8AFFEF"));
                Console.Write("Description: ", ColorTranslator.FromHtml("#efefef"));
                Console.WriteLine("Calculates a timewave to a given point and beyond.", ColorTranslator.FromHtml("#8AFFEF"));
                Console.WriteLine("===============================", Color.White);
            }

            internal static void PrintUsage(string message = null)
            {
                if (message != null) PrintOutput(message, OutputType.Error);
                Console.WriteLine("Usage: Omniwave.exe {daysUntilSingularity} {daysAfterSingularity} {timeInterval} {waveFactor}");
            }

            /// <summary>
            /// Prints a formatted string given an output type or style.
            /// </summary>
            /// <param name="output">The data to print to the console.</param>
            /// <param name="type">The style the data should be formatted in.</param>
            internal static void PrintOutput(string output, OutputType type)
            {
                PrintTimestamp();
                switch (type)
                {
                    case OutputType.None:
                        Console.WriteLine("[-]: {0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                    case OutputType.General:
                        Console.Write("[", Color.White);
                        Console.Write("-", Color.White);
                        Console.Write("]: ", Color.White);
                        Console.WriteLine("{0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                    case OutputType.Notice:
                        Console.Write("[", Color.White);
                        Console.Write("#", ColorTranslator.FromHtml("#B8DBFF"));
                        Console.Write("]: ", Color.White);
                        Console.WriteLine("{0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                    case OutputType.Success:
                        Console.Write("[", Color.White);
                        Console.Write("+", Color.LawnGreen);
                        Console.Write("]: ", Color.White);
                        Console.WriteLine("{0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                    case OutputType.Warning:
                        Console.Write("[", Color.White);
                        Console.Write("!", Color.Yellow);
                        Console.Write("]: ", Color.White);
                        Console.WriteLine("{0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                    case OutputType.Error:
                        Console.Write("[", Color.White);
                        Console.Write("x", Color.Red);
                        Console.Write("]: ", Color.White);
                        Console.WriteLine("{0}", output, ColorTranslator.FromHtml("#efefef"));
                        break;
                }
            }

            #endregion
        }

        /// <summary>
        /// A collection of events which can be used by the <see cref="WaveGenerator"/> class.
        /// </summary>
        internal class Events
        {
            #region Methods

            /// <summary>
            /// Occurs when a point of the omniwave has been generated.
            /// </summary>
            internal static void OnWavePointGenerated(object sender, WavePointGeneratedEventArgs args)
            {
                Logging.PrintOutput(args.Output, Logging.OutputType.Notice);
            }
            /// <summary>
            /// Occurs when the entire omniwave has been calculated.
            /// </summary>
            internal static void OnWaveGenerationComplete(object sender, WaveGenerationCompleteEventArgs args)
            {
                if (args.Successful)
                    Logging.PrintOutput("Wave generated successfully!", Logging.OutputType.Success);
                else
                {
                    if (args.Error != null)
                        Logging.PrintOutput(args.Error.Message, Logging.OutputType.Error);
                    else
                        Logging.PrintOutput("The wave could not be generated.", Logging.OutputType.Error);
                }
            }

            #endregion
        }
    }
}
