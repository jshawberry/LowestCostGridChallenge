using Microsoft.VisualStudio.TestTools.UnitTesting;
using LowestCostGridChallengeConsole;

namespace LowestCostTests
{
    [TestClass]
    public class LowestCostUnitTest
    {
        [TestMethod]
        public void TestCaseSuccessLeftToRight()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {3, 4, 1, 2, 8, 6},
                {6, 1, 8, 2, 7, 4},
                {5, 9, 3, 9, 9, 5},
                {8, 4, 1, 3, 2, 6},
                {3, 7, 2, 8, 6, 4}
            };

            string expectedTxt = LowestCostPath.messageSuccess + LowestCostPath.messageSeparator + "16" + LowestCostPath.messageSeparator + "1 2 3 4 4 5";
            string actualTxt = lowestCostPath.ProcessCostMultiArray(false, costMultiArray);

            Assert.AreEqual(expectedTxt, actualTxt);
        }

        [TestMethod]
        public void TestCaseSuccessWrapper()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {3, 4, 1, 2, 8, 6},
                {6, 1, 8, 2, 7, 4},
                {5, 9, 3, 9, 9, 5},
                {8, 4, 1, 3, 2, 6},
                {3, 7, 2, 1, 2, 3}
            };

            string expectedTxt = LowestCostPath.messageSuccess + LowestCostPath.messageSeparator + "11" + LowestCostPath.messageSeparator + "1 2 1 5 4 5";
            string actualTxt = lowestCostPath.ProcessCostMultiArray(false, costMultiArray);

            Assert.AreEqual(expectedTxt, actualTxt);
        }

        [TestMethod]
        public void TestCaseCostTooHigh()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {19, 10, 19, 10, 19},
                {21, 23, 20, 19, 12},
                {20, 12, 20, 11, 10}
            };

            string expectedTxt = LowestCostPath.messageFailure + LowestCostPath.messageSeparator + "48" + LowestCostPath.messageSeparator + "1 1 1";
            string actualTxt = lowestCostPath.ProcessCostMultiArray(false, costMultiArray);

            Assert.AreEqual(expectedTxt, actualTxt);
        }

        [TestMethod]
        public void TestRowCountTooHigh()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {19, 10, 19, 10, 19},
                {21, 23, 20, 19, 12},
                {20, 12, 20, 11, 10}
            };

            try
            {
                lowestCostPath.ProcessCostMultiArray(false, costMultiArray);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, LowestCostPath.messageInvalidRowCount);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestRowCountTooLow()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
            };

            try
            {
                lowestCostPath.ProcessCostMultiArray(false, costMultiArray);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, LowestCostPath.messageInvalidRowCount);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestColumnCountTooHigh()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
                {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0}
            };

            try
            {
                lowestCostPath.ProcessCostMultiArray(false, costMultiArray);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, LowestCostPath.messageInvalidColumnCount);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestColumnCountTooLow()
        {
            LowestCostPath lowestCostPath = new LowestCostPath();

            int[,] costMultiArray =
            {
                {1, 2},
                {4, 5}
            };

            try
            {
                lowestCostPath.ProcessCostMultiArray(false, costMultiArray);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, LowestCostPath.messageInvalidColumnCount);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }
    }
}
