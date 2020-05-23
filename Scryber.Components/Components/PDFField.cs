﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Components
{
    [PDFParsableComponent("Field")]
    [PDFJSConvertor("scryber.studio.design.convertors.field")]
    public class PDFField : PDFSpanBase
    {
        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "General", Priority = 3)]
        public string Value { get; set; }


        [PDFAttribute("format")]
        [PDFDesignable("Format", Category = "General", Priority = 4, Type = "AnyFormat")]
        public string Format { get; set; }

        public PDFField() : this(PDFObjectTypes.Field)
        {

        }

        public PDFField(PDFObjectType type) : base(type)
        {

        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            this.Contents.Clear();

            string val = this.Value;
            if (!string.IsNullOrEmpty(val))
            {
                if (!string.IsNullOrEmpty(this.Format))
                {
                    double d;
                    DateTime dt;
                    if (double.TryParse(val, out d))
                        val = d.ToString(this.Format);
                    else if (DateTime.TryParse(val, out dt))
                        val = dt.ToString(this.Format);
                }
            }
            this.Contents.Add(new PDFTextLiteral(val));

            base.OnPreLayout(context);
        }
    }
}