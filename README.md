<p align="center">
    <img width="256" height="256" src="https://user-images.githubusercontent.com/40871836/43311404-f49814de-914f-11e8-877e-1142c59ccb53.png"
</p>

# Omniwave
<p align="left">
    <!-- Version -->
    <img src="https://img.shields.io/badge/version-1.0.0-brightgreen.svg">
    <!-- <img src="https://img.shields.io/appveyor/ci/gruntjs/grunt.svg"> -->
    <!-- Docs -->
    <img src="https://img.shields.io/badge/docs-not%20found-lightgrey.svg">
    <!-- License -->
    <img src="https://img.shields.io/badge/license-MIT-blue.svg">
</p>

A small yet powerful library which has the ability to calculate a Timewave up to and after the initial zero-point; the original date being concluded by both Terence McKenna himself and Peter Meyer as being December 21, 2012, however, the date has recently been theorized to take place on July 8, 2018. Omniwave can calculate a Timewave both with and without the infamous "Half Twist". The purpose of this software and the project in general is to further the research of Novelty theory and the original Timewave Zero software conjured up by Terence McKenna and others.

An in-depth explanation of how Novelty theory and the original Timewave Zero software came to be can be found in Terence and Dennis McKenna's book called *The Invisible Landscape: Mind, Hallucinogens, and the I Ching*.<br><br>
`ISBN-10: 0062506358`<br>
`ISBN-13: 978-0062506351`<br>

More detailed information about Novelty theory and Timewave Zero as well as some other interesting facts and tidbits can also be found at the following website: http://fractal-timewave.com/ which is currently maintained by Peter Meyer.

# Features
- Managed API for calculating Timewaves
- Lightweight, fast, extensible, and completely event driven
- Generate waves both synchronously and asynchronously
- Portable executable with all data sets embedded
- Uses the [Colorful.Console](http://colorfulconsole.com/) project for beautiful console output

# Upgrades
### Original Translation
Written by: John A. Phelps ([kl4yfd](https://github.com/kl4yfd))
- Can calculate Timewave *before* and *after* the zero point
- Can calculate a Window of the Timewave data
- Calculates and prints Timewave values with 16 significant digits
- Optimized code specifically for Intel/AMD x86_64 CPU's (most common today)
- Better calculation precision than original code
- 32-bit integer variables upgraded to 64-bit (uint64_t)
- 64-bit floating point variables upgraded to extended-precision 80-bit (long double)
- Switched floating-point calculations to SSE, freeing the FPU registers
- Enabled MMX optimizations (uses freed FPU registers)
- Makes use of SSE2 CPU optimizations

### This Translation
- Allows threading and asynchronous operations within a single API
- Constructs formal Timewave structures referred to as `Wave Properties` and  a `Wave Packet`
    - `Wave Properties` contain the following Timewave properties
        - Singularity (*The point at which the wave should stop being calculated*)
        - Bailout (*The point to continue calculating towards after the initial singularity*)
        - Interval (*A value to increment the wave represented in minutes*)
        - WaveFactor (*A value representing the degree or amount of novelty displayed in the wave*)
    - A `Wave Packet` contains the following Timewave properties
        - Singularity (*The amount of time - in days - before the point of maximum novelty*)
        - KelleyData (*The data set of 384 points in respect to Royce Kelley*)
        - WatkinsData (*The data set of 384 points in respect to Matthew Watkins*)
        - SheliakData (*The date set of 384 points in respect to John Sheliak*)
        - HuangTiData (*The data set of 384 points in respect to Xuanyuan Huangdi(?)*)
- Embedded all data sets within the code itself instead of referencing external files
- Improved readability of code structure (variables, method names, etc)
- Event based calculation allows for individual wave point processing
- Control over the digit length of each calculated Timewave point value
- Improved error handling and documentation of code

# Screenshot
![omni](https://user-images.githubusercontent.com/40871836/43312808-eeda1bec-9153-11e8-8a9c-525e46847c1f.jpg)

# Credits
**Icon:** `Chamestudio Pvt Ltd` <br>
https://www.iconfinder.com/chamedesign

# License
Copyright © ∞ Jason Drawdy (CloneMerge)

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
