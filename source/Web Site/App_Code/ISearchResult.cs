using System;
using System.Collections.Generic;
using System.Text;

namespace Searcher.Lib
{
    public interface ISearchResult
    {
        void AddItem(ISearchProduct item);
        void AddRange(IList<ISearchProduct> list);
    }
}
