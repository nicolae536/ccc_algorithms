using System;

namespace mars_rovers
{
    class MarsRover
    {
        private double currentAngle;
        private double tournRadius;
        private double wheelBase;


        private double xStart;
        private double yStart;
        private double tangentToXY;
        private double newDirection;

        private double xCenter;
        private double yCenter;

        private double distanceToDrive;

        public MarsRover(string carSpecs)
        {
            if (carSpecs == null || carSpecs.Length == 0)
            {
                return;
            }

            string[] carSpecsArr = carSpecs.Split(' ');
            if (carSpecsArr == null || carSpecsArr.Length < 3)
            {
                return;
            }


            wheelBase = Double.Parse(carSpecsArr[0]);
            yCenter = 0;
            ComputeTournRate(carSpecsArr[2]);
            xCenter = xCenter * tournRadius;
            xStart = 0;
            yStart = 0;

            StartCarEvents(carSpecsArr);
        }

        private void StartCarEvents(string[] carSpecsArr)
        {
            distanceToDrive = Double.Parse(carSpecsArr[1]);
            DriveDistance(distanceToDrive);
            ShowResult();
        }

        private void DriveDistance(double distanceToDrive)
        {
            if (tournRadius == 0)
            {
                DriveInStraightLine(distanceToDrive);
                return;
            }

            DriveInCircle(distanceToDrive);
        }

        private void DriveInStraightLine(double distanceToDrive)
        {
            xStart = distanceToDrive + xStart;
        }

        //private void DriveInCircle(double distanceToDrive)
        //{
        //    double alphaRadians = 2 * Math.Asin(distanceToDrive / (2 * tournRadius));
        //    // alpha as degres
        //    if (distanceToDrive > 0)
        //    {
        //        if (alphaRadians >= 0 && alphaRadians <= Math.PI / 2)
        //        {
        //            alphaRadians = Math.PI - alphaRadians;
        //        }
        //        else if (alphaRadians > Math.PI)
        //        {
        //            alphaRadians = 2 * Math.PI + (Math.PI - alphaRadians);
        //        }
        //    }
        //    else
        //    {
        //        if (alphaRadians >= 0 && alphaRadians <= Math.PI)
        //        {
        //            alphaRadians = Math.PI - alphaRadians;
        //        }
        //        else if (alphaRadians > Math.PI)
        //        {
        //            alphaRadians = 2 * Math.PI + (Math.PI - alphaRadians);
        //        }
        //    }

        //    double x = xCenter + tournRadius * Math.Cos(alphaRadians);
        //    double y = yCenter + tournRadius * Math.Sin(alphaRadians);
        //}

        private void DriveInCircle(double distanceToDrive)
        {

            if (distanceToDrive > 0)
            {
                Double distance = distanceToDrive;
                xStart = (distance) / (2 * tournRadius);
                yStart = Math.Sqrt(distance - (xStart * xStart));
            }
            else
            {
                Double distance = 2 * Math.PI * tournRadius + distanceToDrive;
                xStart = (distance) / (2 * tournRadius);
                yStart = -(Math.Sqrt(distance - (yStart * yStart)));
            }


            double tanX = xStart + 1;
            double tanY = ((Math.Pow(tournRadius, 2) - ((xStart - xCenter) * (tanX - xCenter))) / (yStart - yCenter)) + yCenter;

            double normalX = xStart;
            double normalY = tanY;


            double normalToPointLength = Math.Abs(tanY - yStart);
            double normalToTanPoint = Math.Abs(tanX - xStart);
            double pointToTanLength = Math.Sqrt(Math.Pow(normalToPointLength, 2) + Math.Pow(normalToTanPoint, 2));

            double aDegrees = GetDegrees(Math.Asin(normalToTanPoint / pointToTanLength))
        }

        private void ShowResult()
        {
            Console.Write("Mars rover: ");
            Console.Write(GetNumber(xStart));
            Console.Write(" ");
            Console.Write(GetNumber(yStart));
            Console.Write(" ");
            Console.Write(GetNumber(newDirection));
            Console.WriteLine();
        }

        private double GetNumber(double number)
        {
            return Math.Truncate(number * 100) / 100;
        }

        private void ComputeTourn(double degrees)
        {
            ComputeTournRate(degrees.ToString("N2"));
        }

        private void ComputeTournRate(string degrees)
        {
            double deg = Double.Parse(degrees);

            if (deg > 0)
            {
                xCenter = 1;
            }
            else
            {
                xCenter = -1;
            }

            currentAngle = deg;
            double radians = GetRadians(deg);
            tournRadius = radians != 0
                   ? wheelBase / Math.Sin(radians)
                   : 0;
        }

        private double GetDegrees(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        private double GetRadians(double deg)
        {
            return Math.PI * deg / 180.0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "1.00 1.00 30.00",
                "2.13 4.30 23.00",
                "1.75 3.14 -23.00",
                "2.70 45.00 -34.00",
                "4.20 -5.30 20.00",
                "9.53 8.12 0.00"
            };


            foreach (string item in inputs)
            {
                new MarsRover(item);
            }

            Console.ReadLine();
        }
    }
}
