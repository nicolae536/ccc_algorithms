using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace unlock_me
{

    class Block
    {
        public int id;
        public string orientation;
        public int xBottomLeft;
        public int yBottomLeft;
        public int blockLength;

        public int xBottomRight;
        public int yBottomRight;

        public int xTopRight;
        public int yTopRight;

        public int xTopLeft;
        public int yTopLeft;

        public int matrixWidth;
        public int matrixHeight;

        public Block(int id, string orientation, int xBottomLeft, int yBottomLeft, int blockLength, int matrixWidth, int matrixHeight)
        {

            this.id = id;
            this.orientation = orientation;
            this.xBottomLeft = xBottomLeft;
            this.yBottomLeft = yBottomLeft;
            this.blockLength = blockLength;

            this.matrixWidth = matrixWidth;
            this.matrixHeight = matrixHeight;

            if (orientation == "h")
            {
                xBottomRight = xBottomLeft + blockLength;
                yBottomRight = yBottomLeft;

                xTopLeft = xBottomLeft;
                yTopLeft = yBottomLeft + 1;

                xTopRight = xBottomRight;
                yTopRight = yTopLeft;
                return;
            }


            xBottomRight = xBottomLeft + 1;
            yBottomRight = yBottomLeft;

            xTopLeft = xBottomLeft;
            yTopLeft = yBottomLeft + blockLength;

            xTopRight = xTopLeft + 1;
            yTopRight = yTopLeft;
        }

        public bool Overlaps(Block b1)
        {
            if (b1 == this)
            {
                return false;
            }

            if (orientation == "h" && b1.orientation == "h")
            {
                return (
                    // x intersects
                    (xBottomRight > b1.xBottomLeft && xBottomLeft < b1.xBottomRight &&
                    // y intersects
                    (b1.yBottomLeft == yBottomLeft && b1.yBottomRight == yBottomRight && b1.yTopLeft == yTopLeft && b1.yTopRight == yTopRight)
                    ) ||
                    ((b1.yBottomLeft == yBottomLeft && b1.yBottomRight == yBottomRight && b1.yTopLeft == yTopLeft && b1.yTopRight == yTopRight) &&
                    (b1.xBottomLeft == xBottomLeft && b1.xBottomRight == xBottomRight && b1.xTopLeft == xTopLeft && b1.xTopRight == xTopRight))
                );
            }

            if (orientation == "v" && b1.orientation == "v")
            {
                return (
                    // y intersects
                    (yTopLeft > b1.yBottomLeft && yBottomLeft < b1.yTopLeft &&
                    // x intersects
                    (b1.xBottomLeft == xBottomLeft && b1.xBottomRight == xBottomRight && b1.xTopLeft == xTopLeft && b1.xTopRight == xTopRight)
                    ) ||
                    ((b1.yBottomLeft == yBottomLeft && b1.yBottomRight == yBottomRight && b1.yTopLeft == yTopLeft && b1.yTopRight == yTopRight) &&
                    (b1.xBottomLeft == xBottomLeft && b1.xBottomRight == xBottomRight && b1.xTopLeft == xTopLeft && b1.xTopRight == xTopRight))
                );
            }

            if (orientation == "h")
            {
                return (
                    // x intersects
                    (xBottomLeft <= b1.xBottomLeft && xBottomRight >= b1.xBottomRight &&
                    // y intersects
                    (b1.yBottomLeft <= yBottomLeft && yTopLeft <= b1.yTopLeft))
                );
            }

            return (
                // x intersects
                (xBottomLeft >= b1.xBottomLeft && xBottomRight <= b1.xBottomRight &&
                // y intersects
                (b1.yBottomLeft >= yBottomLeft && yTopLeft >= b1.yTopLeft))
            );

        }

        public bool Move(int steps)
        {
            if (orientation == "h")
            {
                xBottomLeft = xBottomLeft + steps;
                xBottomRight = xBottomRight + steps;
                xTopLeft = xTopLeft + steps;
                xTopRight = xTopRight + steps;

                if (xBottomLeft < 1 || xBottomRight > matrixWidth + 1)
                {
                    // crushed with the wall
                    return false;
                }

                return true;
            }

            yBottomLeft = yBottomLeft + steps;
            yBottomRight = yBottomRight + steps;
            yTopLeft = yTopLeft + steps;
            yTopRight = yTopRight + steps;

            if (yBottomLeft < 1 || yTopLeft > matrixHeight + 1)
            {
                // crushed with the wall
                return false;
            }

            return true;
        }

        public bool CanMove(List<Block> blocksCol, int steps)
        {
            if (WillCrushTheWall(steps))
            {
                return false;
            }

            Move(steps);
            if (IsPositionInvalid(blocksCol))
            {
                Move(-steps);
                return false;
            }

            Move(-steps);
            return true;
        }

        private bool WillCrushTheWall(int steps)
        {
            Move(steps);

            if ((xBottomLeft < 1 || xBottomRight > matrixWidth + 1) ||
                (yBottomLeft < 1 || yTopLeft > matrixHeight + 1))
            {
                Move(-steps);
                // crushed with the wall
                return true;
            }

            Move(-steps);
            return false;
        }

        public bool IsPositionInvalid(List<Block> blocksCol)
        {
            if ((xBottomLeft < 1 || xBottomRight > matrixWidth + 1) ||
                (yBottomLeft < 1 || yTopLeft > matrixHeight + 1))
            {
                // crushed with the wall
                return true;
            }

            foreach (Block item1 in blocksCol)
            {
                if (Overlaps(item1))
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal class UnlockMe
    {
        private List<Block> blocksCol = new List<Block>();
        private Block redBlock;
        public UnlockMe(string item)
        {

            string[] blocksData = item.Split(' ');

            int width = Int32.Parse(blocksData[0]);
            int height = Int32.Parse(blocksData[1]);
            int nrOfBlocks = Int16.Parse(blocksData[2]);

            for (int i = 3; i < (nrOfBlocks * 5) + 3; i += 5)
            {
                blocksCol.Add(new Block(
                    Int32.Parse(blocksData[i]),
                    blocksData[i + 1],
                    Int32.Parse(blocksData[i + 2]),
                    Int32.Parse(blocksData[i + 3]),
                    Int32.Parse(blocksData[i + 4]),
                    width,
                    height
                    ));
            }

            redBlock = blocksCol.Find(it => it.id == 0);
            List<SolutionItem> sol = FindSolution(new List<SolutionItem>());

            if (sol != null)
            {
                Console.WriteLine("x = " + string.Join(' ', sol.Select(it => it.id + " " + it.move).ToArray()));
            }
            else
            {
                Console.WriteLine("x = Error");
            }
            // CheckWhichCanBeMoves(blocksData);
        }

        private List<SolutionItem> FindSolution(
            List<SolutionItem> solution
            )
        {
            if (IsSolutionDone())
            {
                return solution;
            }

            foreach (Block item in blocksCol)
            {
                int[] moves = { -1, 1 };
                foreach (int move in moves)
                {
                    if (item.CanMove(blocksCol, move))
                    {
                        item.Move(1);

                        SolutionItem s = new SolutionItem { id = item.id, move = move };
                        solution.Add(s);
                        List<SolutionItem> sol = FindSolution(solution);

                        if (sol != null)
                        {
                            return sol;
                        }
                        else
                        {
                            sol.Remove(s);
                        }

                    }
                }
            }

            return null;
        }

        private bool IsSolutionDone()
        {
            Block test = new Block(int.MaxValue,
                "h",
                redBlock.xBottomRight,
                redBlock.yBottomLeft,
                redBlock.matrixWidth - redBlock.xBottomRight + 1,
                redBlock.matrixWidth,
                redBlock.matrixHeight
                );

            if (test.IsPositionInvalid(blocksCol))
            {
                return false;
            }

            return true;
        }

        // used for lvl 4
        private void CheckWhichCanBeMoves(string[] blocksData)
        {
            int nrOfBlocks = Int16.Parse(blocksData[2]) * 5 + 3;
            int numberOfMoves = Int16.Parse(blocksData[nrOfBlocks]);

            for (int j = nrOfBlocks + 1; j < (numberOfMoves * 2) + nrOfBlocks + 1; j += 2)
            {
                int id = Int16.Parse(blocksData[j]);
                Block item = blocksCol.Find(it => it.id == id);

                int move = Int32.Parse(blocksData[j + 1]);

                if (!item.Move(move) || CheckItemOverlaps(item))
                {
                    int moveIdx = (j - (nrOfBlocks + 1)) / 2;
                    Console.WriteLine("x=" + moveIdx);
                    return;
                }
            }
            Console.WriteLine("x=" + numberOfMoves);
        }

        public bool CheckItemOverlaps(Block item)
        {
            foreach (Block item1 in
                blocksCol)
            {
                if (item.Overlaps(item1))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfBlocksOverlap()
        {
            foreach (Block item in blocksCol)
            {
                if (CheckItemOverlaps(item))
                {
                    return true;
                }
            }

            return false;
        }
        // used for lvl 4
    }

    class SolutionItem
    {
        public int id;
        public int move;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] blocks =
            {
                "6 5 3 0 h 2 3 3 1 h 2 5 5 2 v 6 2 2",
                "6 6 8 0 h 1 4 2 1 h 1 3 2 2 v 2 1 2 3 v 3 4 2 4 v 3 2 2 5 h 3 1 2 6 v 4 3 3 7 v 5 3 3",
                "6 6 12 0 h 3 4 2 1 h 2 3 2 2 h 1 1 3 3 h 1 2 3 4 v 1 3 2 5 v 1 5 2 6 v 3 5 2 7 h 4 6 3 8 v 4 1 2 9 h 4 3 2 10 v 5 4 2 11 v 6 4 2",
                "6 6 10 0 h 3 4 2 1 h 5 2 2 2 v 1 5 2 3 h 3 5 3 4 v 4 2 2 5 h 5 6 2 6 v 2 4 3 7 v 6 3 3 8 h 1 3 2 9 h 1 1 2",
                "12 12 8 0 h 1 7 2 1 v 6 7 5 2 h 3 8 3 3 v 3 4 4 4 v 7 2 5 5 v 9 5 4 6 v 9 9 3 7 v 9 3 2"
            };

            foreach (string item in blocks)
            {
                new UnlockMe(item);
            }

            Console.ReadLine();
        }
    }
}
