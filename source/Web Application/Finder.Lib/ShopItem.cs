using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Finder.Lib
{
    [XmlRoot("SearchItem")]
    public class ShopItem : ISearchItem, IXmlSerializable
    {
        #region Fields

        private String path;
        private String commonPattern;
        private String title;
        private String image;
        private String description;
        private IList<String> priceList;

        #endregion

        #region Properties

        /// <summary>
        /// Get name of search item
        /// </summary>
        public String Path
        {
            get { return path; }
        }


        /// <summary>
        /// Get common pattern of searching for
        /// </summary>
        public String CommonPattern
        {
            get { return commonPattern; }
        }


        /// <summary>
        /// Get the title of product
        /// </summary>
        public String Title
        {
            get { return title; }
        }


        /// <summary>
        /// Get image of product
        /// </summary>
        public String Image
        {
            get { return image; }
        }


        /// <summary>
        /// Get description of Product
        /// </summary>
        public String Description
        {
            get { return description; }
        }


        /// <summary>
        /// Get price list of product
        /// </summary>
        public IList<String> PriceList
        {
            get { return priceList; }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        internal ShopItem()
        {
            this.priceList = new List<String>();
        }

        #endregion

        #region IXmlSerializable


        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"> The System.Xml.XmlReader stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("SearchItem");
            this.path = reader.ReadElementString("Path");
            this.commonPattern = reader.ReadElementString("CommonPattern");
            this.title = reader.ReadElementString("Title");
            this.image = reader.ReadElementString("Image");
            reader.ReadStartElement("Prices");
            while (reader.Name == "Price")
            {
                String price = reader.ReadElementString("Price");
                this.priceList.Add(price);
            }
            if (reader.Name == "Prices")
                reader.ReadEndElement();
            this.description = reader.ReadElementString("Description");
            reader.ReadEndElement();
        }


        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer"> The System.Xml.XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Path", this.path);
            writer.WriteElementString("CommonPattern", this.commonPattern);
            writer.WriteElementString("Title", this.title);
            writer.WriteElementString("Image", this.image);
            writer.WriteStartElement("Prices");
            foreach (String price in priceList)
            {
                writer.WriteElementString("Price", price);
            }
            writer.WriteEndElement();
            writer.WriteElementString("Description", this.description);
        }


        /// <summary>
        /// This method is reserved and should not be used.
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        #endregion

        #region Helpers

        internal void ReplaceSearchText(String alternative)
        {
            this.commonPattern = this.commonPattern.Replace("@SearchText@", alternative);
            this.title = this.title.Replace("@SearchText@", alternative);
            this.image = this.image.Replace("@SearchText@", alternative);
            this.description = this.description.Replace("@SearchText@", alternative);
            for (int priceIndex = 0; priceIndex < priceList.Count; priceIndex++)
            {
                priceList[priceIndex] = priceList[priceIndex].Replace("@SearchText@", alternative);
            }
        }


        /// <summary>
        /// Get Regex object by current node name
        /// </summary>
        /// <param name="name"> name of xml node </param>
        /// <param name="reader"> reader </param>
        /// <returns> Regex </returns>
        private KeyValuePair<String, String> GetRegexByName(String name, XmlReader reader)
        {
            reader.ReadStartElement(name);
            String pattern = reader.ReadElementString("Pattern");
            String options = reader.ReadElementString("Options");
            reader.ReadEndElement();
            return new KeyValuePair<String, String>(pattern, options);
        }

        #endregion
    }
}
