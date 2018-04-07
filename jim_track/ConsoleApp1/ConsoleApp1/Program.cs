using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{

    class Point
    {
        public int x;
        public int y;
    }

    class JimWalker
    {
        private int distance;

        private string previousTourn;
        private string activeCoord;
        private ArrayList points;
        private Dictionary<int, ArrayList> pointsXDic;
        private Dictionary<int, ArrayList> pointsYDic;
        private Dictionary<int, int> pointsAdded;

        private int xMax;
        private int yMax;
        private int xMin;
        private int yMin;

        private int x;
        private int y;

        private float polyArea;
        private int squareArea;
        private int pockets;
        private int addingConstX;
        private int addingConstY;

        public void LogDistance()
        {
            Console.Write("Jim walked: ");
            Console.Write(this.distance);
            Console.Write(" ");
            Console.Write(this.squareArea);
            Console.Write(" ");
            Console.Write(this.polyArea);
            Console.Write(" ");
            Console.WriteLine(this.pockets);
        }

        private void Reset()
        {
            this.points = new ArrayList();
            this.distance = 0;
            this.previousTourn = "";
            this.activeCoord = "X";
            this.xMax = int.MinValue;
            this.xMin = int.MaxValue;
            this.yMax = int.MinValue;
            this.yMin = int.MaxValue;
            this.addingConstX = 1;
            this.polyArea = 0;
            this.squareArea = 0;
            this.addingConstY = 1;
            this.x = 0;
            this.y = 0;
        }

        public void StartJourney(string journey)
        {
            this.Reset();

            if (journey == null || journey.Length == 0)
            {
                return;
            }

            string[] allDirections = journey.Split(' ');

            if (allDirections == null || allDirections.Length < 3)
            {
                return;
            }

            for (int i = 1; i < allDirections.Length; i += 2)
            {
                if (allDirections[i] != null && allDirections[i + 1] != null)
                {
                    this.MoveNext(allDirections[i], allDirections[i + 1]);
                }
            }

            this.SetPolyArea();
            this.SetSqareArea();
            this.ComputePockets();
        }

        private void SetPolyArea()
        {
            int area = 0;
            int j = this.points.Count - 1;

            for (int i = 0; i < this.points.Count; i++)
            {
                Point currentI = (Point)this.points[i];
                Point currentJ = (Point)this.points[j];
                area = area + (currentJ.x + currentI.x) * (currentJ.y - currentI.y);
                j = i;  //j is previous vertex to i
            }
            this.polyArea = Math.Abs(area / 2);
        }

        private void SetSqareArea()
        {
            foreach (Point p in this.points)
            {

                if (p.x > this.xMax)
                {
                    this.xMax = p.x;
                }
                if (p.y > this.yMax)
                {
                    this.yMax = p.y;
                }
                if (p.x < this.xMin)
                {
                    this.xMin = p.x;
                }
                if (p.y < this.yMin)
                {
                    this.yMin = p.y;
                }
            }
            this.squareArea = Math.Abs((Math.Abs(this.xMax) + Math.Abs(this.xMin))) *
                Math.Abs((Math.Abs(this.yMax) + Math.Abs(this.yMin)));
        }

        private void ComputePockets()
        {
            pointsXDic = new Dictionary<int, ArrayList>();
            foreach (Point item in this.points)
            {
                if (!this.pointsXDic.ContainsKey(item.x))
                {
                    this.pointsXDic.Add(item.x, new ArrayList { item.y });
                }
                else
                {
                    ArrayList data = this.pointsXDic.GetValueOrDefault(item.x);
                    if (data != null)
                    {
                        data.Add(item.y);
                        data.Sort();
                    }
                }
            }
            pointsYDic = new Dictionary<int, ArrayList>();
            foreach (Point item in this.points)
            {
                if (!this.pointsYDic.ContainsKey(item.y))
                {
                    this.pointsYDic.Add(item.y, new ArrayList { item.x });
                }
                else
                {
                    ArrayList data = this.pointsYDic.GetValueOrDefault(item.y);
                    if (data != null)
                    {
                        data.Add(item.x);
                        data.Sort();
                    }
                }
            }
            this.pointsAdded = new Dictionary<int, int>();
            // pointsXDic = (Dictionary<int, ArrayList>)pointsXDic.OrderBy(it => it.Key);


            for (int i = this.xMin; i < this.xMax; i++)
            {
                for (int j = this.yMin; j < this.yMax; j++)
                {
                    if (
                        // !this.pointsAdded.ContainsKey(i) &&
                        // this.pointsAdded.GetValueOrDefault(i) != j &&
                            (
                            this.IsJInPocket(i, j) ||
                            this.IsIInPocket(i, j)
                            )
                        )
                    {
                        this.pockets += 1;
                    }
                }
            }
        }

        private bool IsJInPocket(int i, int j)
        {
            //foreach (Point p in this.points)
            //{

            //}

            ArrayList xNeighboars = pointsXDic.GetValueOrDefault(i);

            if (xNeighboars != null && xNeighboars.Count > 2)
            {
                // we have x contant and there are the sides
                // |     |             |          |       |
                for (int yN = 1; yN < xNeighboars.Count - 2; yN += 2)
                {
                    int minSide = (int)xNeighboars[yN];
                    int maxSide = (int)xNeighboars[yN + 1];
                    if (minSide <= j && maxSide > j && (maxSide - minSide > 1))
                    {
                        // this.pointsAdded.Add(i, j);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsIInPocket(int i, int j)
        {


            ArrayList yNeighboars = pointsYDic.GetValueOrDefault(j);
            //   _
            //   _
            //
            //   _
            //   _
            if (yNeighboars != null && yNeighboars.Count > 2)
            {
                // we have x contant and there are the sides
                // |     |             |          |       |
                for (int yN = 1; yN < yNeighboars.Count - 2; yN += 2)
                {
                    int minSide = (int)yNeighboars[yN];
                    int maxSide = (int)yNeighboars[yN + 1];
                    if (minSide <= i && maxSide > i && (maxSide - minSide > 1))
                    {
                        // this.pointsAdded.Add(i, j);
                        return true;
                    }
                }
            }

            return false;
        }

        private void MoveNext(string directions, string repeat)
        {
            if (directions.Length == 0 || repeat.Length == 0)
            {
                return;
            }

            int numberOfRepeats = Int32.Parse(repeat);

            for (int i = 0; i < numberOfRepeats; i++)
            {
                int movesSum = this.GetMovesLength(directions);
                this.distance += movesSum;
            }
        }

        private void AddNewPoint()
        {
            Point newPoint = new Point
            {
                x = x,
                y = y
            };

            if (this.points.Count > 0)
            {
                Point p = (Point)this.points[this.points.Count - 1];
                if (p.x != newPoint.x || p.y != newPoint.y)
                {
                    this.points.Add(newPoint);
                }
            }
            else
            {
                this.points.Add(newPoint);
            }
        }

        private int GetMovesLength(string movesDirections)
        {
            if (movesDirections == null || movesDirections.Length == 0)
            {
                return 0;
            }

            int result = 0;

            for (int i = 0; i < movesDirections.Length; i++)
            {
                string move = movesDirections[i].ToString();

                if (move.Equals("F") || move.Equals("f"))
                {
                    result += 1;
                    if (this.activeCoord == "X")
                    {
                        this.x = this.x + this.addingConstX;
                    }
                    else
                    {
                        this.y = this.y + this.addingConstY;
                    }
                }
                else
                {


                    if (this.activeCoord == "X")
                    {
                        this.activeCoord = "Y";
                    }
                    else
                    {
                        this.activeCoord = "X";
                    }

                    if (this.previousTourn.Equals(move))
                    {
                        if (this.activeCoord == "X")
                        {
                            this.addingConstX = this.addingConstX == 1 ? -1 : 1;

                        }
                        else
                        {
                            this.addingConstY = this.addingConstY == 1 ? -1 : 1;
                        }
                    }

                    if (this.previousTourn.Equals(""))
                    {
                        if (this.activeCoord == "X")
                        {
                            this.addingConstX = move.Equals("R") ? -1 : 1;
                        }
                        else
                        {
                            this.addingConstY = move.Equals("L") ? -1 : 1;
                        }
                    }

                    this.previousTourn = move.ToString();
                }

                this.AddNewPoint();
            }

            // add last point to be sure that we check this also
            this.AddNewPoint();

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string[] journeys = {
                "1 FFFR 4",
                "9 F 6 R 1 F 4 RFF 2 LFF 1 LFFFR 1 F 2 R 1 F 5",
                "14 L 1 FR 1 FFFFFL 1 FFFFL 1 F 12 L 1 F 12 L 1 F 12 L 1 FFFFL 1 FFFFFFFFR 1 FFFR 1 FFFL 1",
                "32 FFRFLFLFFRFRFLFF 3 R 1 FFLFRFRFLFFF 3 R 1 FFFFFF 3 L 1 FFFRFLFLFRFF 2 R 1 FFFRFLFLFRFF 3 R 1 FFFFFF 1 L 1 FFRFLFLFFRFRFLFF 3 R 1 FFLFRFRFFLFLFRFF 2 L 1 FFLFRFRFFLFLFRFF 3 R 1 FFRFLFLFFRFRFLFF 2 R 1 FFRFLFLFFRFRFLFF 2 L 1 FFFFFF 3 R 1 FFFRFLFLFRFF 5 R 1 FFLFRFRFLFFF 1 L 1 FFLFRFRFFLFLFRFF 2 R 1 FFRFLFLFFRFRFLFF 2 L 1",
                "10 FFLFRFRFFLFLFRFF 5 L 1 FFFRFLFLFRFF 4 L 1 FFLFRFRFFLFLFRFF 8 L 1 FFLFRFRFFLFLFRFF 4 L 1 FFFFFF 3 R 1"
            };

            foreach (string journey in journeys)
            {
                JimWalker jim = new JimWalker();
                jim.StartJourney(journey);
                jim.LogDistance();
            }
            Console.ReadKey();
        }
    }
}
