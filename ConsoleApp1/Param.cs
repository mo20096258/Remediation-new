using ConsoleCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Param : ParamsObject
    {

        public Param(string[] args)
            : base(args)
        {

        }
        [Switch("S",true)]
        [SwitchHelpText("Source directory of project")]
        public string sourcePath { get; set; }
        [Switch("D",true)]
        [SwitchHelpText("Destination directory of project")]
        public string destinationPath { get; set; }


        [HelpText(0)]
        public string Description
        {
            get { return "Remediate codebase"; }
        }
        [HelpText(1, "Example")]
        public string ExampleText
        {
            get { return "This is an example: Console1.exe 'source path' 'destination path' "; }
        }
        [HelpText(2)]
        public override string Usage
        {
            get { return base.Usage; }
        }
        [HelpText(3, "Parameters")]
        public override string SwitchHelp
        {
            get { return base.SwitchHelp; }
        }

    }
}
