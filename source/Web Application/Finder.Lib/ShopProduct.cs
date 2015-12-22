using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Finder.Lib
{
    [XmlRoot("SearchProduct")]
    public class ShopProduct : ISearchProduct, IXmlSerializable
    {
        #region Fields

        private String title;
        private String image;
        private String description;
        private IList<String> priceList;

        #endregion

        #region Properties


        /// <summary>
        /// Get or set the title of product
        /// </summary>
        public String Title
        {
            get { return title; }
            set { title = value; }
        }


        /// <summary>
        /// Get or set image of product
        /// </summary>
        public String Image
        {
            get { return image; }
            set { image = value; }
        }


        /// <summary>
        /// Get or set description of Product
        /// </summary>
        public String Description
        {
            get { return description; }
            set { description = value; }
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

        public ShopProduct()
        {
            priceList = new List<String>();
        }

        #endregion

        #region IXmlSerializable


        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"> The System.Xml.XmlReader stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("SearchProduct");
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
    }
}
