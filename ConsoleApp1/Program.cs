using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = String.Empty;
            string destFile = String.Empty;

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

                foreach (string dirPath in Directory.GetDirectories(_source, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(_source, _dest));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(_source, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(_source, _dest), true);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

       
    }
}
