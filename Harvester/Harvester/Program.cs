using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Harvester
{
    class Harvest
    {
        int[][] cultureData;
        string output;
        Dictionary<int, int> frecvence = new Dictionary<int, int>();

        int rows;
        int columns;
        int startRow;
        int startColumn;
        private string direction;
        private string orientation;
        private int width;

        public Harvest(string harvestData)
        {
            string[] harvestSpecs = harvestData.Split(' ');
            LoadCultureTable(harvestSpecs);
            StartHarvest();
        }

        private void StartHarvest()
        {
            if (orientation != "Z")
            {
                HarvestSerpentine();
                return;
            }

            HarvestCircular();
            //if (direction != "N" && direction != "S")
            //{
            //    HarvestWestEst();
            //    return;
            //}

            //HarvestNorthSouth();

        }

        //public void HarvestWestEst()
        //{
        //    string result = "";

        //    int j = startColumn;
        //    bool goForeward = direction == "W";
        //    bool willCompleteFromFirstPass = false;

        //    if (goForeward && startColumn == columns - 1 && startRow == 0)
        //    {
        //        goForeward = false;
        //        willCompleteFromFirstPass = true;
        //    }
        //    else if (!goForeward && startColumn == columns - 1 && startRow == rows - 1)
        //    {
        //        willCompleteFromFirstPass = true;
        //    }

        //    for (int i = startRow; i < rows; i++)
        //    {
        //        result += GetLine(goForeward, i, j);
        //        j = goForeward ? columns - 1 : 0;
        //        goForeward = !goForeward;

        //    }


        //    if (!willCompleteFromFirstPass)
        //    {
        //        goForeward = direction != "W";
        //        j = goForeward ? 0 : startColumn - 1;
        //        for (int i = startRow; i >= 0; i--)
        //        {
        //            if (j >= 0)
        //            {
        //                result += GetLine(goForeward, i, j);
        //                goForeward = !goForeward;
        //                j = goForeward ? columns - 1 : 0;
        //            }
        //            else
        //            {
        //                j = columns - 1;
        //            }
        //        }
        //    }
        //    output = result;
        //    Console.WriteLine(output.Trim());
        //}

        public void HarvestSerpentine()
        {
            bool isRightCompleted = false;
            bool isLeftCompleted = false;

            int i = startRow;
            int j = startColumn;
            bool goForeward = direction == "W" || direction == "S";
            string result = "";

            if (direction == "O" && startColumn == 0)
            {
                goForeward = true;
            }
            else if (direction == "W" && startColumn == columns - 1)
            {
                goForeward = false;
            }
            else if (direction == "S" && startRow == rows - 1)
            {
                goForeward = true;
            }
            else if (direction == "N" && startRow == 0)
            {
                goForeward = false;
            }

            while (result.Trim().Split(' ').Length != rows * columns)
            {
                if (direction == "N" || direction == "S")
                {
                    result += GetColumn(goForeward, i, j, width);
                    i = goForeward ? rows - 1 : 0;
                    goForeward = !goForeward;
                    j = !isRightCompleted ? j + width : j - width;
                    if (!isRightCompleted && j == columns)
                    {
                        isRightCompleted = true;
                        j = direction == "S" && startColumn == columns - 1 ? startColumn - 1 : startColumn;
                        i = j == startColumn - 1 ? (goForeward ? 0 : rows - 1) : startRow;

                        if (
                            (direction == "N" && startColumn == 0 && startRow == rows - 1) ||
                            (direction == "S" && startColumn == 0 && startRow == 0)
                            )
                        {
                            isLeftCompleted = true;
                        }

                    }
                    else if (isRightCompleted && j < 0)
                    {
                        isLeftCompleted = true;
                    }
                }
                else
                {
                    result += GetLine(goForeward, i, j, width, result);
                    j = goForeward ? columns - 1 : 0;
                    goForeward = !goForeward;
                    i = !isRightCompleted ? i + width : i - width;
                    if (!isRightCompleted && i >= rows)
                    {
                        isRightCompleted = true;
                        i = startRow;
                        j = startColumn;

                        //if (
                        //     (startColumn == 0 && startRow == 0) ||
                        //     (startColumn == columns - 1 && rows - 1 == startRow)
                        //     )
                        //{
                        //    isLeftCompleted = true;
                        //}
                    }
                    else if (isRightCompleted && i == 0)
                    {
                        isLeftCompleted = true;
                    }
                }
            }

            Console.WriteLine(result.Trim());
        }

        public void HarvestCircular()
        {

            int i = startRow;
            int j = startColumn;

            bool goForeward = direction == "W" || direction == "S";
            string result = "";
            int jIncrease = 0;
            int toggle = 1;
            bool isCompleted = false;

            if (direction == "O" && startColumn == 0)
            {
                goForeward = true;
            }
            else if (direction == "W" && startColumn == columns - 1)
            {
                goForeward = false;
            }
            else if (direction == "S" && startRow == rows - 1)
            {
                goForeward = true;
            }
            else if (direction == "N" && startRow == 0)
            {
                goForeward = false;
            }

            while (!isCompleted)
            {
                int addingCost = toggle == 0 ? 1 : 0;

                if (direction == "N" || direction == "S")
                {
                    result += GetColumn(goForeward, i, j, width);
                    goForeward = !goForeward;
                    i = goForeward ? rows - 1 : 0;

                    addingCost = addingCost + width;

                    if (j == (columns - 1) - jIncrease + addingCost)
                    {
                        j = 0 + jIncrease + addingCost;
                    }
                    else
                    {
                        j = (columns - 1) - jIncrease - addingCost;
                    }
                }
                else
                {
                    result += GetLine(goForeward, i, j, width, result);
                    j = goForeward ? columns - 1 : 0;
                    goForeward = !goForeward;

                    if (i == (rows - width) - jIncrease + addingCost)
                    {
                        i = width - 1 + jIncrease + addingCost;
                    }
                    else
                    {
                        i = (rows - width * jIncrease) - addingCost;
                    }
                }
                toggle++;
                if (toggle == 2)
                {
                    jIncrease++;
                    toggle = 0;
                }

                if (result.Trim().Split(' ').Length == rows * columns)
                {
                    isCompleted = true;
                }
            }

            Console.WriteLine(result.Trim());
        }

        public string GetLine(bool goForeward, int i, int colSt, int width, string visited)
        {
            visited = " " + visited.Trim() + " ";
            string result = "";
            int j = colSt;

            if (goForeward && i < rows && i >= 0)
            {
                while (j < columns)
                {
                    for (int k = 0; k < width; k++)
                    {
                        if (k + i < rows && k + i >= 0)
                        {
                            result += visited.IndexOf(" " + cultureData[i + k][j] + " ") == -1 ? cultureData[i + k][j] + " " : "";
                        }
                    }
                    j++;
                }


                return result;
            }

            while (j >= 0 && j < columns && i < rows && i >= 0)
            {
                for (int k = 0; k < width; k++)
                {

                    int f = orientation == "Z" ? i - k : i + k;
                    if (f < rows && f >= 0)
                    {
                        result += visited.IndexOf(" " + cultureData[f][j] + " ") == -1 ? cultureData[f][j] + " " : "";
                    }
                }
                j--;
                //result += cultureData[i][j] + " ";
            }
            return result;
        }

        public string GetColumn(bool goForeward, int lineSt, int colSt, int width)
        {
            string result = "";
            int i = lineSt;

            if (goForeward && i < rows && i >= 0)
            {
                while (i < rows)
                {
                    for (int k = 0; k < width; k++)
                    {
                        if (colSt + k < columns && colSt + k >= 0)
                        {
                            result += cultureData[i][colSt + k] + " ";
                        }
                    }
                    i++;
                }


                return result;
            }

            while (i >= 0 && i < rows && i < rows && i >= 0)
            {
                for (int k = 0; k < width; k++)
                {
                    if (colSt - k < columns && colSt - k >= 0)
                    {
                        result += cultureData[i][colSt - k] + " ";
                    }
                }
                i--;
            }
            return result;
        }

        private void LoadCultureTable(string[] harvestSpecs)
        {
            rows = Int32.Parse(harvestSpecs[0]);
            columns = Int32.Parse(harvestSpecs[1]);
            // using 0 index
            startRow = Int32.Parse(harvestSpecs[2]) - 1;
            startColumn = Int32.Parse(harvestSpecs[3]) - 1;
            direction = harvestSpecs[4];
            orientation = harvestSpecs[5];
            width = Int32.Parse(harvestSpecs[6]);

            cultureData = new int[rows][];
            int k = 1;
            for (int i = 0; i < rows; i++)
            {
                cultureData[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    cultureData[i][j] = k;
                    k++;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] harvester =
            {
    
                //"5 4 4 1 O Z 2",
                "10 10 10 10 W S 1",
                "10 10 10 10 W S 2",
                "17 9 17 1 N Z 2"
            };

            foreach (string item in harvester)
            {
                new Harvest(item);
            }
            Console.ReadLine();
        }
    }
}
