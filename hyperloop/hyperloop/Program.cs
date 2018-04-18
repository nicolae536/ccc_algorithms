using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace hyperloop
{
    class Point
    {
        public int x;
        public int y;
        public double angle;


        public double angleXSecondary;
        public int secondaryX;
    }

    class Hyperloop
    {
        List<Point> allPointsCol = new List<Point>();
        List<Point> pointsUnderLine = new List<Point>();
        List<Point> obstaclesList = new List<Point>();
        List<Point> intersectedObsList = new List<Point>();


        //Point firstStart = new Point { x = 0, y = 0 };
        //Point secondStart = new Point { x = 0, y = 0 };
        //double aValue;
        //double bValue;

        int mapSize;
        int numberOfObstacles;

        public Hyperloop(string input)
        {
            ReadData(input);
            FindPointsUnderLine();
        }

        void ReadData(string input)
        {
            string[] dataDef = File.ReadAllLines(input);
            mapSize = Int32.Parse(dataDef[0]);
            InitObstacles(dataDef);

            //numberOfPoints = Int32.Parse(dataDef[numberOfObstacles + 1]);

            //for (int i = numberOfObstacles + 2; i < numberOfPoints + numberOfObstacles + 2; i++)
            //{
            //    string[] pointDef = dataDef[i].Split(' ');
            //    Point p = new Point { x = Int32.Parse(pointDef[0]), y = Int32.Parse(pointDef[1]) };
            //    p.angle = Math.Atan2(p.y, p.x);
            //    allPointsCol.Add(p);
            //}
        }

        void InitObstacles(string[] dataDef)
        {
            numberOfObstacles = Int32.Parse(dataDef[1]);

            for (int i = 2; i < numberOfObstacles + 2; i++)
            {

                string[] separatingLine = dataDef[i].Trim().Split(' ');
                Point obstacle = new Point
                {
                    x = Int32.Parse(separatingLine[0]),
                    y = Int32.Parse(separatingLine[2]),
                    secondaryX = Int32.Parse(separatingLine[1])

                };

                obstacle.angle = Math.Atan2(obstacle.y, obstacle.x);
                obstacle.angleXSecondary = Math.Atan2(obstacle.y, obstacle.secondaryX);

                if ((obstacle.angle > obstacle.angleXSecondary && obstacle.y > 0) ||
                    (obstacle.angle < obstacle.angleXSecondary && obstacle.y < 0))
                {
                    double d = obstacle.angle;
                    obstacle.angle = obstacle.angleXSecondary;
                    obstacle.angleXSecondary = d;
                }

                obstaclesList.Add(obstacle);
            }

            obstaclesList = obstaclesList.OrderBy(o => o.angleXSecondary).ToList();
        }

        void FindPointsUnderLine()
        {
            int xC = -mapSize;
            int yC = 0;
            int sweepPoints = mapSize * mapSize;
            List<Point> intersections = new List<Point>();

            for (int i = 0; i < mapSize * 4; i++)
            {

                if (obstaclesList.Count > 0)
                {
                    Point p = new Point { x = xC, y = yC };
                    p.angle = Math.Atan2(p.y, p.x);
                    if (IsOutsideOfObstacle(obstaclesList[0], p))
                    {
                        obstaclesList.RemoveAt(0);

                        // if ()
                    }
                }





                if (xC == -mapSize && yC > -mapSize + 1)
                {
                    yC--;
                    continue;
                }

                if (yC == -mapSize && xC < mapSize - 1)
                {
                    xC++;
                    continue;
                }

                if (xC == mapSize && yC < mapSize - 1)
                {
                    yC++;
                    continue;
                }

                if (yC == mapSize && xC > -mapSize + 1)
                {
                    xC--;
                }
            }

            //for (int x = -mapSize; x < mapSize; x++)
            //{
            //    for (int y = -mapSize; y < mapSize; y++)
            //    {
            //        Point p = new Point { x = x, y = y };
            //        p.angle = Math.Atan2(p.y, p.x);
            //        allPointsCol.Add(p);
            //    }
            //}

            //allPointsCol = allPointsCol.OrderBy(p => p.angle).ToList();

            //foreach (Point item in allPointsCol)
            //{
            //    if (IsPointReachable(item))
            //    {
            //        pointsUnderLine.Add(item);
            //    }
            //}

            //Console.WriteLine("points =" + pointsUnderLine.Count);
        }

        bool IsPointReachable(Point currentPoint)
        {

            int i = 0;
            while (i < obstaclesList.Count)
            {
                Point obs = obstaclesList[i];
                if (!IsOutsideOfObstacle(obs, currentPoint))
                {
                    intersectedObsList.Add(obs);
                    return false;
                }
                else if (intersectedObsList.Contains(obs))
                {
                    obstaclesList.Remove(obs);
                    intersectedObsList.Remove(obs);
                }
                else
                {
                    i++;
                }
            }

            return true;
        }

        bool IsOutsideOfObstacle(Point obs, Point currentPoint)
        {
            if (obs.y < 0)
            {

                if ((currentPoint.y > obs.y) ||
                    (obs.angle < currentPoint.angle || obs.angleXSecondary > currentPoint.angle) ||
                    (obs.angle >= currentPoint.angle && obs.angleXSecondary <= currentPoint.angle && currentPoint.y > obs.y))
                {
                    return true;
                }
            }
            else
            {
                if ((currentPoint.y < obs.y) ||
                    (obs.angle > currentPoint.angle || obs.angleXSecondary < currentPoint.angle) ||
                    (obs.angle <= currentPoint.angle && currentPoint.angle <= obs.angleXSecondary && currentPoint.y < obs.y))
                {
                    return true;
                }
            }

            return false;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // level1
            //new Hyperloop("level1/level1-1.txt");
            //new Hyperloop("level1/level1-2.txt");
            //new Hyperloop("level1/level1-3.txt");
            //new Hyperloop("level1/level1-4.txt");
            //new Hyperloop("level1/level1-eg.txt");

            // level2
            //new Hyperloop("level2/level2-1.txt");
            //new Hyperloop("level2/level2-2.txt");
            //new Hyperloop("level2/level2-3.txt");
            //new Hyperloop("level2/level2-4.txt");
            //new Hyperloop("level2/level2-eg.txt");

            // level3
            //new Hyperloop("level3/level3-1.txt");
            //new Hyperloop("level3/level3-2.txt");
            //new Hyperloop("level3/level3-3.txt");
            //new Hyperloop("level3/level3-4.txt");
            //new Hyperloop("level3/level3-eg.txt");

            ////level4
            //new Hyperloop("level4/level4-1.txt");
            //new Hyperloop("level4/level4-2.txt");
            //new Hyperloop("level4/level4-3.txt");
            //new Hyperloop("level4/level4-4.txt");
            //new Hyperloop("level4/level4-eg.txt");

            //level5
            //new Hyperloop("level5/level5-1.txt");
            //new Hyperloop("level5/level5-2.txt");
            //new Hyperloop("level5/level5-3.txt");
            new Hyperloop("level5/level5-4.txt");
            Console.ReadLine();
        }
    }
}
