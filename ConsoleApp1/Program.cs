using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Param _param = new Param(args);
                _param.CheckParams();

                string _helptext = _param.GetHelpIfNeeded();
                //Print help to console if requested
                if (!string.IsNullOrEmpty(_helptext))
                {
                    Console.WriteLine(_helptext);
                    Environment.Exit(0);
                }

                string _source = _param.sourcePath;
                string _dest = _param.destinationPath;
                Console.WriteLine(_source);
                Console.WriteLine(_dest);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
