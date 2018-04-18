using System;
using System.Collections.Generic;
using System.Linq;

namespace genetic_drift
{
    class Pair
    {
        public int xGene;
        public int yGent;
        public int i;
        public int j;
    }

    class GeneticDrift
    {
        List<int> allPairs = new List<int>();

        public GeneticDrift(string input)
        {
            string[] data = input.Split(' ');

            int nrOfNodes = Int32.Parse(data[0]);

            for (int i = 1; i < nrOfNodes + 1; i++)
            {
                allPairs.Add(Int32.Parse(data[i]));
            }

            Console.WriteLine("X =" + OrderPermutation(allPairs));

            //int invX = Int32.Parse(data[nrOfNodes + 1]);
            //int invI = Int32.Parse(data[nrOfNodes + 2]);
            //int invY = Int32.Parse(data[nrOfNodes + 3]);
            //int invJ = Int32.Parse(data[nrOfNodes + 4]);
            //InverPair(invX, invY, invI, invJ);
        }

        List<Pair> FindOrientedPairs(List<int> pairsList)
        {
            List<Pair> orientedPairs = new List<Pair>();
            for (int i = 0; i < pairsList.Count - 1; i++)
            {
                for (int j = i + 1; j < pairsList.Count; j++)
                {
                    if (IsPair(pairsList[i], pairsList[j]))
                    {
                        Pair p = new Pair
                        {
                            xGene = pairsList[i],
                            yGent = pairsList[j],
                            i = i,
                            j = j
                        };
                        orientedPairs.Add(p);
                    }
                }
            }
            return orientedPairs.OrderBy(it => it.xGene).ToList();
        }

        void InverPair(List<int> pairsList, int invX, int invY, int invI, int invJ)
        {
            int i = invX + invY == 1 ? invI : invI + 1;
            int j = invX + invY == 1 ? invJ - 1 : invJ;
            int start = i;
            int end = j;

            while (i < j)
            {
                int p = pairsList[i];
                pairsList[i] = pairsList[j] * -1;
                pairsList[j] = p * -1;
                i++;
                j--;
            }

            if (Math.Abs(end - start + 1) % 2 != 0)
            {
                pairsList[i] = pairsList[i] * -1;
            }

            //Console.WriteLine("X = " + string.Join(' ', pairsList));
            //Console.WriteLine("S = " + FindOrientedPairs(pairsList).Count);
        }

        int OrderPermutation(List<int> pairsList)
        {
            List<Pair> allPairsList = FindOrientedPairs(pairsList);
            int pairsCount = allPairsList.Count;
            int inversions = 0;

            while (pairsCount > 0)
            {
                pairsList = InvertMaxPair(allPairsList, pairsList);
                allPairsList = FindOrientedPairs(pairsList);
                inversions++;
                pairsCount = allPairsList.Count;
            }

            return inversions;
        }

        List<int> InvertMaxPair(List<Pair> allPairsList, List<int> permutation)
        {
            int maxOrientedScore = int.MinValue;
            List<int> retVal = permutation;

            foreach (Pair p in allPairsList)
            {

                List<int> newPairsList = CloneList(permutation);
                InverPair(newPairsList, p.xGene, p.yGent, p.i, p.j);
                List<Pair> newP = FindOrientedPairs(newPairsList);

                if (newP.Count > maxOrientedScore)
                {
                    retVal = newPairsList;
                }
            }

            return retVal;
        }

        bool IsPair(int x, int y)
        {
            int sum = Math.Abs(Math.Abs(x) - Math.Abs(y));

            if (sum == 1 && ((x < 0 && y >= 0) || (x >= 0 && y < 0)))
            {
                return true;
            }

            return false;
        }

        List<int> CloneList(List<int> arr)
        {
            List<int> newPairsList = new List<int>();

            foreach (int p in arr)
            {
                newPairsList.Add(p);
            }

            return newPairsList;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] pairs = {
                "8 0 3 1 6 5 -2 4 7",
                "193 125 133 134 135 136 -52 -51 -50 -49 -48 -47 -46 -45 66 67 68 69 70 71 -38 -37 -36 -35 -34 -33 -32 -31 -30 -29 -132 -131 -130 -193 -192 -191 -190 -189 -188 -187 -186 -185 -184 -183 -182 -181 -180 -179 -178 -177 -176 -175 -174 -173 -172 -171 -170 -169 -77 -76 -75 -74 -73 -72 18 19 20 21 22 23 24 25 26 27 28 -164 -163 -65 -64 -63 -62 -61 -60 -59 -58 -57 -56 -55 -54 -53 39 40 41 42 43 44 159 160 161 162 -17 -16 -15 -14 -13 -12 -11 -10 -9 -8 -7 -6 -5 -4 -3 -2 -1 -168 -167 -166 -165 126 127 128 129 86 87 88 89 90 91 92 93 94 95 96 -124 -123 -122 -121 -120 -119 -118 -117 -116 -115 -114 -113 -112 -111 -110 -109 -108 -107 -106 -105 -104 -103 -102 -101 -100 -99 -98 -97 153 154 155 156 157 158 -148 -147 -146 -145 -144 -143 -142 -141 -140 -139 -138 -137 -85 -84 -83 -82 -81 -80 -79 -78 -152 -151 -150 -149"
            };

            foreach (string item in pairs)
            {
                new GeneticDrift(item);
            }

            Console.ReadLine();
        }
    }
}
