using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Searcher.Lib
{
    public abstract class ProductFactory
    {
        #region Methods

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected ProductFactory()
        {
        }

        #endregion

        #region Factory


        /// <summary>
        /// Create and return search item object from file stream
        /// </summary>
        /// <returns> SearchItem </returns>
        /// <param name="input"> stream of input in file </param>
        internal virtual ISearchItem MakeSearchItem(FileStream input, IList<String> parameters)
        {
            return null;
        }


        /// <summary>
        /// Create and return search result object
        /// </summary>
        /// <param name="searchItem"> object with search parameters </param>
        /// <returns> ISearchResult </returns>
        internal virtual ISearchResult MakeSearchResult(ISearchItem searchItem)
        {
            return null;
        }


        /// <summary>
        /// Create and return area of searchig for
        /// </summary>
        /// <param name="path"> the path of searching for </param>
        /// <returns> ISearchArea </returns>
        internal virtual ISearchArea MakeArea(String path)
        {
            return null;
        }


        /// <summary>
        /// Create and return found item
        /// </summary>
        /// <param name="searchItem"> search item object </param>
        /// <returns> IList<String> </returns>
        internal virtual IList<ISearchProduct> MakeSearchProductList(ISearchItem searchItem, ISearchArea area)
        {
            return null;
        }


        /// <summary>
        /// Make list of sub pathes
        /// </summary>
        /// <param name="area"> area of searching for </param>
        /// <param name="searchItem"> item of searching for </param>
        /// <returns> IList<String> </returns>
        internal virtual IList<String> MakeSubPathList(ISearchArea area, ISearchItem searchItem)
        {
            return null;
        }


        /// <summary>
        /// Make xml file through output stream
        /// </summary>
        /// <param name="searchResult"> object with search results </param>
        /// <param name="output"> stream of output in file </param>
        internal virtual void MakeXml(ISearchResult searchResult, FileStream output)
        {
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Store match collection in list
        /// </summary>
        /// <param name="storeList"> stored List </param>
        /// <param name="collection"> collection of regex matches </param>
        protected void AppendList(IList<String> dataList, MatchCollection matchCollection)
        {
            for (int matchIndex = 0; matchIndex < matchCollection.Count; matchIndex++)
            {
                if (!dataList.Contains(matchCollection[matchIndex].Value))
                {
                    dataList.Add(matchCollection[matchIndex].Value);
                }
            }
        }


        /// <summary>
        /// Store url adress in result List
        /// </summary>
        /// <param name="dataList"> main list of search results </param>
        /// <param name="resultList"> list of search results </param>
        protected void AppendList(IList<String> dataList, IList<String> resultList)
        {
            for (int resultIndex = 0; resultIndex < resultList.Count; resultIndex++)
            {
                if (!dataList.Contains(resultList[resultIndex]))
                {
                    dataList.Add(resultList[resultIndex]);
                }
            }
        }

        #endregion

        #endregion
    }
}
