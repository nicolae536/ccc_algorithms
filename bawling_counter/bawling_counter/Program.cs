using System;
using System.Collections.Generic;
using System.Linq;

namespace bawling_counter
{

    class Round
    {
        public int idx;
        public bool isExtraRound;
        public int firstThrow;
        public int secondThrow;

        public int roundPoints;
    }

    class BowlingCounter
    {
        int numberOfRounds;
        List<Round> roundsCol = new List<Round>();

        public BowlingCounter(string input)
        {
            string[] rowndsData = input.Split(':');
            numberOfRounds = Int32.Parse(rowndsData[0]);
            string[] bowlingData = rowndsData[1].Split(',');

            int k = 0;
            int i = 0;
            int roundsCounter = 0;
            while (i < bowlingData.Length)
            {
                int firstThrow = 0;
                int secondThrow = 0;
                bool isBonusRound = roundsCounter >= numberOfRounds;

                if (i < bowlingData.Length)
                {
                    firstThrow = Int32.Parse(bowlingData[i]);
                }
                if (i + 1 < bowlingData.Length && firstThrow != 10)
                {
                    secondThrow = Int32.Parse(bowlingData[i + 1]);
                }

                roundsCol.Add(new Round() { idx = k, firstThrow = firstThrow, secondThrow = secondThrow, isExtraRound = isBonusRound });
                if (firstThrow != 10)
                {
                    i += 2;
                }
                else
                {
                    i += 1;
                }
                roundsCounter++;
                k++;
            }

            SetupScores();

            Console.WriteLine("X =" + string.Join(',', roundsCol.Where(r => !r.isExtraRound).Select(it => it.roundPoints).ToArray()));
        }

        private void SetupScores()
        {
            Round rootRound = null;

            for (int i = 0; i < roundsCol.Count; i++)
            {
                Round r = roundsCol[i];

                if (i > 0)
                {
                    r.roundPoints = roundsCol[i - 1].roundPoints;
                }

                Round workRound = r; // rootRound == null ? r : rootRound;

                if (r.firstThrow == 10 &&
                    i + 1 < roundsCol.Count)
                {
                    workRound.roundPoints += roundsCol[i + 1].firstThrow + roundsCol[i + 1].secondThrow;

                    if (roundsCol[i + 1].firstThrow == 10 && i + 2 < roundsCol.Count)
                    {
                        workRound.roundPoints += roundsCol[i + 2].firstThrow;
                    }
                }
                else if (r.firstThrow + r.secondThrow == 10 &&
                    i + 1 < roundsCol.Count)
                {
                    workRound.roundPoints += roundsCol[i + 1].firstThrow;
                }

                workRound.roundPoints += workRound.firstThrow + workRound.secondThrow;

                if (i + 1 < roundsCol.Count &&
                    roundsCol[i + 1].isExtraRound &&
                    !r.isExtraRound)
                {
                    rootRound = r;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs =
            {
                "4:1,4,10,10,3,6",
                "3:10,10,10,3,6",
                "4:1,5,10,10,1,7"
            };

            foreach (string item in inputs)
            {
                new BowlingCounter(item);
            }

            Console.ReadLine();
        }
    }
}
