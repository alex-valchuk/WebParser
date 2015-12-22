using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Finder.Lib
{
    [XmlRoot("SearchResult")]
    public class ShopResult : ISearchResult, IXmlSerializable
    {
        #region Fields

        private ShopItem shopItem;
        private IList<ShopProduct> productList;

        #endregion

        #region Properties

        /// <summary>
        /// Get current search item
        /// </summary>
        internal ShopItem ShopItem
        {
            get { return shopItem; }
        }


        /// <summary>
        /// Get found item list of searching for
        /// </summary>
        public IList<ShopProduct> ProductList
        {
            get { return productList; }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Default constructor
        /// </summary>
        internal ShopResult()
        {
            this.shopItem = new ShopItem();
            this.productList = new List<ShopProduct>();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        internal ShopResult(ShopItem item)
        {
            this.shopItem = item;
            this.productList = new List<ShopProduct>();
        }


        /// <summary>
        /// Add found item in list of found items
        /// </summary>
        /// <param name="item"> founded item </param>
        public void AddItem(ISearchProduct item)
        {
            this.productList.Add((ShopProduct)item);
        }


        /// <summary>
        /// Add the list into dataList
        /// </summary>
        /// <param name="list"></param>
        public void AddRange(IList<ISearchProduct> list)
        {
            foreach (ShopProduct item in list)
            {
                if (!productList.Contains(item))
                {
                    productList.Add(item);
                }
            }
        }

        #endregion

        #region IXmlSerializable


        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"> The System.Xml.XmlReader stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("SearchResult");

            shopItem.ReadXml(reader);

            reader.ReadStartElement("SearchProductList");
            while (reader.Name == "SearchProduct")
            {
                ShopProduct product = new ShopProduct();
                product.ReadXml(reader);
                this.productList.Add(product);
            }
            if (reader.Name == "SearchProductList")
                reader.ReadEndElement();

            reader.ReadEndElement();
        }


        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer"> The System.Xml.XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("SearchItem");
            shopItem.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("SearchProductList");
            foreach (ShopProduct product in productList)
            {
                writer.WriteStartElement("SearchProduct");
                product.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
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
