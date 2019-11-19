using System;
using System.Collections.Generic;
using System.IO;

namespace HangbankTrainer.DataAccess
{
    class CalibrationPersister
    {

        private static string GetCalibrationCsvFile()
        {
            return Path.Combine(PersistenceTools.GetHangbankDir(), "calibration.csv");
        }

        public static void Write(HangbankModel model)
        {
            var l = new List<string>
            {
                model.LinksOnbelast.ToString(),
                model.LinksBelast.ToString(),
                model.RechtsOnbelast.ToString(),
                model.RechtsBelast.ToString()
            };
            var s = string.Join(", ", l);
            File.WriteAllText(GetCalibrationCsvFile(), s);
        }

        public static void Read(HangbankModel model)
        {
            string calibrationPath = GetCalibrationCsvFile(); 
            if (!File.Exists(calibrationPath))
            {
                // Default values are used. 
                return; 
            }

            var lines = File.ReadAllLines(GetCalibrationCsvFile());
            if (lines.Length == 0)
            {
                // Default values are used. 
                return;
            }

            var split = lines[0].Trim().Split(',');
            if (split.Length == 4)
            {
                model.LinksOnbelast = int.Parse(split[0]);
                model.LinksBelast = int.Parse(split[1]);
                model.RechtsOnbelast = int.Parse(split[2]);
                model.RechtsBelast = int.Parse(split[3]);
            }
        }
    }
}

