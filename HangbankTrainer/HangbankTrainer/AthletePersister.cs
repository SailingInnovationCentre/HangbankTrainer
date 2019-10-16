using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HangbankTrainer
{
    static class AthletePersister
    {

        public static void Write(string path, IEnumerable<Athlete> athletes)
        {
            var sb = new StringBuilder(); 
            foreach (var athlete in athletes)
            {
                sb.Append(String.Join(',', new[] {
                    athlete.Name,
                    athlete.LengthCm.ToString(),
                    athlete.WeightKg.ToString(),
                    athlete.MomentMax.ToString(),
                    athlete.Moment75.ToString(),
                    athlete.Moment145degrees.ToString(),
                }));
            }
            File.WriteAllText(path, sb.ToString()); 
        }

        public static IEnumerable<Athlete> Read(string path)
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var elements = line.Split(',');
                yield return new Athlete
                {
                    Name = elements[0],
                    LengthCm = int.Parse(elements[1]),
                    WeightKg = int.Parse(elements[2]),
                    MomentMax = int.Parse(elements[3]),
                    Moment75 = int.Parse(elements[4]),
                    Moment145degrees = int.Parse(elements[5])
                };
            }
        }

    }
}
