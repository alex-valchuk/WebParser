using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Finder.Lib
{
    public class SearchProcess
    {
        #region Fields

        private FileStream inputStream;
        private FileStream outputStream;
        private IList<String> targetPathList;

        #endregion

        #region Methods

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inputStream"> stream of search item file </param>
        /// <param name="outputStream"> stream of search result file </param>
        public SearchProcess(FileStream inputStream, FileStream outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
            this.targetPathList = new List<String>();
        }

        #endregion

        #region Factory

        /// <summary>
        /// Generate list of results of searching for
        /// </summary>
        /// <param name="factory"> certain concrete factory </param>
        public ISearchResult GetSearchResult(ProductFactory factory, IList<String> parameters)
        {
            try
            {
                ISearchItem searchItem = factory.MakeSearchItem(inputStream, parameters);
                ISearchResult searchResult = factory.MakeSearchResult(searchItem);

                this.AppendTargetPathList(new List<String> { searchItem.Path });
                for (int pathIndex = 0; pathIndex < targetPathList.Count; pathIndex++)
                {
                    ISearchArea area = factory.MakeArea(targetPathList[pathIndex]);
                    if (area != null)
                    {
                        IList<ISearchProduct> searchProductList = factory.MakeSearchProductList(searchItem, area);
                        searchResult.AddRange(searchProductList);

                        IList<String> subPathList = factory.MakeSubPathList(area);
                        this.AppendTargetPathList(subPathList);
                    }
                }

                factory.MakeXml(searchResult, outputStream);
                return searchResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Helpers


        /// <summary>
        /// Append pathList 
        /// </summary>
        /// <param name="pathList"></param>
        private void AppendTargetPathList(IList<String> pathList)
        {
            foreach (String path in pathList)
            {
                if (!targetPathList.Contains(path))
                {
                    targetPathList.Add(path);
                }
            }
        }

        #endregion

        #endregion
    }
}
