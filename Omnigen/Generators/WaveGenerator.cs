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
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Omnigen.Utils;

#endregion
namespace Omnigen.Generators
{
    #region Enums

    /// <summary>
    /// Collection of the currently available datasets to calculate waves with.
    /// </summary>
    public enum WaveType { Kelley = 1, Watkins = 2, Sheliak = 3, HuangTi = 4 }

    #endregion
    #region Structs

    /// <summary>
    /// Proper structure for a timewave encapsualting all of its properties.
    /// </summary>
    [Serializable]
    public struct WaveProperties
    {
        /// <summary>
        /// The point at which the wave should stop being calculated.
        /// </summary>
        public double Singularity { get; private set; }
        /// <summary>
        /// The point to continue calculating towards after the initial singularity (zero point).
        /// </summary>
        public double Bailout { get; private set; }
        /// <summary>
        /// A value to increment the wave represented in minutes.
        /// </summary>
        public double Interval { get; private set; }
        /// <summary>
        /// A value representing the degree or amount of novelty displayed in the wave. 64 is the default.
        /// </summary>
        public int WaveFactor { get; private set; }
        /// <summary>
        /// Error object to be checked before continuing with the wave calculation.
        /// </summary>
        public Exception Error { get; private set; }
        /// <summary>
        /// Allows the construction of a timewave with traditional structure and error handling.
        /// </summary>
        /// <param name="daysBeforeSingularity">The point at which the wave should stop being calculated.</param>
        /// <param name="daysAfterSingularity">The point to continue calculating towards after the initial singularity (zero point).</param>
        /// <param name="timeInterval">A value to increment the wave represented in minutes.</param>
        /// <param name="waveFactor">A value representing the degree or amount of novelty displayed in the wave. 64 is the default.</param>
        /// <param name="error">Error object to be checked before continuing with the wave calculation.</param>
        public WaveProperties(double daysBeforeSingularity, double daysAfterSingularity, double timeInterval, int waveFactor, Exception error = null)
        {
            Singularity = daysBeforeSingularity;
            Bailout = daysAfterSingularity;
            Interval = timeInterval;
            WaveFactor = waveFactor;
            Error = error;
        }
    }

    #endregion
    /// <summary>
    /// Allows calculation of alternative versions of a timewave.
    /// </summary>
    public class WaveGenerator
    {
        // TODO: Add ability to calculate past the Bailout.

        #region Variables

        /// <summary>
        /// Occurs when a point of the omniwave has been generated.
        /// </summary>
        public event WavePointGeneratedEventHandler OnWavePointGenerated;
        /// <summary>
        /// Occurs when the entire omniwave has been generated.
        /// </summary>
        public event WaveGenerationCompleteEventHnadler OnWaveGenerationComplete;
        /// <summary>
        /// The type of wave to be generated considering the provided datasets.
        /// </summary>
        public WaveType Type { get; private set; }
        /// <summary>
        /// The point of maximum novelty the wave can calculate towards.
        /// </summary>
        public double Singularity { get; private set; }
        /// <summary>
        /// The amount of time after the singularity to calculate.
        /// </summary>
        public double Bailout { get; private set; }
        /// <summary>
        /// The amount of time to step — in minutes — towards the singularity.
        /// </summary>
        public double Step { get; private set; }
        /// <summary>
        /// The weight of each point calculated in the wave.
        /// </summary>
        public int WaveFactor { get; private set; }
        const int WaveValue = 1000000;
        private int _precision = 16;
        private double[] _powers = new double[64];
        private int[,] _waveSets = GetDataSets();

        #endregion
        #region Initialization

        /// <summary>
        /// Allows the ability to calculate a timewave.
        /// </summary>
        /// <param name="daysBeforeZeroPoint">The amount of time before the point of maximum novelty to calculate.</param>
        /// <param name="daysAfterZeroPoint">The amount of time after the inital singularity to calculate.</param>
        /// <param name="timeInterval">The amount of time to step — in minutes — towards the singularity.</param>
        /// <param name="waveFactor">The weight of each point calculated in the wave.</param>
        /// <param name="type">The type of wave to be generated.</param>
        public WaveGenerator(double daysBeforeZeroPoint, double daysAfterZeroPoint, double timeInterval, int waveFactor, WaveType type = WaveType.Kelley)
        {
            // Initialize our properties.
            Type = type;
            Singularity = daysBeforeZeroPoint;
            Bailout = daysAfterZeroPoint;
            Step = timeInterval;
            WaveFactor = waveFactor;

            Bailout *= -1; // Set the bailout by getting the negative of the provided value.
            Step /= 60; // Convert our time interval into minutes.
            Step /= 24; // Convert our time interval into hours.

            // Setup our powers array.
            SetPowers();
        }

