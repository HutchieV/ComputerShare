using System;
using System.IO;
using System.Collections.Generic;

namespace ComputerShare
{
    class ShareReader
    {
        static int Main(string[] args)
        {
            // We print to Error stream so that the standard output only
            // contains the requested data
            Console.Error.WriteLine("ComputerShare Coding Challenge");
            Console.Error.WriteLine(String.Format("Launching from: {0}", Directory.GetCurrentDirectory()));

            // Check we have received a path to the shares data
            // If not, return erroneous exit code
            if (args.Length == 0)
            {
                Console.Error.WriteLine("!! Please provide a path to the historical shares data !!");
                return 1;
            }

            // Print path for debug
            Console.Error.WriteLine(String.Format("Using path: {0}", args[0]));

            var _dailyShares = GetShares(args[0]);
            var _bestShares  = ProcessShares(_dailyShares);

            Console.Error.WriteLine();
            Console.WriteLine(String.Format("{0}({1}),{2}({3})", 
                _bestShares[0].Item1, 
                _bestShares[0].Item2, 
                _bestShares[1].Item1, 
                _bestShares[1].Item2));

            return 0;
        }

        /// <summary>
        /// Parses a comma-delimited file containing daily share prices
        /// </summary>
        /// <param name="path">Path to shares data file</param>
        /// <returns>A List of floats which represent the daily share prices</returns>
        private static List<float> GetShares(string path)
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
        private static List<Tuple<int, float>> ProcessShares(List<float> shares)
        {
            List<Tuple<int, float>> returnValues = new List<Tuple<int, float>>();

            int _lowest     = 0;
            int _highest    = 0;
            float _diff     = 0;

            // For every day...
            for(int i = 0; i < shares.Count; i++)
            {
                // Compare this day's value to every successive day
                // to find the biggest difference (starting from the lowest value)
                for(int j = i; j < shares.Count; j++)
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

            Console.Error.WriteLine(String.Format("L: {0}, H: {1}, D: {2}", _lowest+1, _highest+1, _diff));

            returnValues.Add(new Tuple<int, float>(_lowest + 1, shares[_lowest]));
            returnValues.Add(new Tuple<int, float>(_highest + 1, shares[_highest]));

            return returnValues;
        }
    }

}
