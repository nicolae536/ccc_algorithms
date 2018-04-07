using System;
using System.Collections.Generic;
using System.Linq;

namespace fractal_area
{
    class FractlArea
    {
        public FractlArea(string fractalDef)
        {
            string[] fract = fractalDef.Split(' ');

            string shape = fract[0];
            int length = Int32.Parse(fract[1].Split('=')[1]);
            int iterations = Int32.Parse(fract[2].Split('=')[1]);
            if (shape == "tri")
            {
                Console.WriteLine("Perimeter=" + GetTriaglePerimeter(length, iterations));
            }

            if (shape == "sq")
            {
                Console.WriteLine("Perimeter=" + GetSqaurePerimeter(length, iterations));
            }
        }

        double GetSidesOnIt(int it)
        {
            return 3 * Math.Pow(4, it);
        }

        double GetTriangleSidesSum(double length, int it)
        {
            return length / Math.Pow(3, it);
        }

        double GetTriaglePerimeter(double baseLength, int it)
        {
            return GetSidesOnIt(it) * GetTriangleSidesSum(baseLength, it);
        }

        double GetSquareSides(int it)
        {
            return 4 + Math.Pow(4, it + 1);
        }

        double GetSqareSidesSum(double length, int it)
        {
            return length / Math.Pow(3, it);
        }

        double GetSqaurePerimeter(double baseLength, int it)
        {
            double totlaP = baseLength * 4;
            baseLength = baseLength / 3;
            int totlaSides = 4;

            for (int i = 0; i < it; i++)
            {
                int newTotalSides = totlaSides;
                for (int j = 0; j < totlaSides; j++)
                {
                    totlaP = totlaP - baseLength;
                    totlaP = totlaP + 3 * baseLength;
                    // newTotalSides += 3;
                    newTotalSides += 4;
                }
                totlaSides = newTotalSides;
                baseLength = baseLength / 3;
            }
            return totlaP;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "sq Length=9 Iterations=1",
                "sq Length=243 Iterations=3",
                "sq Length=19683 Iterations=7",
                "sq Length=531441 Iterations=7",
                "sq Length=531441 Iterations=9"
            };

            foreach (string item in inputs)
            {
                new FractlArea(item);
            }

            Console.ReadLine();
        }
    }
}
