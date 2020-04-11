using System;
using System.Collections.Generic;

namespace LowestCostGridChallengeConsole
{
    public class LowestCostPath
    {
        const int maxCostAllowed = 50;
        bool isDebug = false;

        const int minNumberOfRows = 1;
        const int maxNumberOfRows = 10;

        const int minNumberOfColumns = 5;
        const int maxNumberOfColumns = 100;

        public const string messageSuccess = "Yes";
        public const string messageFailure = "No";

        public const string messageSeparator = "\n";

        public const string messageInvalidRowCount = "Invalid Row Count";
        public const string messageInvalidColumnCount = "Invalid Column Count";

        private class MinimumPathResult
        {
            public bool IsUnderThreshold { get; set; }
            public int? MinimumTotalCost { get; set; }
            public int HighestColumnIndexAchieved { get; set; }
            public List<int> RowNumberList { get; set; }

            public MinimumPathResult()
            {
                IsUnderThreshold = true;
                MinimumTotalCost = null;
                HighestColumnIndexAchieved = 0;
                RowNumberList = new List<int>();
            }
        }

        static void Main()
        {
            // uncomment to test in console
            //LowestCostPath lowestCostPath = new LowestCostPath();

            //int[,] costMultiArray =
            //{
            //    {3, 4, 1, 2, 8, 6},
            //    {6, 1, 8, 2, 7, 4},
            //    {5, 9, 3, 9, 9, 5},
            //    {8, 4, 1, 3, 2, 6},
            //    {3, 7, 2, 8, 6, 4}
            //};

            //lowestCostPath.ProcessCostMultiArray(true, costMultiArray);
        }

        public string ProcessCostMultiArray(bool isDebugMode, int[,] costMultiArray)
        {
            isDebug = isDebugMode;

            int totalNumberOfRows = costMultiArray.GetLength(0);
            int totalNumberOfColumns = costMultiArray.GetLength(1);

            if (totalNumberOfRows < minNumberOfRows || totalNumberOfRows > maxNumberOfRows)
            {
                throw new ArgumentOutOfRangeException("row count", totalNumberOfRows, LowestCostPath.messageInvalidRowCount);
            }
            else if (totalNumberOfColumns < minNumberOfColumns || totalNumberOfColumns > maxNumberOfColumns)
            {
                throw new ArgumentOutOfRangeException("column count", totalNumberOfColumns, LowestCostPath.messageInvalidColumnCount);
            }

            if (isDebug)
            {
                Console.WriteLine("Total Rows: " + totalNumberOfRows + ", Total Columns: " + totalNumberOfColumns);
            }

            MinimumPathResult winningPathInfo = null;

            // get the full matrix options for each cell in the first column,
            // the first cells constitute all of the possible starting points
            for (int rowIndex = 0; rowIndex < totalNumberOfRows; rowIndex++)
            {
                if (isDebug)
                {
                    Console.WriteLine("Checking Row: " + rowIndex);
                }

                MinimumPathResult currentPathInfo = GetMinimumPathFromCostArray(costMultiArray, totalNumberOfRows, totalNumberOfColumns, rowIndex, 0);

                if (winningPathInfo == null || (currentPathInfo.HighestColumnIndexAchieved >= winningPathInfo.HighestColumnIndexAchieved && currentPathInfo.MinimumTotalCost < winningPathInfo.MinimumTotalCost))
                {
                    winningPathInfo = currentPathInfo;
                }
            }

            if (isDebug)
            {
                Console.WriteLine("Final Lowest Minimum Cost: " + winningPathInfo.MinimumTotalCost + ", Winning Starting Row Index: " + (winningPathInfo.RowNumberList[0] - 1));
            }

            string resultTxt = LowestCostPath.messageFailure;

            if (winningPathInfo.IsUnderThreshold)
            {
                resultTxt = LowestCostPath.messageSuccess;
            }

            resultTxt += LowestCostPath.messageSeparator + winningPathInfo.MinimumTotalCost;

            var rowIndexPathTxt = String.Join(" ", winningPathInfo.RowNumberList.GetRange(0, winningPathInfo.HighestColumnIndexAchieved + 1).ToArray());
            resultTxt += LowestCostPath.messageSeparator + rowIndexPathTxt;

            if (isDebug)
            {
                Console.WriteLine("Highest Column Achieved: " + winningPathInfo.HighestColumnIndexAchieved);
                Console.WriteLine(resultTxt);
            }

            return resultTxt;
        }

