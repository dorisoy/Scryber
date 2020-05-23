﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace Scryber.Data
{
    /// <summary>
    /// Encapsulates a blob of binary data that is converted to and from Base64. 
    /// It also supports parsing from a string from the PDFParser and binder
    /// </summary>
    [PDFParsableValue()]
    public class PDFBase64Data
    {
        /// <summary>
        /// Gets the binary raw data associated with this instance
        /// </summary>
        public byte[] Raw { get; private set; }

        /// <summary>
        /// Gets the base64 encoded data associated with this instance
        /// </summary>
        public string Base64 { get; private set; }

        /// <summary>
        /// Creates a new instance with the specified string data
        /// </summary>
        /// <param name="data"></param>
        public PDFBase64Data(string data)
        {
            this.Base64 = data;
            this.Raw = System.Convert.FromBase64String(data);
        }

        /// <summary>
        /// Creates a new instance with the specified binary data
        /// </summary>
        /// <param name="data"></param>
        public PDFBase64Data(byte[] data)
        {
            this.Raw = data;
            this.Base64 = System.Convert.ToBase64String(data);
        }

        public static PDFBase64Data Parse(string data)
        {
            PDFBase64Data imgdata = new PDFBase64Data(data);
            return imgdata;
        }
    }
}
