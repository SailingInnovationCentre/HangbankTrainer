using System;
using System.IO;

namespace HangbankTrainer.DataAccess
{
    static class PersistenceTools
    {

        public static string GetHangbankDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "hangbank");
        }

        public static void AssertPersistenceDirPresent()
        {
            string hangbankPath = GetHangbankDir();
            if (!Directory.Exists(hangbankPath))
            {
                Directory.CreateDirectory(hangbankPath);
            }
        }
    }
}
