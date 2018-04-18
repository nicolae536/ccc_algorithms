using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace trafic_jam
{
    class CarSegment
    {
        public int name;
        public int start;
        public int current;
        public int end;
        public bool isEmpty = false;
        public bool wasCarOnIt = false;
        public bool hasArrived = false;
        public int time = 1;
    }

    class TraficJam
    {
        public int numberSegments;
        public int numberOfCars;
        List<int> arrivalTime = new List<int>();

        List<CarSegment> segments = new List<CarSegment>();
        List<CarSegment> segmentsCars = new List<CarSegment>();

        public TraficJam(string fileLoc)
        {
            string[] jamSpecs = File.ReadAllLines(fileLoc);
            numberSegments = Int32.Parse(jamSpecs[0]);
            numberOfCars = Int32.Parse(jamSpecs[1]);

            for (int i = 0; i < numberSegments; i++)
            {
                segments.Add(new CarSegment { isEmpty = true });
            }

            for (int i = 2; i < numberOfCars + 2; i++)
            {
                string[] jam1 = jamSpecs[i].Split(',');
                int start = Int32.Parse(jam1[0]);
                int end = Int32.Parse(jam1[1]);
                segments[start - 1].name = i - 1;
                segments[start - 1].isEmpty = false;
                segments[start - 1].start = start;
                segments[start - 1].end = end;
                segments[start - 1].current = start;
                segmentsCars.Add(segments[start - 1]);
            }

            int carsWhichArrived = 0;
            List<CarSegment> segmentsCars1 = segmentsCars.ToList();
            segmentsCars = segmentsCars.OrderByDescending(it => it.start).ToList();
            while (carsWhichArrived < numberOfCars)
            {
                foreach (CarSegment item in segmentsCars)
                {
                    if (item.current == item.end)
                    {
                        segments[item.current - 1] = new CarSegment { isEmpty = true };
                        item.time++;
                        item.hasArrived = true;
                        carsWhichArrived++;
                        continue;
                    }

                    if (item.current < item.end)
                    {
                        item.time++;
                    }

                    if (segments[item.current].isEmpty)
                    {
                        CarSegment s = segments[item.current];
                        segments[item.current] = item;
                        segments[item.current - 1] = s;
                        item.current++;
                    }
                }

                segmentsCars = segmentsCars.Where(it => !it.hasArrived).ToList();
            }

            Console.WriteLine("timings = " + string.Join(',', segmentsCars1.Select(it => it.time).ToList()));
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] files = {
                "level1_1 - Copy.in",
                "level1_1.in",
                "level1_2.in",
                "level1_3.in",
                "level1_4.in",
                "level1_5.in",
                "level1_6.in"
            };

            foreach (string item in files)
            {
                new TraficJam(item);
            }

            Console.ReadLine();
        }
    }
}
