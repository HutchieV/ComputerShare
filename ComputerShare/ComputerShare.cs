using System;
using System.IO;

namespace ComputerShare
{
    class Entry
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

            ShareWorker sh = new ShareWorker();

            var dailyShares = sh.GetShares(args[0]);
            var bestShares  = sh.ProcessShares(dailyShares);

            Console.Error.WriteLine();
            Console.WriteLine(String.Format("{0}({1}),{2}({3})", 
                bestShares[0].Item1, 
                bestShares[0].Item2, 
                bestShares[1].Item1, 
                bestShares[1].Item2));

            return 0;
        }

    }

}
