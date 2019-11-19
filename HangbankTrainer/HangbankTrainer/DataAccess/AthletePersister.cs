using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HangbankTrainer.DataAccess
{
    static class AthletePersister
    { 
        private static string GetAthletesCsvFile()
        {
            return Path.Combine(PersistenceTools.GetHangbankDir(), "athletes.csv"); 
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
                    athlete.Bandwidth.ToString()
                }));
            }
            File.WriteAllText(GetAthletesCsvFile(), sb.ToString()); 
        }

        public static List<Athlete> Read()
        {
            var athletesPath = GetAthletesCsvFile(); 
            if (!File.Exists(athletesPath))
            {
                // Default: empty list of athletes. 
                return new List<Athlete>(); 
            }
            
            var lines = File.ReadAllLines(GetAthletesCsvFile());
            var athletes = new List<Athlete>(); 
            foreach (var line in lines)
            {
                var elements = line.Split(',');
                try
                {
                    if (elements.Length == 6)
                    {
                        // Backwards compatibility
                        athletes.Add(new Athlete(
                            elements[0],
                            int.Parse(elements[1]),
                            int.Parse(elements[2]),
                            double.Parse(elements[3].Replace(',', '.')),
                            double.Parse(elements[4].Replace(',', '.')),
                            double.Parse(elements[5].Replace(',', '.')),
                            10.0));  // Add default bandwidth
                    }
                    else if (elements.Length == 7)
                    {
                        athletes.Add(new Athlete(
                            elements[0],
                            int.Parse(elements[1]),
                            int.Parse(elements[2]),
                            double.Parse(elements[3].Replace(',', '.')),
                            double.Parse(elements[4].Replace(',', '.')),
                            double.Parse(elements[5].Replace(',', '.')),
                            double.Parse(elements[6].Replace(',', '.'))));
                    }
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
