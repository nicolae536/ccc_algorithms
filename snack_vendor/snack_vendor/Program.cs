using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace snack_vendor
{
    class Node : IComparable
    {
        public int i;
        public int j;
        public int dist = 0;

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(Node))
            {
                Node n = (Node)obj;

                if (n.i == i && n.j == j)
                {
                    return 0;
                }

                return -1;
            }

            return 1;
        }
    }

    class Distance
    {
        public Node n;
        public int dist;
    }

    class Snack
    {
        public int price;
        public int stock;
    }

    class SnackVendor
    {
        int numberOfCustomersIndex;

        int[] change = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] moneyValues = { 1, 2, 5, 10, 20, 50, 100, 200 };
        int matrixHeight;
        int matrixWidth;
        int totalEarnings = 0;

        List<List<Node>> graphMatrix = new List<List<Node>>();

        public SnackVendor(string input)
        {
            string[] specs = input.Split(' ');
            InitSnackMaching(specs);
            MoveRobotToPoint(specs);

            //Math.Max(Math.Abs(robotWidth - destWidth), Math.Abs(robotHeight - destHeight));
            //InitSnackMaching(specs);
            //ServeAllCustomers(specs);
            //ServeCustomer(splittedInput);
            //Console.WriteLine($"totlaE={Math.Max(Math.Abs(robotWidth - destWidth), Math.Abs(robotHeight - destHeight))}");
        }

        void MoveRobotToPoint(string[] specs)
        {
            int robotHeight = char.ToUpper(specs[1][0]) - 65;
            int robotWidth = Int32.Parse(specs[1].Remove(0, 1)) - 1;

            int destHeight = char.ToUpper(specs[2][0]) - 65;
            int destWidth = Int32.Parse(specs[2].Remove(0, 1)) - 1;

            int maxDist = Math.Abs(robotHeight - destHeight) + Math.Abs(robotWidth - destWidth);

            Node startNode = graphMatrix[robotHeight][robotWidth];
            Node destNode = graphMatrix[destHeight][destWidth];

            int brokenDir = Int32.Parse(specs[3]);

            IAStarHeuristic<Node> heuristicImpl = new RobotArmHeuristic(graphMatrix, matrixHeight, matrixWidth, brokenDir);
            AStarSearch<Node> aSearchAlgo = new AStarSearch<Node>(startNode, destNode, heuristicImpl);
            IEnumerable<Node> result = aSearchAlgo.FindPath();

            if (result != null)
            {
                Console.WriteLine("Path hans n=" + result.Count() + " nodes");
            }
            else
            {
                Console.WriteLine("No path found");
            }
        }

        void InitSnackMaching(string[] input)
        {
            matrixHeight = char.ToUpper(input[0][0]) - 64;
            matrixWidth = Int32.Parse(input[0].Remove(0, 1));

            int k = 1;
            int k1 = matrixHeight * matrixWidth;

            for (int i = 0; i < matrixHeight; i++)
            {
                graphMatrix.Add(new List<Node>());
                for (int j = 0; j < matrixWidth; j++)
                {
                    graphMatrix[i].Add(new Node
                    {
                        i = i,
                        j = j
                    });
                    k++;
                }
            }

            numberOfCustomersIndex = k + k1;
        }

        //void ServeAllCustomers(string[] input)
        //{
        //    int numberOfCustomers = Int32.Parse(input[numberOfCustomersIndex]);

        //    for (int i = numberOfCustomersIndex + 1; i < numberOfCustomersIndex + numberOfCustomers + 1; i++)
        //    {
        //        ServeCustomer(input[i]);
        //    }
        //}

        //void ServeCustomer(string selection)
        //{
        //    // 0 based index here
        //    int selectedLine = char.ToUpper(selection[0]) - 65;
        //    int selectedColumn = Int32.Parse(selection.Remove(0, 1)) - 1;

        //    //numberOfCoings = Int32.Parse(input[selectionIndex + 1]);
        //    Snack slectedSnack = snackMachine[selectedLine][selectedColumn];

        //    if (slectedSnack.stock == 0)
        //    {
        //        // UpdateChangeForCustomer()
        //        return;
        //    }
        //    slectedSnack.stock--;
        //    totalEarnings += slectedSnack.price;

        //    //remainindMoney = slectedSnack.price;

        //    //for (int i = selectionIndex + 2; i < numberOfCoings + selectionIndex + 2; i++)
        //    //{
        //    //    int value = Int32.Parse(input[i]);
        //    //    remainindMoney -= value;
        //    //}

        //    //if (remainindMoney < 0)
        //    //{
        //    //    totalEarnings += slectedSnack.price;
        //    //    UpdateChangeForCustomer(remainindMoney);
        //    //    // Console.WriteLine($"CHANGE {string.Join(' ', change)}");
        //    //}
        //    //else
        //    //{
        //    //    totalEarnings += slectedSnack.price - Math.Abs(remainindMoney);
        //    //    // Console.WriteLine($"MISSING {Math.Abs(remainindMoney)}");
        //    //}
        //}

        //int[] UpdateChangeForCustomer(int remainindMoney)
        //{

        //    int[] changeMoney = { 0, 0, 0, 0, 0, 0, 0, 0 };
        //    remainindMoney = Math.Abs(remainindMoney);
        //    while (remainindMoney > 0)
        //    {

        //        bool updated = false;
        //        int i = 7;

        //        while (i > -1 && !updated)
        //        {
        //            if (moneyValues[i] <= remainindMoney)
        //            {
        //                remainindMoney -= moneyValues[i];
        //                changeMoney[i]++;
        //                updated = true;
        //            }
        //            i--;
        //        }
        //    }

        //    return changeMoney;
        //}
    }

    //class NodeComparer : IComparer<Node>
    //    {
    //        Node dest;
    //    Node current;
    //    int dist;
    //    public NodeComparer(Node dest, Node current, int dist)
    //    {
    //        this.dest = dest;
    //        this.current = current;
    //        // next dist
    //        this.dist = dist + 1;
    //    }

    //    public int Compare(Node x, Node y)
    //    {
    //        int nextWidth1 = x.i + current.i;
    //        int nextHeight1 = x.j + current.j;

    //        int nextWidth2 = y.i + current.i;
    //        int nextHeight2 = y.j + current.j;

    //        int mindDest1 = Math.Max(Math.Abs(nextWidth1 - dest.i), Math.Abs(nextHeight1 - dest.j));
    //        int mindDest2 = Math.Max(Math.Abs(nextWidth2 - dest.i), Math.Abs(nextHeight2 - dest.j));

    //        if (mindDest1 < mindDest2)
    //        {
    //            return -1;
    //        }

    //        if (mindDest1 > mindDest2)
    //        {
    //            return 1;
    //        }

    //        return 0;
    //    }
    //}

    interface IAStarHeuristic<TNode>
    {
        double DistanceBetween(TNode start, TNode goal);

        IEnumerable<TNode> GetNeighboars(TNode node);
    }

    class RobotArmHeuristic : IAStarHeuristic<Node>
    {
        List<List<Node>> matrix;
        List<Node> positions;
        int height;
        int width;

        public RobotArmHeuristic(List<List<Node>> matrix, int height, int width, int brokenMove)
        {
            this.matrix = matrix;
            positions = new List<Node>();
            this.height = height;
            this.width = width;
            InitMovements(brokenMove);
        }

        public double DistanceBetween(Node start, Node goal)
        {
            return Math.Sqrt(Math.Pow(start.i - goal.i, 2) + Math.Pow(start.j - goal.j, 2));
        }

        public IEnumerable<Node> GetNeighboars(Node node)
        {
            List<Node> neighboars = new List<Node>();

            foreach (var item in positions)
            {
                int nextI = item.i + node.i;
                int nextJ = item.j + node.j;

                if (nextI < 0 ||
                        nextI >= height ||
                        nextJ < 0 ||
                        nextJ >= width)
                {
                    continue;
                }

                neighboars.Add(matrix[nextI][nextJ]);
            }

            return neighboars;
        }

        private void InitMovements(int noMove)
        {
            if (noMove != 0)
            {
                positions.Add(new Node { i = 0, j = 1 });
            }
            if (noMove != 1)
            {
                positions.Add(new Node { i = -1, j = 1 });
            }
            if (noMove != 2)
            {
                positions.Add(new Node { i = -1, j = 0 });
            }
            if (noMove != 3)
            {
                positions.Add(new Node { i = -1, j = -1 });
            }
            if (noMove != 4)
            {
                positions.Add(new Node { i = 0, j = -1 });
            }
            if (noMove != 5)
            {
                positions.Add(new Node { i = 1, j = -1 });
            }
            if (noMove != 6)
            {
                positions.Add(new Node { i = 1, j = 0 });
            }
            if (noMove != 7)
            {
                positions.Add(new Node { i = 1, j = 1 });
            }
        }
    }

    class AStarSearch<TNode> where TNode : IComparable
    {
        // The set of nodes already evaluated
        Dictionary<TNode, bool> nodesVisited;

        // The set of currently discovered nodes that are not evaluated yet.
        // Initially, only the start node is known.
        Dictionary<TNode, Int32> notEvaluatedNodes;

        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        Dictionary<TNode, TNode> nodeToParent;

        // For each node, the cost of getting from the start node to that node.
        Dictionary<TNode, double> costFromStartToNode;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        Dictionary<TNode, double> costToGoal;

        IAStarHeuristic<TNode> heuristicImpl;
        TNode start;
        TNode goal;

        public AStarSearch(TNode start, TNode goal, IAStarHeuristic<TNode> heuristicImpl)
        {
            this.start = start;
            this.goal = goal;
            this.heuristicImpl = heuristicImpl;

            InitializeSets();
        }

        private void InitializeSets()
        {
            nodesVisited = new Dictionary<TNode, bool>();
            notEvaluatedNodes = new Dictionary<TNode, int>
            {
                { start, 0 }
            };
            Dictionary<TNode, TNode> dictionary = new Dictionary<TNode, TNode>();
            nodeToParent = dictionary;

            costFromStartToNode = new Dictionary<TNode, double>
            {
                { start, 0 }
            };
            costToGoal = new Dictionary<TNode, double>
            {
                { start, heuristicImpl.DistanceBetween(start, goal) }
            };
        }

        public IEnumerable<TNode> FindPath()
        {
            while (notEvaluatedNodes.Count > 0)
            {
                TNode current = notEvaluatedNodes.Keys.First();

                if (current.CompareTo(goal) == 0)
                {
                    // TODO reconstruct_path
                    return ReconstructPath(current);
                }

                notEvaluatedNodes.Remove(current);
                nodesVisited.Add(current, true);
                foreach (TNode neighboar in heuristicImpl.GetNeighboars(current))
                {
                    if (nodesVisited.ContainsKey(neighboar))
                    {
                        continue;
                    }

                    if (!notEvaluatedNodes.ContainsKey(neighboar))
                    {
                        notEvaluatedNodes.Add(neighboar, int.MaxValue);
                    }

                    // compute heuristic cost
                    double tentativeDistance = costFromStartToNode.GetValueOrDefault(current) + heuristicImpl.DistanceBetween(current, neighboar);
                    // This is not a better path.
                    double costOrDefault = costFromStartToNode.ContainsKey(neighboar) ? costFromStartToNode.GetValueOrDefault(neighboar) : double.MaxValue;

                    if (tentativeDistance >= costOrDefault)
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!                
                    ReplaceInDictionary(nodeToParent, neighboar, current);
                    ReplaceInDictionary(costFromStartToNode, neighboar, tentativeDistance);
                    ReplaceInDictionary(costToGoal, neighboar, heuristicImpl.DistanceBetween(neighboar, goal));
                    //costToGoal = costToGoal.OrderBy((keyItem) => keyItem.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                    notEvaluatedNodes = notEvaluatedNodes.OrderBy(keyIt => costToGoal.GetValueOrDefault(keyIt.Key)).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                }
            }

            return null;
        }



        private void ReplaceInDictionary<T, K>(Dictionary<T, K> dict, T key, K value)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }

            dict.Add(key, value);
        }

        private List<TNode> ReconstructPath(TNode current)
        {
            List<TNode> path = new List<TNode> { current };

            while (nodeToParent.ContainsKey(current))
            {
                current = nodeToParent.GetValueOrDefault(current);
                path.Add(current);
            }

            return path;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "D6 D5 B2 4",
                "W18 E15 E2 4",
                "S15 R6 H4 2",
                "Z26 T2 Z25 0",
                "Z6 Z2 B5 2",
                "Z26 E5 X23 7",
                "E25 D2 B23 0",
                "Z2 A1 Z2 6",
                "Z2 A1 Z1 6"
                //"O12 3577 4269 4082 3042 2126 2174 1828 1482 1238 2290 186 4243 4170 231 3374 3400 2886 4271 4487 3326 4770 663 1598 3190 3574 3820 1816 3305 4414 445 3620 2605 1015 2735 4052 4293 2313 3494 245 3343 4427 1511 1477 3676 1831 1359 1234 1067 3763 3655 3975 3864 1073 1524 564 3916 329 3929 1802 3476 4967 572 1315 3144 3913 4533 3146 2330 4124 2138 1861 2243 4345 2334 122 323 4653 3125 464 4243 4885 664 3347 30 3875 1772 1415 4810 4813 4443 4564 3355 543 4828 183 104 262 3776 207 2118 3585 2656 2345 232 1837 620 4163 1417 1929 3287 2409 3362 1558 2585 1263 1406 35 171 732 1559 3906 214 1637 996 988 2374 3115 4812 1593 1246 1945 757 885 491 915 4177 4612 457 771 563 2871 4097 740 4330 772 4150 138 1816 2541 3335 2048 930 2328 3763 3127 1379 2150 2542 277 4676 4773 4515 747 2989 77 3804 3599 2311 573 2732 4831 1329 2854 2708 379 1136 1020 3985 3038 2108 N3 46 20 5 50 200 10 1 200 1 5 10 50 2 10 10 10 20 2 20 5 2 20 2 200 1 5 5 20 2 1 100 1 20 200 200 1 200 200 10 50 50 10 20 20 5 1 10"
            };

            foreach (string item in inputs)
            {
                new SnackVendor(item);
            }

            Console.ReadLine();
        }
    }
}
