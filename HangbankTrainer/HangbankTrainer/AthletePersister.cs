using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HangbankTrainer
{
    static class AthletePersister
    {

        public static string GetHangbankDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "hangbank"); 
        }

        public static string GetAthletesCsvFile()
        {
            return Path.Combine(GetHangbankDir(), "athletes.csv"); 
        }

        public static void AssertFilesPresent()
        {
            string hangbankPath = GetHangbankDir();
            if (!Directory.Exists(hangbankPath))
            {
                Directory.CreateDirectory(hangbankPath); 
            }

            string athletesCsvPath = GetAthletesCsvFile();
            if (!File.Exists(athletesCsvPath))
            {
                File.Create(athletesCsvPath).Close(); 
            }
        }

        public static void Write(IEnumerable<Athlete> athletes)
        {
            var sb = new StringBuilder(); 
            foreach (var athlete in athletes)
            {
                sb.AppendLine(string.Join(',', new[] {
                    athlete.Name,
                    athlete.LengthCm.ToString(),
                    athlete.WeightKg.ToString(),
                    athlete.MomentMin.ToString(),
                    athlete.MomentMid.ToString(),
                    athlete.MomentMax.ToString(),
                }));
            }
            File.WriteAllText(GetAthletesCsvFile(), sb.ToString()); 
        }

        public static List<Athlete> Read()
        {
            var lines = File.ReadAllLines(GetAthletesCsvFile());
            var athletes = new List<Athlete>(); 
            foreach (var line in lines)
            {
                var elements = line.Split(',');
                try
                {
                    athletes.Add(new Athlete(
                        elements[0],
                        int.Parse(elements[1]),
                        int.Parse(elements[2]),
                        double.Parse(elements[3].Replace(',', '.')),
                        double.Parse(elements[4].Replace(',', '.')),
                        double.Parse(elements[5].Replace(',', '.'))));
                }
                catch (Exception)
                {
                    // gulp
                }
            }
            return athletes; 
        }
    }
}
