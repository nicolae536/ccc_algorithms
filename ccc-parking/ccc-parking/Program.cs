using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ccc_parking
{
    class ParkedCar
    {
        public int carNumber;
        public ParkeingSpot spot;
    }

    class ParkeingSpot
    {
        public int spotNumber;
        public int spotPrice;
    }

    class CccGarage
    {
        int maxPlaces;
        int allCars;
        List<int> carsOrder = new List<int>();

        public CccGarage(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] garageDef = lines[0].Split(' ');
            maxPlaces = Int32.Parse(garageDef[0]);
            allCars = Int32.Parse(garageDef[1]);

            // ComputeLevel1(lines[1]);
            // ComputeLevel2(lines[1]);
            // ComputeLevel3(lines);
            // ComputeLevel4(lines);
            ComputeLevel5(lines);
        }

        private void ComputeLevel1(string carsList)
        {
            int maxCars = int.MinValue;
            int currentCars = 0;

            foreach (string carDef in carsList.Split(' '))
            {
                int car = Int32.Parse(carDef);

                if (car > 0)
                {
                    currentCars++;
                }
                else
                {
                    currentCars--;
                }

                if (currentCars > maxCars)
                {
                    maxCars = currentCars;
                }
            }

            Console.WriteLine("Mark max:" + maxCars);
        }

        private void ComputeLevel2(string carsList)
        {
            int maxCars = int.MinValue;
            int currentCars = 0;
            int maxCarsInQueue = int.MinValue;

            Queue<int> carsQueue = new Queue<int>();

            foreach (string carDef in carsList.Split(' '))
            {
                int car = Int32.Parse(carDef);

                if (car > 0 && currentCars < maxPlaces)
                {
                    currentCars++;
                }
                else if (car > 0)
                {
                    carsQueue.Enqueue(car);
                }
                else if (car < 0)
                {
                    currentCars--;

                    if (carsQueue.Count > 0)
                    {
                        carsQueue.Dequeue();
                        currentCars++;
                    }
                }

                if (currentCars > maxCars)
                {
                    maxCars = currentCars;
                }

                if (carsQueue.Count > maxCarsInQueue)
                {
                    maxCarsInQueue = carsQueue.Count;
                }
            }

            Console.WriteLine("Mark max:" + maxCars + " in queue:" + maxCarsInQueue);
        }

        private void ComputeLevel3(string[] carDefs)
        {
            Queue<int> carsQueue = new Queue<int>();
            List<ParkedCar> parking = new List<ParkedCar>();


            List<ParkeingSpot> parkingSpots = GetParkingSpots(carDefs[1]);
            int markFortune = 0;

            foreach (string carDef in carDefs[2].Trim().Split(' '))
            {
                int car = Int32.Parse(carDef);

                if (car > 0 && parkingSpots.Count > 0)
                {
                    ParkeingSpot firstFree = parkingSpots.First();
                    parkingSpots.Remove(firstFree);
                    parking.Add(new ParkedCar { carNumber = car, spot = firstFree });
                }
                else if (car > 0)
                {
                    carsQueue.Enqueue(car);
                }
                else if (car < 0)
                {
                    ParkedCar p = parking.First(it => it.carNumber == Math.Abs(car));
                    parking.Remove(p);
                    markFortune += p.spot.spotPrice;
                    parkingSpots.Add(p.spot);
                    parkingSpots = parkingSpots.OrderBy(it => it.spotNumber).ToList();

                    if (carsQueue.Count > 0)
                    {
                        int newCar = carsQueue.Dequeue();
                        ParkeingSpot firstFree = parkingSpots.First();
                        parkingSpots.Remove(firstFree);
                        parking.Add(new ParkedCar { carNumber = newCar, spot = firstFree });
                    }
                }

            }

            Console.WriteLine("Mark max:" + markFortune);
        }

        private void ComputeLevel4(string[] carDefs)
        {
            Queue<int> carsQueue = new Queue<int>();
            List<ParkedCar> parking = new List<ParkedCar>();

            List<ParkeingSpot> parkingSpots = GetParkingSpots(carDefs[1]);
            List<int> kilosTable = GetKilosTable(carDefs[2]);

            double markFortune = 0;
            foreach (string carDef in carDefs[3].Trim().Split(' '))
            {
                int car = Int32.Parse(carDef);

                if (car > 0 && parkingSpots.Count > 0)
                {
                    ParkeingSpot firstFree = parkingSpots.First();
                    parkingSpots.Remove(firstFree);
                    parking.Add(new ParkedCar { carNumber = car, spot = firstFree });
                }
                else if (car > 0)
                {
                    carsQueue.Enqueue(car);
                }
                else if (car < 0)
                {
                    ParkedCar p = parking.First(it => it.carNumber == Math.Abs(car));
                    parking.Remove(p);
                    // the kilosTable is 0 based index cars are numerotated from 1
                    double addedValue = Math.Ceiling((double)kilosTable[p.carNumber - 1] / 100);

                    markFortune += (p.spot.spotPrice * addedValue);
                    parkingSpots.Add(p.spot);
                    parkingSpots = parkingSpots.OrderBy(it => it.spotNumber).ToList();

                    if (carsQueue.Count > 0)
                    {
                        int newCar = carsQueue.Dequeue();
                        ParkeingSpot firstFree = parkingSpots.First();
                        parkingSpots.Remove(firstFree);
                        parking.Add(new ParkedCar { carNumber = newCar, spot = firstFree });
                    }
                }

            }

            Console.WriteLine("Mark max:" + markFortune);
        }

        private void ComputeLevel5(string[] carDefs)
        {
            LinkedList<int> carsQueue = new LinkedList<int>();
            List<ParkedCar> parking = new List<ParkedCar>();

            List<ParkeingSpot> parkingSpots = GetParkingSpots(carDefs[1]);
            List<int> kilosTable = GetKilosTable(carDefs[2]);

            double markFortune = 0;
            foreach (string carDef in carDefs[3].Trim().Split(' '))
            {
                int car = Int32.Parse(carDef);

                if (car > 0 && parkingSpots.Count > 0)
                {
                    ParkeingSpot firstFree = parkingSpots.First();
                    parkingSpots.Remove(firstFree);
                    parking.Add(new ParkedCar { carNumber = car, spot = firstFree });
                }
                else if (car > 0)
                {
                    carsQueue.AddLast(car);
                }
                else if (car < 0)
                {
                    ParkedCar p = parking.FirstOrDefault(it => it.carNumber == Math.Abs(car));
                    if (p != null)
                    {
                        parking.Remove(p);
                        // the kilosTable is 0 based index cars are numerotated from 1
                        double addedValue = Math.Ceiling((double)kilosTable[p.carNumber - 1] / 100);

                        markFortune += (p.spot.spotPrice * addedValue);
                        parkingSpots.Add(p.spot);
                        parkingSpots = parkingSpots.OrderBy(it => it.spotNumber).ToList();

                        if (carsQueue.Count > 0)
                        {
                            int newCar = carsQueue.First.Value;
                            carsQueue.RemoveFirst();
                            ParkeingSpot firstFree = parkingSpots.First();
                            parkingSpots.Remove(firstFree);
                            parking.Add(new ParkedCar { carNumber = newCar, spot = firstFree });
                        }
                    }
                    else if (carsQueue.Count > 0)
                    {
                        carsQueue.Remove(Math.Abs(car));
                    }
                }

            }

            Console.WriteLine("Mark max:" + markFortune);
        }

        private List<int> GetKilosTable(string carDefs)
        {
            List<int> ret = new List<int>();
            foreach (string item in carDefs.Trim().Split(' '))
            {
                ret.Add(Int32.Parse(item));
            }

            return ret;
        }

        private List<ParkeingSpot> GetParkingSpots(string carDefs)
        {
            List<ParkeingSpot> parkingSpots = new List<ParkeingSpot>();
            int i = 1;

            foreach (string item in carDefs.Trim().Split(' '))
            {
                parkingSpots.Add(new ParkeingSpot { spotPrice = Int32.Parse(item), spotNumber = i });
                i++;
            }

            return parkingSpots;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // level 1
            //new CccGarage("level1/input.1");
            //new CccGarage("level1/input.2");
            //new CccGarage("level1/input.3");

            ////level 2
            //new CccGarage("level2/input.1");
            //new CccGarage("level2/input.2");
            //new CccGarage("level2/input.3");
            //new CccGarage("level2/input.4");
            //new CccGarage("level2/input.5");

            //level 3
            //new CccGarage("level3/input.1");
            //new CccGarage("level3/input.2");
            //new CccGarage("level3/input.3");
            //new CccGarage("level3/input.4");

            // level 4
            // new CccGarage("level4/input.1");
            // new CccGarage("level4/input.2");
            // new CccGarage("level4/input.3");

            // level 5
            new CccGarage("level5/input.1");
            new CccGarage("level5/input.2");
            new CccGarage("level5/input.3");
            new CccGarage("level5/input.4");
            new CccGarage("level5/input.5");
            Console.ReadLine();
        }
    }
}