        #endregion
        #region Methods

        /// <summary>
        /// Calculates an omniwave to a point of maximum novelty known as a singularity.
        /// </summary>
        public bool Generate()
        {
            try
            {
                // Calculate the timewave given the singularity and a bailout.
                while (Singularity > 0)
                {
                    // Create a wave packet which will contain information about the comprising points of the wave.
                    WavePacket packet = new WavePacket();
                    packet.SetSingularity(Singularity);

                    // Generate points for all data sets.
                    for (int set = 0; set < _waveSets.GetLength(0); set++)
                        packet.SetData(GeneratePoint(packet.Singularity, set), set);

                    // Let the user know we've calculated a new point.
                    string data = FormatValue(packet.Singularity, _precision) + ", " +
                                  FormatValue(packet.KelleyData, _precision) + ", " +
                                  FormatValue(packet.WatkinsData, _precision) + ", " +
                                  FormatValue(packet.SheliakData, _precision) + ", " +
                                  FormatValue(packet.HuangTiData, _precision);
                    PointGenerated(data);

                    // Decrement our days left by our step to calculate the days left until maximum novelty.
                    Singularity -= Step;
                }

                // The wave has been successfully calculated.
                GenerationComplete(true);
                return true;
            }
            catch (Exception ex)
            {
                GenerationComplete(false, ex);
                return false;
            }
        }

        /// <summary>
        /// Asynchronously calculates an omniwave to a point of maximum novelty known as a singularity.
        /// </summary>
        public async Task<bool> GenerateAsync()
        {
            try
            {
                // Allow the method to run async.
                await Task.Delay(0); // TODO: Add alternative way to generate async in order to remove this.

                // Calculate the timewave given the singularity and a bailout.
                while (Singularity > 0)
                {
                    // Create a wave packet which will contain information about the comprising points of the wave.
                    WavePacket packet = new WavePacket();
                    packet.SetSingularity(Singularity);

                    // Generate points for all data sets.
                    for (int set = 0; set < _waveSets.GetLength(0); set++)
                        packet.SetData(GeneratePoint(packet.Singularity, set), set);

                    // Let the user know we've calculated a new point.
                    string data = FormatValue(packet.Singularity, _precision) + ", " +
                                  FormatValue(packet.KelleyData, _precision) + ", " +
                                  FormatValue(packet.WatkinsData, _precision) + ", " +
                                  FormatValue(packet.SheliakData, _precision) + ", " +
                                  FormatValue(packet.HuangTiData, _precision);
                    PointGenerated(data);

                    // Decrement our days left by our step to calculate the days left until maximum novelty.
                    Singularity -= Step;
                }

                // The wave has been successfully calculated.
                GenerationComplete(true);
                return true;
            }
            catch (Exception ex)
            {
                GenerationComplete(false, ex);
                return false;
            }
        }

        private void SetPowers()
        {
            // Generate powers using our wave factor.
            _powers[0] = 1;
            for (int index = 1; index < 64; index++)
                _powers[index] = (WaveFactor * _powers[index - 1]);
        }

        private double GeneratePoint(double singularity, int set)
        {
            double currentSum = 0, lastSum = 0;
            if (singularity != 0)
            {
                int index = 0;
                for (index = 0; singularity >= _powers[index]; index++)
                    currentSum += MultiplyPower(v(DividePower(singularity, index), set), index);
                index = 0;
                do
                {
                    if (++index > WaveValue + 2)
                        break;
                    lastSum = currentSum;
                    currentSum += DividePower(v(MultiplyPower(singularity, index), set), index);
                }
                while ((currentSum == 0) || (currentSum > lastSum));
            }

            currentSum = DividePower(currentSum, 3);
            return currentSum;
        }
        private double v(double y, int set)
        {
            int i = (int)(Math.IEEERemainder(y, 384));
            int j = (i + 1) % 384;
            double z = y - Math.Floor(y);
            i = Math.Abs(i);
            j = Math.Abs(j);
            return (z == 0.0 ? (_waveSets[set, i]) : ((_waveSets[set, j] - _waveSets[set, i]) * z + _waveSets[set, i]));
        }

        private double MultiplyPower(double x, int i)
        {
            x *= _powers[i];
            return (x);
        }
        private double DividePower(double x, int i)
        {
            x /= _powers[i];
            return (x);
        }

        private string FormatValue(double value, int precision)
        {
            string formatter = "{0:f" + precision + "}";
            return string.Format(formatter, value);
        }

