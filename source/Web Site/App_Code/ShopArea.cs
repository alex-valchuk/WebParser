using System;
using System.Collections.Generic;
using System.Text;

namespace Searcher.Lib
{
    internal class ShopArea : ISearchArea
    {
        #region Fields

        private String path;
        private String text = null;

        #endregion

        #region Properties

        /// <summary>
        /// Get the path of searching for
        /// </summary>
        internal String Path
        {
            get { return path; }
        }


        /// <summary>
        /// Get or set text for current searchArea
        /// </summary>
        internal String Text
        {
            get { return text; }
            set { text = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"> the path of searching for </param>
        internal ShopArea(String path)
        {
            this.path = path;
        }

        #endregion
    }
}
