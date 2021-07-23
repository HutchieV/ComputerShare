using System;
using System.IO;
using System.Collections.Generic;

namespace ComputerShare
{
    /// <summary>
    /// ShareWorker class which includes functions to parse a shares data file
    /// and find the ideal lowest buying and highest selling days in a month
    /// </summary>
    public class ShareWorker
    {
        /// <summary>
        /// Instantiates a ShareWorker object
        /// </summary>
        public ShareWorker() {}

        /// <summary>
        /// Parses a comma-delimited file containing daily share prices
        /// </summary>
        /// <param name="path">Path to shares data file</param>
        /// <returns>A List of floats which represent the daily share prices</returns>
        public List<float> GetShares(string path)
        {
            List<float> _shares = new List<float>();

            // There isn't much we can do if an error occurs,
            // so we'll let the program throw the exception
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var _tempShares = reader.ReadLine().Split(',');

                    foreach (string s in _tempShares)
                    {
                        _shares.Add(float.Parse(s));
                    }
                }
            }

            return _shares;
        }

        /// <summary>
        /// Finds the biggest difference between two share prices, always
        /// lowest price to highest price.
        /// </summary>
        /// <param name="shares">A List of floats which represent the daily share prices</param>
        /// <returns>A List of Tuples in the form (dayOfMonth, price), which
        /// represents the buying day (lowest price) and selling day (highest price)</returns>
        public List<Tuple<int, float>> ProcessShares(List<float> shares)
        {
            List<Tuple<int, float>> returnValues = new List<Tuple<int, float>>();

            int _lowest = -1;
            int _highest = -1;
            float _diff = -1;

            // For every day...
            for (int i = 0; i < shares.Count; i++)
            {
                // Compare this day's value to every successive day
                // to find the biggest difference (starting from the lowest value)
                for (int j = i; j < shares.Count; j++)
                {
                    float _thisDiff = shares[j] - shares[i];
                    if (_thisDiff > _diff)
                    {
                        _lowest = i;
                        _highest = j;
                        _diff = _thisDiff;
                    }
                }
            }

            // Console.Error.WriteLine(String.Format("L: {0}, H: {1}, D: {2}", _lowest + 1, _highest + 1, _diff));

            returnValues.Add(new Tuple<int, float>(_lowest + 1, shares[_lowest]));
            returnValues.Add(new Tuple<int, float>(_highest + 1, shares[_highest]));

            return returnValues;
        }
    }
}
