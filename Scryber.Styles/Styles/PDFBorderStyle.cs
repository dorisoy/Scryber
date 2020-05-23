﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;

namespace Scryber.Styles
{
    [PDFParsableComponent("Border")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Border\"")]
    public class PDFBorderStyle : PDFStyleItemBase
    {


        #region public PDFUnit CornerRadius {get;set;} + RemoveCornerRadius()

        [PDFAttribute("corner-radius")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"corner-radius\"")]
        [PDFDesignable("Border Radius", Category = "Border", Priority = 2, Type = "PDFUnit")]
        public PDFUnit CornerRadius
        {
            get
            {
                PDFUnit rad;
                if (this.TryGetValue(PDFStyleKeys.BorderCornerRadiusKey, out rad))
                    return rad;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderCornerRadiusKey, value);
            }
        }

        public void RemoveCornerRadius()
        {
            this.RemoveValue(PDFStyleKeys.BorderCornerRadiusKey);
        }

        #endregion

        #region public Sides Sides {get; set;} + RemoveSides()

        [PDFAttribute("sides")]
        [PDFJSConvertor("scryber.studio.design.convertors.bordersides_css", JSParams = "\"border\"")]
        [PDFDesignable("Sides", Ignore = false, Category = "Border", Priority = 3, Type = "FlagsSelect")]
        public Sides Sides
        {
            get
            {
                Sides side;
                if (this.TryGetValue(PDFStyleKeys.BorderSidesKey, out side))
                    return side;
                else
                    return (Sides.Left | Sides.Right | Sides.Top | Sides.Bottom);
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderSidesKey, value);
            }
        }

        public void RemoveSides()
        {
            this.RemoveValue(PDFStyleKeys.BorderSidesKey);
        }

        #endregion

        #region public LineStyle LineStyle {get;set;} + RemoveLineStyle()

        [PDFAttribute("style")]
        [PDFJSConvertor("scryber.studio.design.convertors.string_css", JSParams = "\"border-style\"")]
        public LineStyle LineStyle
        {
            get
            {
                LineStyle val;
                if (this.TryGetValue(PDFStyleKeys.BorderStyleKey,out val))
                    return val;
                else if (this.IsDefined(PDFStyleKeys.BorderDashKey) && this.Dash != PDFDash.None)
                    return LineStyle.Dash;
                else if (this.IsDefined(PDFStyleKeys.BorderColorKey))
                    return LineStyle.Solid;
                else
                    return LineStyle.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderStyleKey, value);
            }
        }

        public void RemoveLineStyle()
        {
            this.RemoveValue(PDFStyleKeys.BorderStyleKey);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        [PDFJSConvertor("scryber.studio.design.convertors.color_css", JSParams = "\"border-color\"")]
        [PDFDesignable("Border Color", Category = "Border", Priority = 1, Type = "Color")]
        public PDFColor Color
        {
            get
            {
                PDFColor col;
                if (this.TryGetValue(PDFStyleKeys.BorderColorKey, out col))
                    return col;
                else
                    return PDFColors.Transparent;
            }
            set { this.SetValue(PDFStyleKeys.BorderColorKey, value); }
        }

        public void RemoveColor()
        {
            this.RemoveValue(PDFStyleKeys.BorderColorKey);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"border-width\"")]
        [PDFDesignable("Border Width", Category = "Border", Priority = 1, Type = "PDFUnit")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.BorderWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(PDFStyleKeys.BorderWidthKey);
        }

        #endregion

        #region public PDFDash Dash {get;set;} + RemoveDash()

        [PDFAttribute("dash")]
        public PDFDash Dash
        {
            get
            {
                PDFDash dash;
                if (this.TryGetValue(PDFStyleKeys.BorderDashKey, out dash))
                    return dash;
                else
                    return PDFDash.None;
            }
            set
            {
                if (null == value)
                    this.RemoveDash();
                else
                    this.SetValue(PDFStyleKeys.BorderDashKey, value);
            }
        }

        public void RemoveDash()
        {
            this.RemoveValue(PDFStyleKeys.BorderDashKey);
        }

        #endregion

        #region public LineCaps LineCap {get;set;} + RemoveLineCap()

        [PDFAttribute("ending")]
        public LineCaps LineCap
        {
            get
            {
                LineCaps cap;
                if (this.TryGetValue(PDFStyleKeys.BorderEndingKey, out cap))
                    return (LineCaps)cap;
                else
                    return Const.DefaultLineCaps;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderEndingKey, value);
            }
        }

        public void RemoveLineCap()
        {
            this.RemoveValue(PDFStyleKeys.BorderEndingKey);
        }

        #endregion

        #region public LineJoin LineJoin {get;set;} + RemoveLineJoin()

        [PDFAttribute("join")]
        public LineJoin LineJoin
        {
            get
            {
                LineJoin join;
                if (this.TryGetValue(PDFStyleKeys.BorderJoinKey, out join))
                    return (LineJoin)join;
                else
                    return Const.DefaultLineJoin;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderJoinKey, value);
            }
        }

        public void RemoveLineJoin()
        {
            this.RemoveValue(PDFStyleKeys.BorderJoinKey);
        }

        #endregion

        #region public float Mitre {get;set;} + RemoveMitre()

        [PDFAttribute("mitre")]
        public float Mitre
        {
            get
            {
                float f;
                if (this.TryGetValue(PDFStyleKeys.BorderMitreKey, out f))
                    return f;
                else
                    return 0.0F;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderMitreKey, value);
            }
        }

        public void RemoveMitre()
        {
            this.RemoveValue(PDFStyleKeys.BorderMitreKey);
        }

        #endregion

        #region public float Opacity {get; set} + RemoveOpacity()

        /// <summary>
        /// Gets or sets the opacity of the fill
        /// </summary>
        [PDFAttribute("opacity")]
        public double Opacity
        {
            get
            {
                double found;
                if (this.TryGetValue(PDFStyleKeys.BorderOpacityKey, out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(PDFStyleKeys.BorderOpacityKey, value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(PDFStyleKeys.BorderOpacityKey);
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFBorderStyle()

        /// <summary>
        /// Creates a new Border style
        /// </summary>
        public PDFBorderStyle(): 
            base(PDFStyleKeys.BorderItemKey)
        {

        }

        #endregion

        //
        // public methods
        //

        #region public virtual PDFPen CreatePen()

        /// <summary>
        /// Creates a new pen that matches this styles values.
        /// </summary>
        /// <returns></returns>
        public virtual PDFPen CreatePen()
        {
            return this.AssertOwner().DoCreateBorderPen();
        }

        #endregion

        
    }
}