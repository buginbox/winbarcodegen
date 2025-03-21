﻿/*
MIT License

Copyright (c) Léo Corporation

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
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/

namespace Gerayis.Enums;

/// <summary>
/// Barcodes available in Gerayis
/// </summary>
public enum Barcodes
{
	/// <summary>
	/// Code 128 barcode.
	/// </summary>
	Code128 = 0,

	/// <summary>
	/// Code 11 barcode.
	/// </summary>
	Code11 = 1,

	/// <summary>
	/// UPC-A barcode.
	/// </summary>
	UPCA = 2,

	/// <summary>
	/// MSI barcode.
	/// </summary>
	MSI = 3,

	/// <summary>
	/// ISBN barcode.
	/// </summary>
	ISBN = 4,

	/// <summary>
	/// Code39
	/// </summary>
	Code39 = 5,
    
	/// <summary>
	/// Code39 extended
	/// </summary>
	Code39Extended = 6
}
