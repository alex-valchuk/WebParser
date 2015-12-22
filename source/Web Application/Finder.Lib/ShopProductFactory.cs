using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;

namespace Finder.Lib
{
    public class ShopProductFactory : ProductFactory
    {
        #region Fields

        private IList<String> checkedAddressList;

        #endregion

        #region Methods

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopProductFactory()
        {
            this.checkedAddressList = new List<String>();
        }

        #endregion

        #region Factory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns> ShopItem </returns>
        internal override ISearchItem MakeSearchItem(FileStream input, IList<String> parameters)
        {
            ShopItem item = (ShopItem)XmlHelper.Deserialize(input, typeof(ShopItem));
            String altParams = RegexHelper.GetAlternativePatternFromList(parameters);
            item.ReplaceSearchText(altParams);
            return item;
        }


        /// <summary>
        /// Create and return search result object
        /// </summary>
        /// <param name="searchItem"> object with search parameters </param>
        /// <returns> ISearchResult </returns>
        internal override ISearchResult MakeSearchResult(ISearchItem searchItem)
        {
            return new ShopResult((ShopItem)searchItem);
        }


        /// <summary>
        /// Create and return area of searchig for
        /// </summary>
        /// <param name="path"> the path of searching for </param>
        /// <returns> HtmlSearchArea </returns>
        internal override ISearchArea MakeArea(String address)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                this.ProcessRequest(request);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                String text = this.ProcessResponse(response);

                if (!String.IsNullOrEmpty(text))
                {
                    ShopArea searchArea = new ShopArea(address);
                    searchArea.Text = RegexHelper.SetFullHttpAddress(text, address);
                    return searchArea;
                }
            }
            catch(Exception e)
            {
                if (e is WebException)
                {
                    return null;
                }
                throw e;
            }
            return null;
        }


        /// <summary>
        /// Create and return found item
        /// </summary>
        /// <param name="searchItem"> search item object </param>
        /// <param name="searchArea"> search area object </param>
        /// <returns> IList<String> </returns>
        internal override IList<ISearchProduct> MakeSearchProductList(ISearchItem searchItem, ISearchArea searchArea)
        {
            ShopItem item = searchItem as ShopItem;
            ShopArea area = searchArea as ShopArea;

            Regex commonRegex = new Regex(item.CommonPattern, RegexOptions.IgnoreCase);
            MatchCollection commonCollection = commonRegex.Matches(area.Text);

            IList<ISearchProduct> shopProductList = new List<ISearchProduct>();
            foreach (Match match in commonCollection)
            {
                ShopProduct product = this.GetShopProduct(item, match.Value);
                shopProductList.Add(product);
            }
            return shopProductList;
        }


        /// <summary>
        /// Make list of sub pathes
        /// </summary>
        /// <returns> IList<String> </returns>
        internal override IList<String> MakeSubPathList(ISearchArea area)
        {
            return RegexHelper.GetSubAddressList((ShopArea)area);
        }


        /// <summary>
        /// Make xml file through output stream
        /// </summary>
        /// <param name="searchResult"> object with search results </param>
        internal override void MakeXml(ISearchResult searchResult, FileStream output)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ShopResult));
            serializer.Serialize(output, searchResult);
        }


        /// <summary>
        /// Check serch items
        /// </summary>
        /// <param name="searchItems"></param>
        /// <returns> true or false </returns>
        internal override bool IsValidSearchItems(IList<ShopItem> searchItems)
        {
            foreach (ShopItem item in searchItems)
            {
                //if (String.IsNullOrEmpty(item.Regex.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Process web request for current url address
        /// </summary>
        /// <param name="request"> current web request object </param>
        private void ProcessRequest(WebRequest request)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream(), Encoding.Default))
            {
                streamWriter.Write(String.Empty);
            }
        }


        /// <summary>
        /// Process web response and get List of similar url addresses
        /// </summary>
        /// <param name="response"> current web response object </param>
        private String ProcessResponse(WebResponse response)
        {
            if (!checkedAddressList.Contains(response.ResponseUri.AbsoluteUri))
            {
                checkedAddressList.Add(response.ResponseUri.AbsoluteUri);
                using (TextReader textReader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    return textReader.ReadToEnd();
                }
            }
            return null;
        }


        /// <summary>
        /// Create ShopProduct object by ShopItem from text
        /// </summary>
        /// <returns></returns>
        private ShopProduct GetShopProduct(ShopItem item, String text)
        {
            ShopProduct product = new ShopProduct();
            product.Title = Regex.Match(text, item.Title, RegexOptions.IgnoreCase).Value;
            product.Image = Regex.Match(text, item.Image, RegexOptions.IgnoreCase).Value;
            product.Description = Regex.Match(text, item.Description, RegexOptions.IgnoreCase).Value;
            String altPrice = RegexHelper.GetAlternativePatternFromList(item.PriceList);
            MatchCollection collection = Regex.Matches(text, altPrice, RegexOptions.IgnoreCase);
            AppendList(product.PriceList, collection);
            return product;
        }


        #endregion

        #endregion
    }
}
