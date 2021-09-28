using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineQuee.Utils
{
    class Logger
    {
        public static void Init()
        {
            //Windows.Storage.StorageFolder StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //LogManager.Configuration.Variables["LogPath"] = StorageFolder.Path;
        }

        public static NLog.Logger CoreLogger = LogManager.GetLogger("coreLogger");
    }
}
