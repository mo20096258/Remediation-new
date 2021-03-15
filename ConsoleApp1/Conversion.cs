using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Conversion
    {
        internal void Run()
        {
            ConversionFunctionality();
        }
        public static void ConversionFunctionality()
        {
            try
            {
                int valid = 0;
                List<string> linesReport = new List<string>();
                string html = "<!DOCTYPE html> <head> <link href=\"https://fonts.googleapis.com/css2?family=Noto+Serif&display=swap\" rel =\"stylesheet\" > </head> <body style=\"font-family:'Noto Serif',serif;text-align:center;background-image:linear-gradient(to right,#03A9F4,#B388FF);\">";
                Console.WriteLine("Please enter the path for code conversion");
                string sourcePath = Console.ReadLine();
                bool sourcePathdriveexists = DriveExists(sourcePath);
                if (!sourcePathdriveexists)
                {
                    valid = 1;
                    Console.WriteLine("Please enter valid Source Path");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else if (!Directory.Exists(sourcePath))
                {
                    valid = 1;
                    Console.WriteLine("Please enter valid Source Path");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Console.WriteLine("Please enter the path for Destination Folder");
                string root = Console.ReadLine();
                bool driveexists = DriveExists(root);
                if (!driveexists)
                {
                    valid = 1;
                    Console.WriteLine("Please enter valid Destination Path");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                if (valid == 0)
                {
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                        Console.WriteLine("Please wait work in progress...Don't press any key");
                        Log.Information("Begin If Directory is not exist.. CopyFolder");
                        CopyFolder(sourcePath, root);
                        Log.Information("End If Directory is not exist.. CopyFolder");
                        Remediation(root);
                    }
                    else
                    {
                        System.IO.DirectoryInfo dinfo = new DirectoryInfo(root);
                        foreach (FileInfo file in dinfo.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in dinfo.GetDirectories())
                        {
                            setAttributesNormal(dir);
                            dir.Delete(true);
                        }
                        CopyFolder(sourcePath, root);
                        Remediation(root);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Information(e.ToString());
            }
        }
        public static void Remediation(string root)
        {

            Log.Information("Entered in Remediation Block");
            string GlobalCsprojpath = string.Empty;
            var extensions1 = new List<string> { ".csproj" };
            string[] files2 = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories)
                                .Where(f => extensions1.IndexOf(Path.GetExtension(f)) >= 0).ToArray();
            //string pth = files2[0].Substring(0, files2[0].LastIndexOf("\\"));
            foreach (var folder in Directory.GetDirectories(files2[0].Substring(0, files2[0].LastIndexOf("\\"))))
            {
                DirectoryInfo dir = new DirectoryInfo(folder);
                //  var results = Array.FindAll(folders, s => s.Equals(dir.Name.ToLower()));
                if (dir.Name != ".vs" && dir.Name != "bin" && dir.Name != "obj" && dir.Name != "Views" && dir.Name != ".svn")
                {
                    var extensions = new List<string> { ".cs" };
                    string[] files1 = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                                        .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0).ToArray();
                    foreach (var file in files1)
                    {
                        Remediation remediation = new Remediation();
                        var properties = remediation.GetType().GetFields();
                        List<string> lstremediation = remediation.ReadClassFileForChecking(file, file);
                    }
                }
            }


            Log.Information("Leaving Remediation Block");
        }
        public static bool DriveExists(string fileLocation)
        {
            string drive = Path.GetPathRoot(fileLocation); // To ensure we are just testing the root directory.
            return Directory.Exists(drive); // Does the path exist?
        }
        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string Extension = Path.GetExtension(file);
                    string dest = Path.Combine(destFolder, name);
                    File.Copy(file, dest);
                }
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    if (name != ".vs" && name != ".svn" && name != ".git")
                    {
                        string dest = Path.Combine(destFolder, name);
                        CopyFolder(folder, dest);
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                Console.WriteLine(ex);
            }

        }
        public static void setAttributesNormal(DirectoryInfo dir)
        {

            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

        }

    }
}
