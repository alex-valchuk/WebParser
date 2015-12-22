using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Searcher.Lib
{
    public class XmlHelper
    {
        public static object Deserialize(FileStream input, Type type)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(input, settings);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
