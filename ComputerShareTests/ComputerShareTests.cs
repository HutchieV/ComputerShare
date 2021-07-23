using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComputerShare;
using System.Collections.Generic;
using System;

namespace ComputerShareTests
{
    [TestClass]
    public class ComputerShareUnitTests
    {
        /// <summary>
        /// Validates test data sets
        /// </summary>
        [TestMethod]
        public void FileImportTest()
        {
            ShareWorker sh = new ShareWorker();

            string path1 = @"../../../TestData/ChallengeSampleDataSet1.txt";
            string path2 = @"../../../TestData/ChallengeSampleDataSet2.txt";

            sh.GetShares(path1);
            sh.GetShares(path2);
        }

        /// <summary>
        /// Test the outcome if the share value starts at 1 and increases
        /// by 1 every day
        /// </summary>
        [TestMethod]
        public void LinearValuesTest()
        {
            ShareWorker sh = new ShareWorker();
            List<float> ds = new List<float>();

            for (int i = 1; i < 31; ++i)
                ds.Add(i);

            var bs = sh.ProcessShares(ds);

            Assert.AreEqual(1, bs[0].Item1);   // Lowest is the first day
            Assert.AreEqual(1, bs[0].Item2);   // Lowest price is 1
            Assert.AreEqual(30, bs[1].Item1);  // Highest is the last day
            Assert.AreEqual(30, bs[1].Item2);  // Highest price is 30
        }

        /// <summary>
        /// Test the outcome if the share value starts at 30 and decrease
        /// by 1 every day
        /// 
        /// This will buy and sell on the same day (the first),
        /// which is equiv to the least losses.
        /// </summary>
        [TestMethod]
        public void LinearValuesReversedTest()
        {
            ShareWorker sh = new ShareWorker();
            List<float> ds = new List<float>();

            for (int i = 30; i > 0; --i)
                ds.Add(i);

            var bs = sh.ProcessShares(ds);

            Console.WriteLine(String.Format("{0}({1}),{2}({3})",
                bs[0].Item1,
                bs[0].Item2,
                bs[1].Item1,
                bs[1].Item2));

            Assert.AreEqual(1, bs[0].Item1);    // Lowest is the first day
            Assert.AreEqual(30, bs[0].Item2);   // Lowest price is 30
            Assert.AreEqual(1, bs[1].Item1);    // Highest is also the first day
            Assert.AreEqual(30, bs[1].Item2);   // Highest price is also 30
        }

        [TestMethod]
        public void ManualValuesTest()
        {
            ShareWorker sh = new ShareWorker();
            List<float> ds = new List<float>()
            {
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                10, 20, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15
            };

            var bs = sh.ProcessShares(ds);

            Console.WriteLine(String.Format("{0}({1}),{2}({3})",
                bs[0].Item1,
                bs[0].Item2,
                bs[1].Item1,
                bs[1].Item2));

            Assert.AreEqual(11, bs[0].Item1);   // Lowest is day 11
            Assert.AreEqual(10, bs[0].Item2);   // Lowest price is 10
            Assert.AreEqual(12, bs[1].Item1);   // Highest is day 12
            Assert.AreEqual(20, bs[1].Item2);   // Highest price is 20
        }

        /// <summary>
        /// Tests the algorithm with values where the ideal buying/selling
        /// days (the biggest difference in values) is followed by an even lower price,
        /// but with a smaller difference (so not as profitable)
        /// </summary>
        [TestMethod]
        public void LowestTrickManualValuesTest()
        {
            ShareWorker sh = new ShareWorker();
            List<float> ds = new List<float>()
            {
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                10, 20, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 5, 7.5f
            };

            var bs = sh.ProcessShares(ds);

            Console.WriteLine(String.Format("{0}({1}),{2}({3})",
                bs[0].Item1,
                bs[0].Item2,
                bs[1].Item1,
                bs[1].Item2));

            Assert.AreEqual(11, bs[0].Item1);   // Lowest is day 11
            Assert.AreEqual(10, bs[0].Item2);   // Lowest price is 10
            Assert.AreEqual(12, bs[1].Item1);   // Highest is day 12
            Assert.AreEqual(20, bs[1].Item2);   // Highest price is 20
        }

        /// <summary>
        /// Tests the algorithm with negative share prices
        /// </summary>
        [TestMethod]
        public void NegativeManualValuesTest()
        {
            ShareWorker sh = new ShareWorker();
            List<float> ds = new List<float>()
            {
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                -10, 20, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 5, 7.5f
            };

            var bs = sh.ProcessShares(ds);

            Console.WriteLine(String.Format("{0}({1}),{2}({3})",
                bs[0].Item1,
                bs[0].Item2,
                bs[1].Item1,
                bs[1].Item2));

            Assert.AreEqual(11, bs[0].Item1);   // Lowest is day 11
            Assert.AreEqual(-10, bs[0].Item2);  // Lowest price is -10
            Assert.AreEqual(12, bs[1].Item1);   // Highest is day 12
            Assert.AreEqual(20, bs[1].Item2);   // Highest price is 20
        }
    }
}