        private MinimumPathResult GetMinimumPathFromCostArray(int[,] costMultiArray, int totalNumberOfRows, int totalNumberOfColumns, int startingRowIndex, int startingColumnIndex)
        {
            MinimumPathResult pathResultInfo = new MinimumPathResult();

            int[,] costMatrix = new int[totalNumberOfRows, totalNumberOfColumns];

            // int literal types in multi-dimensional arrays automagically default to 0
            // so i couldnt use that value to indicate if it was previosuly set or not
            // therefore im using this bool multi-dimensional array as well
            bool[,] isPopulatedMatrix = new bool[totalNumberOfRows, totalNumberOfColumns];

            List<int>[,] rowNumberMatrix = new List<int>[totalNumberOfRows, totalNumberOfColumns];

            // populate the cost of the first cell to start with
            costMatrix[startingRowIndex, startingColumnIndex] = costMultiArray[startingRowIndex, startingColumnIndex];
            isPopulatedMatrix[startingRowIndex, startingColumnIndex] = true;
            rowNumberMatrix[startingRowIndex, startingColumnIndex] = new List<int>();

            // populate the minimum cost of all appropriate cells for each subsequent column
            for (int currentColumnIndex = startingColumnIndex + 1; currentColumnIndex < totalNumberOfColumns; currentColumnIndex++)
            {
                // go through each row for the next column and determine if the minimum cost matrix value should be set
                for (int currentRowIndex = 0; currentRowIndex < totalNumberOfRows; currentRowIndex++)
                {
                    int previousColumnIndex = currentColumnIndex - 1;

                    int previousRowIndexDiagonalUp = currentRowIndex - 1;
                    int previousRowIndexDiagonalDown = currentRowIndex + 1;

                    // wrap up from the first row to the bottom row
                    if (previousRowIndexDiagonalUp < 0)
                    {
                        previousRowIndexDiagonalUp = totalNumberOfRows - 1;
                    }

                    // wrap down from the last row to the top row
                    if (previousRowIndexDiagonalDown >= totalNumberOfRows)
                    {
                        previousRowIndexDiagonalDown = 0;
                    }

                    bool isPreviousCostSetDiagonalUp = isPopulatedMatrix[previousRowIndexDiagonalUp, previousColumnIndex];
                    bool isPreviousCostSetSameRow = isPopulatedMatrix[currentRowIndex, previousColumnIndex];
                    bool isPreviousCostSetDiagonalDown = isPopulatedMatrix[previousRowIndexDiagonalDown, previousColumnIndex];

                    int lowestPreviousCost = 0;
                    int previousRowIndexUsed = 0;

                    bool isLowestPreviousCostSet = false;

                    if (isPreviousCostSetDiagonalUp)
                    {
                        CheckCostAndSetIfLowestValue(previousRowIndexDiagonalUp, previousColumnIndex, ref isLowestPreviousCostSet, costMatrix, ref lowestPreviousCost, ref previousRowIndexUsed);
                    }

                    if (isPreviousCostSetSameRow)
                    {
                        CheckCostAndSetIfLowestValue(currentRowIndex, previousColumnIndex, ref isLowestPreviousCostSet, costMatrix, ref lowestPreviousCost, ref previousRowIndexUsed);
                    }

                    if (isPreviousCostSetDiagonalDown)
                    {
                        CheckCostAndSetIfLowestValue(previousRowIndexDiagonalDown, previousColumnIndex, ref isLowestPreviousCostSet, costMatrix, ref lowestPreviousCost, ref previousRowIndexUsed);
                    }

                    if (isLowestPreviousCostSet)
                    {
                        int costForCurrentCell = costMultiArray[currentRowIndex, currentColumnIndex];
                        int lowestTotalForCurrentCell = costForCurrentCell + lowestPreviousCost;

                        costMatrix[currentRowIndex, currentColumnIndex] = lowestTotalForCurrentCell;
                        isPopulatedMatrix[currentRowIndex, currentColumnIndex] = true;

                        List<int> currentList = new List<int>();
                        List<int> previousList = rowNumberMatrix[previousRowIndexUsed, previousColumnIndex];

                        for (int i = 0; i < previousList.Count; i++)
                        {
                            currentList.Add(previousList[i]);
                        }

                        currentList.Add(previousRowIndexUsed + 1);

                        rowNumberMatrix[currentRowIndex, currentColumnIndex] = currentList;
                    }
                }
            }

            if (isDebug)
            {
                // print the matrix for debug - rows x columns
                for (int i = 0; i < totalNumberOfRows; i++)
                {
                    string rowCostsTxt = "";

                    for (int j = 0; j < totalNumberOfColumns; j++)
                    {
                        int cost = costMatrix[i, j];
                        rowCostsTxt += "[" + cost + "]";
                    }

                    Console.WriteLine(rowCostsTxt);
                }
            }

            int winningRowIndex = 0;

            for (int j = 0; j < totalNumberOfColumns; j++)
            {
                int? lowestCostForColumn = null;
                List<int> winningRowNumberList = null;

                for (int i = 0; i < totalNumberOfRows; i++)
                {
                    int cost = costMatrix[i, j];

                    if (lowestCostForColumn == null || cost < lowestCostForColumn)
                    {
                        lowestCostForColumn = cost;

                        winningRowIndex = i;
                        winningRowNumberList = rowNumberMatrix[i, j];
                    }
                }

                if (lowestCostForColumn < maxCostAllowed)
                {
                    if (isDebug)
                    {
                        Console.WriteLine("Lowest cost for Column[" + j + "]: " + lowestCostForColumn);
                    }

                    pathResultInfo.MinimumTotalCost = lowestCostForColumn;
                    pathResultInfo.HighestColumnIndexAchieved = j;
                }
                else
                {
                    pathResultInfo.IsUnderThreshold = false;
                }

                pathResultInfo.RowNumberList = winningRowNumberList;
            }

            pathResultInfo.RowNumberList.Add(winningRowIndex + 1);
            return pathResultInfo;
        }

        private void CheckCostAndSetIfLowestValue(int rowIndex, int columnIndex, ref bool isLowestPreviousCostSet, int[,] costMatrix, ref int lowestPreviousCost, ref int previousRowIndexUsed)
        {
            int cost = costMatrix[rowIndex, columnIndex];

            if (!isLowestPreviousCostSet || cost < lowestPreviousCost)
            {
                lowestPreviousCost = cost;
                isLowestPreviousCostSet = true;

                previousRowIndexUsed = rowIndex;
            }
        }
    }
}
