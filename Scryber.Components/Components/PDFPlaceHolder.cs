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
using Scryber.Components;
using Scryber.Styles;
using Scryber.Data;
using Scryber.Native;

namespace Scryber.Components
{
    [PDFParsableComponent("PlaceHolder")]
    public class PDFPlaceHolder : PDFContainerComponent, IPDFInvisibleContainer
    {

        #region public string ParsableContents {get; set;}

        private bool _parsed = false;
        string _data;

        [PDFAttribute("contents")]
        [PDFElement()]
        public string ParsableContents
        {
            get { return _data; }
            set
            {
                if (_parsed)
                    throw new InvalidOperationException(Errors.CannotChangeTheContentsOfAPlaceHolderOnceParsed);

                _data = value;
                _parsed = false;

            }
        }

        #endregion

        #region public PDFXmlNamespaceCollection Namespaces

        private PDFXmlNamespaceCollection _nss;

        /// <summary>
        /// Gets the list of namespaces that will be used in the parsable contents so they can be identified and used.
        /// </summary>
        [PDFArray(typeof(PDFXmlNamespaceDeclaration))]
        [PDFElement("Namespaces")]
        public PDFXmlNamespaceCollection Namespaces
        {
            get
            {
                if (null == _nss)
                    _nss = new PDFXmlNamespaceCollection();
                return _nss;
            }

        }

        #endregion

        /// <summary>
        /// Gets the collection of components within this place holder
        /// </summary>
        public PDFComponentList Contents
        {
            get { return this.InnerContent; }
        }

        /// <summary>
        /// Returns true if this placeholder has parsed the contents to 
        /// </summary>
        public bool Parsed
        {
            get { return this._parsed; }
        }
        public PDFPlaceHolder()
            : this(Scryber.PDFObjectTypes.PlaceHolder)
        {
        }

        protected PDFPlaceHolder(PDFObjectType type)
            : base(type)
        {
        }

        protected override void DoLoad(PDFLoadContext context)
        {
            base.DoLoad(context);
            this.EnsureContentsParsed(this.ParsableContents, context);
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);

            this.EnsureContentsParsed(this.ParsableContents, context);
        }


        protected override void OnDataBound(PDFDataContext context)
        {
            base.OnDataBound(context);

            //If we did actually parse the contents here, then we should honour the lifcycle and DataBind ourselves.
            //If the content was previously parsed, then this would be automatic as the components were already in the collection.
            bool parsed = this.EnsureContentsParsed(this.ParsableContents, context);

            if (parsed)
                this.Contents.DataBind(context);
        }

        /// <summary>
        /// Removes any parsed (or otherwise) contents from this placeholder and 
        /// </summary>
        public void ResetContents()
        {
            this.Contents.Clear();
            this._parsed = false;

        }

        /// <summary>
        /// Makse sure that if we do have some data and it has not been parsed, them we should parse it.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns>True if we did do the actual parsing when this method was called, otherwise false.</returns>
        protected virtual bool EnsureContentsParsed(string data, PDFContextBase context)
        {
            if (!this._parsed && !string.IsNullOrEmpty(data))
            {
                IEnumerable<IPDFComponent> all = DoParseContents(data, context);

                foreach (IPDFComponent child in all)
                {
                    this.InnerContent.Add((PDFComponent)child);
                }

                //Do the init and load for these components

                PDFInitContext init = new PDFInitContext(context.Items, context.TraceLog, context.PerformanceMonitor);
                PDFLoadContext load = new PDFLoadContext(context.Items, context.TraceLog, context.PerformanceMonitor);
                foreach (IPDFComponent child in all)
                {
                    child.Init(init);
                }
                foreach (IPDFComponent child in all)
                {
                    if (child is PDFComponent)
                        ((PDFComponent)child).Load(load);
                }

                //We did do the parsing so let's return true.

                _parsed = true;
                return _parsed;
            }
            else
                return false;
        }

        protected virtual IEnumerable<IPDFComponent> DoParseContents(string data, PDFContextBase context)
        {
            System.Xml.XmlNamespaceManager mgr = GetNamespaceManager();
            Scryber.Data.PDFParsableTemplateGenerator gen = new Data.PDFParsableTemplateGenerator(data, mgr);
            IEnumerable<IPDFComponent> all = gen.Instantiate(0, this);

            return all;
        }


        private System.Xml.XmlNamespaceManager GetNamespaceManager()
        {
            System.Xml.NameTable nt = new System.Xml.NameTable();
            System.Xml.XmlNamespaceManager mgr = new System.Xml.XmlNamespaceManager(nt);
            IPDFRemoteComponent parsed = this.GetParsedParent();
            IDictionary<string, string> parsedNamespaces = null;

            //add the namespaces of the last parsed document so we can infer any declarations
            if (null != parsed)
            {
                parsedNamespaces = parsed.GetDeclaredNamespaces();
                if (null != parsedNamespaces)
                {
                    foreach (string prefix in parsedNamespaces.Keys)
                    {
                        mgr.AddNamespace(prefix, parsedNamespaces[prefix]);
                    }
                }
            }

            if (null != this._nss)
            {
                string found;
                foreach (PDFXmlNamespaceDeclaration dec in _nss)
                {
                    //makes sure this overrides any existing namespace declaration
                    if (null != parsedNamespaces && parsedNamespaces.TryGetValue(dec.Prefix, out found))
                        mgr.RemoveNamespace(dec.Prefix, found);

                    mgr.AddNamespace(dec.Prefix, dec.NamespaceURI);
                }
            }
            return mgr;
        }
    }
}
