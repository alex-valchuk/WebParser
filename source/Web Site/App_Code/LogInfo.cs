using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UrlFinder.Log
{
    public class LogInfo
    {
        #region Fields

        private String directoryPath;
        private String filePath;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor of LogInfo class
        /// </summary>
        public LogInfo(String path)
        {
            directoryPath = path;
            filePath = path + "ErrorLog.txt";
            CreateLogTargets();
        }


        /// <summary>
        /// Write time in log file
        /// </summary>
        /// <param name="time"> loged time </param>
        public void Write(DateTime time)
        {
            Write(String.Format("Start time: {0}:{1}", time.ToUniversalTime(), time.Millisecond));
        }


        /// <summary>
        /// Write text in log file
        /// </summary>
        /// <param name="text"> text for write </param>
        public void Write(String text)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("{0}\r\n", text);
                    writer.Close();
                }
                stream.Close();
            }
        }


        /// <summary>
        /// Write exception in log file
        /// </summary>
        /// <param name="e"></param>
        public void Write(Exception ex)
        {
            string text = String.Format("Message: {0} \r\nStackTrace: {1} \r\n", ex.Message, ex.StackTrace);
            this.Write(text);
        }


        /// <summary>
        /// Create directory and file if they aren't exists
        /// </summary>
        private void CreateLogTargets()
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
            }
        }

        #endregion
    }
}
