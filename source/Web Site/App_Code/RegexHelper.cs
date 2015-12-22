using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Searcher.Lib
{
    public abstract class RegexHelper
    {
        #region Methods

        /// <summary>
        /// Set for all html atributes 'href' and 'src' full http address
        /// </summary>
        /// <param name="text"> text for replacing </param>
        /// <param name="address"> http address </param>
        /// <returns> String </returns>
        internal static String SetFullHttpAddress(String text, String address)
        {
            address = GetShortAddress(address);
            String fullAddress = String.Format(@"""http://{0}/", address);
            String replacementPattern = String.Format(@"(?<=(href|src|url)\s*(=|\()\s*)[""']?(http://)?(www\.)?({0})?/?", address);
            Regex replacementRegex = new Regex(replacementPattern, RegexOptions.IgnoreCase);
            return replacementRegex.Replace(text, fullAddress);
        }


        /// <summary>
        /// Find all sub addresses for given http address in text
        /// </summary>
        /// <param name="path"> path of searching for </param>
        /// <param name="text"> text of searching for </param>
        /// <returns> IList<String> </returns>
        internal static IList<String> GetSubAddressList(String path, String text)
        {
            String address = GetShortAddress(path);
            IList<String> linkList = FindAllLinks(text, address);
            IList<String> addressList = GetAddressesFromLinks(linkList, address);
            return addressList;
        }


        /// <summary>
        /// Find all links in the page
        /// </summary>
        /// <param name="text"> text of searching for </param>
        /// <param name="address"> http address </param>
        /// <returns> IList<String> </returns>
        private static IList<String> FindAllLinks(String text, String address)
        {
            IList<String> linkList = new List<String>();
            String linkPattern = String.Format(@"<a .*href\s*=\s*[""']?http://{0}/.*\s*(?<=</a *>)", address);
            Regex regex = new Regex(linkPattern, RegexOptions.IgnoreCase);
            MatchCollection collection = regex.Matches(text);
            AppendList(linkList, collection);
            return linkList;
        }


        /// <summary>
        /// Mathes all http addresses from link list
        /// </summary>
        /// <param name="linkList"> List of links </param>
        /// <returns></returns>
        private static IList<String> GetAddressesFromLinks(IList<String> linkList, String address)
        {
            IList<String> addressList = new List<String>();
            String pattern = String.Format(@"(?<=href\s*=\s*[""']?)http://(www\.)?{0}[-A-Za-z0-9/_.:@&?=+%]*", address);

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            for (int linkIndex = 0; linkIndex < linkList.Count; linkIndex++)
            {
                MatchCollection collection = regex.Matches(linkList[linkIndex]);
                AppendList(addressList, collection);
            }
            return addressList;
        }


        /// <summary>
        /// Remove short form for given http address
        /// </summary>
        /// <param name="address"> http address </param>
        /// <returns> String </returns>
        public static String GetShortAddress(String address)
        {
            String[] patterns = new String[] { @"(http://)?(www\.)?", @"(?<=/*)/.*" };
            for (int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
            {
                address = Regex.Replace(address, patterns[patternIndex], "");
            }
            return address;
        }


        /// <summary>
        /// Store match collection in list
        /// </summary>
        /// <param name="storeList"> stored List </param>
        /// <param name="collection"> collection of regex matches </param>
        protected static void AppendList(IList<String> storeList, MatchCollection matchCollection)
        {
            for (int matchIndex = 0; matchIndex < matchCollection.Count; matchIndex++)
            {
                if (!storeList.Contains(matchCollection[matchIndex].Value))
                {
                    storeList.Add(matchCollection[matchIndex].Value);
                }
            }
        }


        /// <summary>
        /// Get alternative regex pattern from list
        /// </summary>
        /// <param name="parameters"> list of parameters </param>
        /// <returns> String </returns>
        internal static String GetAlternativePatternFromList(IList<String> parameters)
        {
            String pattern = "";
            if (parameters != null && parameters.Count > 0)
            {
                pattern = "(" + parameters[0];
                for (int paramIndex = 1; paramIndex < parameters.Count; paramIndex++)
                {
                    pattern += String.Format("|{0}", parameters[paramIndex]);
                }
                pattern += ")";
            }
            return pattern;
        }


        /// <summary>
        /// Replace pattern data from given text
        /// </summary>
        /// <param name="text"> given text </param>
        /// <param name="patternList"> patterns for replacing </param>
        /// <returns> String </returns>
        internal static String ReplacePatternFromText(String text, IList<String> patternList)
        {
            String altPattern = RegexHelper.GetAlternativePatternFromList(patternList);
            return Regex.Replace(text, altPattern, "", RegexOptions.IgnoreCase);
        }

        #endregion
    }
}
