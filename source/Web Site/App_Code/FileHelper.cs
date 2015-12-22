using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FileHelper
{
    #region Methods

    /// <summary>
    /// Get file names list without extensions by given directory path
    /// </summary>
    /// <param name="path"> the path of given directory </param>
    /// <returns> IList<String> </returns>
    public static IList<String> GetFileNamesFomPath(String path)
    {
        if(Directory.Exists(path))
        {
            try
            {
                String pattern = @"(?=\.xml\b)\.xml";
                IList<String> fileList = Directory.GetFiles(path, "*.xml");
                for (int fileIndex = 0; fileIndex < fileList.Count; fileIndex++)
                {
                    fileList[fileIndex] = Regex.Replace(new FileInfo(fileList[fileIndex]).Name, pattern, String.Empty, RegexOptions.IgnoreCase);
                }
                return fileList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        else
        {
            throw new Exception(String.Format("The path {0} is not exists."));
        }
    }

    #endregion
}
