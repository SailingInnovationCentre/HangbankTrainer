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
                sb.AppendLine(String.Join(',', new[] {
                    athlete.Name,
                    athlete.LengthCm.ToString(),
                    athlete.WeightKg.ToString(),
                    athlete.MomentMax.ToString(),
                    athlete.Moment75.ToString(),
                    athlete.Moment145degrees.ToString(),
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
                    athletes.Add(new Athlete
                    {
                        Name = elements[0],
                        LengthCm = int.Parse(elements[1]),
                        WeightKg = int.Parse(elements[2]),
                        MomentMax = int.Parse(elements[3]),
                        Moment75 = int.Parse(elements[4]),
                        Moment145degrees = int.Parse(elements[5])
                    });
                }
                catch (Exception)
                {
                    int x = 3; 
                }
            }
            return athletes; 
        }
    }
}