        private static int[,] GetDataSets()
        {
            const int setAmount = 4;
            const int setSize = 384;
            int[,] dataSets = new int[setAmount, setSize];
            List<int[]> dataStack = new List<int[]>();
            var set1 = Regex.Split(Properties.Resources.DATA.FromBytes(), @"\D+").RemoveNulls();
            var set2 = Regex.Split(Properties.Resources.DATA1.FromBytes(), @"\D+").RemoveNulls();
            var set3 = Regex.Split(Properties.Resources.DATA2.FromBytes(), @"\D+").RemoveNulls();
            var set4 = Regex.Split(Properties.Resources.DATA3.FromBytes(), @"\D+").RemoveNulls();
            dataStack.Add(Array.ConvertAll(set1, int.Parse));
            dataStack.Add(Array.ConvertAll(set2, int.Parse));
            dataStack.Add(Array.ConvertAll(set3, int.Parse));
            dataStack.Add(Array.ConvertAll(set4, int.Parse));
            for (int i = 0; i < setAmount; i++)
            {
                for (int n = 0; n < setSize; n++)
                    dataSets[i, n] = dataStack[i][n];
            }
            return dataSets;
        }

        private void PointGenerated(string output)
        {
            if (OnWavePointGenerated == null) return;
            OnWavePointGenerated(this, new WavePointGeneratedEventArgs(output));
        }

        private void GenerationComplete(bool successful, Exception error = null)
        {
            if (OnWaveGenerationComplete == null) return;
            OnWaveGenerationComplete(this, new WaveGenerationCompleteEventArgs(successful, error));
        }

        #endregion
        #region Objects

        /// <summary>
        /// Contains information for a calculated point in an omniwave.
        /// </summary>
        [Serializable]
        class WavePacket
        {
            #region Variables

            /// <summary>
            /// The amount of time — in days — before the point of maximum novelty.
            /// </summary>
            public double Singularity { get; private set; }
            /// <summary>
            /// The data set of 384 points in respect to Royce Kelley.
            /// </summary>
            public double KelleyData { get; private set; }
            /// <summary>
            /// The data set of 384 points in respect to Matthew Watkins.
            /// </summary>
            public double WatkinsData { get; private set; }
            /// <summary>
            /// The data set of 384 points in respect to John Sheliak.
            /// </summary>
            public double SheliakData { get; private set; }
            /// <summary>
            /// The data set of 384 points in respect to Xuanyuan Huangdi.
            /// </summary>
            public double HuangTiData { get; private set; }

            #endregion
            #region Methods

            /// <summary>
            /// Sets the amount of time before the point of maximum novelty.
            /// </summary>
            /// <param name="days">Amount of time before the singularity.</param>
            public void SetSingularity(double days)
            {
                Singularity = days;
            }
            /// <summary>
            /// Sets the data for the calculated omniwave point.
            /// </summary>
            /// <param name="data">Point data for the currently calculating wave.</param>
            /// <param name="set">The dataset that is being used to generate point information.</param>
            public void SetData(double data, int set)
            {
                switch (set)
                {
                    case 0:
                        KelleyData = data;
                        break;
                    case 1:
                        WatkinsData = data;
                        break;
                    case 2:
                        SheliakData = data;
                        break;
                    case 3:
                        HuangTiData = data;
                        break;
                }
            }

            #endregion
        }

        #endregion
    }
    #region Events

    /// <summary>
    /// Represents the method that will handle the <see cref="WaveGenerator.OnWavePointGenerated"/> event of a <see cref="WaveGenerator"/> object.
    /// </summary>
    public delegate void WavePointGeneratedEventHandler(object sender, WavePointGeneratedEventArgs args);
    /// <summary>
    /// Contains data for the <see cref="WaveGenerator.OnWavePointGenerated"/> event.
    /// </summary>
    public class WavePointGeneratedEventArgs : EventArgs
    {
        #region Variables

        public string Output { get; private set; }

        #endregion
        #region Initialization

        public WavePointGeneratedEventArgs(string output) { Output = output; }

        #endregion
    }
    /// <summary>
    /// Represents the method that will handle the <see cref="WaveGenerator.OnWaveGenerationComplete"/> event of a <see cref="WaveGenerator"/> object.
    /// </summary>
    public delegate void WaveGenerationCompleteEventHnadler(object sender, WaveGenerationCompleteEventArgs args);
    /// <summary>
    /// Contains data for the <see cref="WaveGenerator.OnWaveGenerationComplete"/> event.
    /// </summary>
    public class WaveGenerationCompleteEventArgs : EventArgs
    {
        #region Variables

        public bool Successful { get; private set; }
        public Exception Error { get; private set; }

        #endregion
        #region Initialization

        public WaveGenerationCompleteEventArgs(bool successful, Exception error = null)
        {
            Successful = successful;
            Error = error;
        }

        #endregion
    }

    #endregion
}
