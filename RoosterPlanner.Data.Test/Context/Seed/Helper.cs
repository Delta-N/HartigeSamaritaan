using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models.Types;

namespace RoosterPlanner.Data.Context.Seed
{
    public class Helper
    {
        public static T ReturnRandomEntity<T>(List<T> entities)
        {
            Random r = new Random();
            int rInt = r.Next(0, entities.Count);
            return entities[rInt];
        }

        public static AvailibilityType RandomType()
        {
            Array values = Enum.GetValues(typeof(AvailibilityType));
            Random r = new Random();
            return (AvailibilityType) values.GetValue(r.Next(values.Length));
        }

        public static int RandomNumberFromRange(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }

        public static Guid ConcatGuid(Guid first, Guid second)
        {
            
            string concat = first.ToString().Substring(0, 18) + second.ToString().Substring(18);
            Console.WriteLine(first);
            Console.WriteLine(second);
            Console.WriteLine(concat);
            Console.WriteLine();
            return Guid.Parse(concat);
        }
    }
}