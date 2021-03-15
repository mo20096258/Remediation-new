using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace ConsoleApp1
{
    class Remediation
    {
        public string serilogChecked;
        public string Catchblock;
        static RulesConfiguration objrulesconfig = new RulesConfiguration();
        List<string> linesReport = new List<string>();
        public List<string> ReadClassFileForChecking(string sourceFile, string destinationFile)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
               var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
               var configuration = builder.Build();
                ConfigurationBinder.Bind(configuration.GetSection("AppSettings"), objrulesconfig);
                string FileNamewithoutExtension = Path.GetFileNameWithoutExtension(sourceFile);
                string FileName = Path.GetFileName(sourceFile);
                if (FileName != "Reference.cs")
                {
                    string[] lines = File.ReadAllLines(sourceFile);

                    string stringconcet = "+=";
                    string searchWord = "StringBuilder ";
                    string stringToCheck1 = "== string.Empty";
                    string stringCatch = "Dispose()";
                    string conn = objrulesconfig.InstantiationsFor;
                    // StringBuilder in Loop
                    if (objrulesconfig.InstantiationsFor == "true")
                        StringBuilderInLoop(lines, searchWord);
                    //if (objrulesconfig.PublicField == "true")
                    //    lines = MakeProperties(lines);
                    //Avoid having transaction with the Thread.Sleep method in
                    if (objrulesconfig.ThreadSleep == "true")
                        searchWord = ThreadSleepInLookCheck(lines);
                    // Avoid using a break statement in 'for' loops
                    if (objrulesconfig.BreakInFor == "true")
                    {
                        searchWord = "break;";
                        var foundBreak = lines.Where(x => x.Contains(searchWord));
                        if (foundBreak.Count() > 0)
                        {
                            BreakInLoop(lines);
                        }
                    }
                    //Avoid unreferenced Form
                    if (objrulesconfig.ManyConstructors == "true")
                    {
                        ManyConstructorsCheck(lines);
                    }
                    ManualRefactorComments(lines);
                    if (objrulesconfig.DisposeRule == "true")
                    {
                        InsertLine(lines, stringCatch);
                    }
                    //Avoid empty catch blocks
                    stringCatch = "catch (";
                    if (objrulesconfig.EmptyCatch == "true")
                    {
                        InsertLine(lines, stringCatch);
                    }
                    //Avoid empty finally blocks
                    stringCatch = "finally";
                    if (objrulesconfig.EmptyFinal == "true")
                    {
                        InsertLine(lines, stringCatch);
                    }
                    //Avoid String concatenation in loops (.NET)
                    //Avoid large number of String concatenation (.NET)
                    lines = StringConcatenationCheck(stringconcet, lines);
                    //Avoid using String.Empty for empty string tests
                    if (objrulesconfig.StringEmpty == "true")
                    {
                        lines = AvoidStringEmptyCheck(stringToCheck1, lines);
                    }

                    stringCatch = "using Serilog;";
                    //Add namespace if elk checked
                    if (objrulesconfig.AvoidSingleCharVariable == "true")
                    {
                        AvoidSingleCharVariable(lines);
                    }
                    if (objrulesconfig.AvoidUnderscore == "true")
                    {
                        AvoidUnderscore(lines);
                    }
                    if (objrulesconfig.CamelCaseNaming == "true")
                    {
                        CamelCaseNaming(lines);
                    }
                    if (objrulesconfig.DirectAccesstoDbCheck == "true")
                    {
                        DirectAccesstoDbCheck(lines);
                    }
                    if (objrulesconfig.DisableConstraintsBeforeMergingDataSet == "true")
                    {
                        DisableConstraintsBeforeMergingDataSet(lines);


                    }
                    if (objrulesconfig.AvoidcalltoAcceptChangesinaloop == "true")
                    {
                        AvoidcalltoAcceptChangesinaloop(lines);

                    }
                    if (objrulesconfig.AvoidUsingGotoStatement == "true")
                    {
                        AvoidUsingGotoStatement(lines);

                    }
                    if (objrulesconfig.AvoidArtifactsWithTooManyParameters == "true")
                    {
                        AvoidArtifactsWithTooManyParameters(lines);

                    }
                    if (objrulesconfig.Namespaceuppercase == "true")
                    {
                        Namespaceuppercase(lines);
                    }
                    //if (objrulesconfig.nuget == "true")
                    //{
                    //    nuget(lines);
                    //}
                    if (objrulesconfig.UnusedVariables == "true")
                    {
                        lines = RSPEC1481(lines);
                    }
                    if (objrulesconfig.CollapsibleIfToBeMerged == "true")
                    {
                        lines = RSPEC1066(lines);
                    }
                    if (objrulesconfig.SwitchToHaveAtleastThreeCases == "true")
                    {
                        lines = RSPEC1301(lines);
                    }
                    if (objrulesconfig.UtilityClassesNotInstantiated == "true")
                    {
                        lines = RSPEC1118(lines);
                    }
                    if (objrulesconfig.DuplicateImplementation == "true")
                    {
                        lines = RSPEC1871(lines);
                    }
                    if (objrulesconfig.IndexOfChecks == "true")
                    {
                        lines = RSPEC2692(lines);
                    }
                    if (objrulesconfig.ForLoopCounterRightDirection == "true")
                    {
                        lines = RSPEC2251(lines);
                    }
                    if (objrulesconfig.NonConstantStaticField == "true")
                    {
                        lines = RSPEC2223(lines);
                    }
                    if (objrulesconfig.GetHashCodeMutableField == "true")
                    {
                        lines = RSPEC2328(lines);
                    }

                    if (objrulesconfig.EmptyArrayandCollection == "true")
                    {
                        lines = RSPEC1168(lines);
                    }
                    if (objrulesconfig.stringIsNullOrEmpty == "true")
                    {
                        lines = RSPEC3256(lines);
                    }
                    if (objrulesconfig.EqualsOverridden == "true")
                    {
                        lines = RSPEC1698(lines);
                    }
                    if (objrulesconfig.RightOperandOfShiftOperator == "true")
                    {
                        lines = RSPEC3449(lines);
                    }
                    if (objrulesconfig.AvoidMethodwithManyParameters == "true")
                    {
                        lines = RSPEC107(lines);
                    }
                    // Write the new file over the old file.
                    //File.WriteAllLines(destinationFile, lines);
                    WriteToFile(destinationFile, lines);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Remediation Exceptions");
                //Console.WriteLine(ex);
            }
            return linesReport;
        }
        #region remediation rules
        public string Findsubstring(string text, string startstr, string endstr, bool flag)
        {
            int startIndex, endIndex;
            string substring;
            if (flag == true)  //true for taking the last occurrence of endstring variable
            {
                startIndex = text.IndexOf(startstr) + startstr.Length;
                endIndex = text.LastIndexOf(endstr);
                substring = text.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                startIndex = text.IndexOf(startstr) + startstr.Length;
                endIndex = text.IndexOf(endstr);
                substring = text.Substring(startIndex, endIndex - startIndex);
            }
            return substring;
        }
        public string Replacefun(List<string> fundef)
        {
            string text = "null";
            if (fundef.Any(x => x.Contains("[]")))
            {
                string Arraytype = fundef.FirstOrDefault(t => t.Contains("[]")).Trim('[', ']');
                if (!(Arraytype.Equals("string") || Arraytype.Equals("char")))
                    text = text.Replace("null", "new " + Arraytype + "[0]");
            }

            if (fundef.Any(x => x.Contains("<") && x.Contains(">")))
            {
                string[] arr = fundef.FirstOrDefault(t => t.Contains("<") && t.Contains(">")).Split('<', '>');
                if (!(arr[1].Equals("string")))
                {
                    if (arr[0].Equals("IEnumerable"))
                        text = text.Replace("null", "Enumerable.Empty<" + arr[1] + ">()");
                    else
                        text = text.Replace("null", "(" + arr[0] + "<" + arr[1] + ">)Enumerable.Empty<" + arr[1] + ">()");
                }
            }
            return text;
        }
        public string RemoveWhiteSpace(string text, string Replacestring)
        {
            if (text.Contains(Replacestring + " ") || text.Contains(" " + Replacestring + " ") || text.Contains(" " + Replacestring))
                return text.Replace(Replacestring + " ", Replacestring).Replace(" " + Replacestring + " ", Replacestring).Replace(" " + Replacestring, Replacestring);
            else
                return text;

        }
        public string[] RSPEC2692(string[] lines)
        {
            List<string> checks_condition = new List<string> { "while", "if", "switch" };
            for (int i = 0; i < lines.Count(); i++)
            {
                if (checks_condition.Any(x => lines[i].Contains(x)) && lines[i].Contains("IndexOf") && lines[i].Contains(">") && lines[i].Contains("0"))
                {
                    string substr = Findsubstring(lines[i], "IndexOf(", ")", false);
                    if (substr.StartsWith("\"") || substr.StartsWith("\'")) //checking if the first parameter(IndexOf) is a string
                    {
                        lines[i] = lines[i].Replace("IndexOf", "Contains").Replace(">", "").Replace("0", "");
                    }
                    else //first parameter(IndexOf) is list or Array 
                    {
                        lines[i] = lines[i].Replace("0", "-1");
                    }
                }
            }
            return lines;
        }
        public string[] RSPEC2251(string[] lines)
        {
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].TrimStart().StartsWith("for"))
                {
                    //to get the condition substring within the for loop       
                    string substr = Findsubstring(lines[i], "(", ")", true);
                    //Splitting the substring by ; separator
                    string[] arr = substr.Split(';');
                    if ((arr[0].Contains("=") && arr[0].Contains("0")) && arr[2].Contains("--"))
                        lines[i] = lines[i].Replace("--", "++");
                }
            }
            return lines;
        }
        public string[] RSPEC2223(string[] lines)
        {

            List<string> datatype_list1 = new List<string> { "int", "float", "string", "double", "char", "bool", "long" };
            List<string> datatype_list2 = new List<string> { "new", "[]", "<>" };
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("public static"))
                {
                    if (!(lines[i].EndsWith("{") || lines[i].EndsWith("}") || lines[i + 1].TrimStart().StartsWith("{") || lines[i].Contains("public static readonly")))  //Discarding function definitions and line containing "public static readonly" 
                    {
                        string[] arr = lines[i].Split(' ');
                        for (int j = 0; j < arr.Length; j++)
                        {
                            if (datatype_list1.Any(x => arr[j].Contains(x)))
                            {
                                lines[i] = lines[i].Replace("static", "const");
                                break;
                            }
                            if (datatype_list2.Any(x => arr[j].Contains(x)))
                            {
                                lines[i] = lines[i].Replace("static", "static readonly");
                                break;
                            }
                        }
                    }
                }
            }
            return lines;
        }
        public string[] RSPEC2328(string[] lines)
        {
            int f;
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains(".GetHashCode()"))
                {
                    f = 0;
                    string substr = Findsubstring(lines[i], "this.", ".GetHashCode()", false);
                    for (int j = i - 1; j >= 0; j--) //checking whether the field is readonly or not
                    {
                        if (lines[j].Contains("readonly") && lines[j].Contains(substr))
                        {
                            f = 1;
                            break;
                        }
                    }
                    if (f == 0)
                        lines[i] = lines[i] + " //Noncompliant : The field should be readonly";
                }
            }
            return lines;
        }
        public string[] RSPEC1168(string[] lines)
        {
            int count, f = 0, j;
            List<string> fundef = new List<string>();
            for (int i = 0; i < lines.Count(); i++)
            {
                count = i + 1;
                if (count < lines.Count() && lines[count].TrimStart().StartsWith("{"))
                {
                    fundef = lines[i].Split(' ').ToList();
                    fundef.RemoveAll(y => y.Contains("(") && y.Contains(")"));
                    if (fundef.Any(x => x.Contains("[]")) || (fundef.Any(x => x.Contains("<")) && fundef.Any(x => x.Contains(">"))))
                    {
                        f = 0;
                        for (j = i; j < lines.Count() && f == 0; j++)
                        {
                            if (lines[j].Contains("return") && lines[j].Contains("null"))
                                f = 1;
                            else if (lines[j].Contains("return") && !lines[j].Contains("null"))
                            {
                                if (lines[j + 1].TrimStart().Equals("}"))
                                    break;
                            }
                        }
                        if (f == 1)
                            lines[j - 1] = lines[j - 1].Replace("null", Replacefun(fundef));
                    }
                }
                if ((lines[i].Contains("{") && lines[i].Contains("}")) || (lines[i].Contains("=>") && lines[i].Contains("null")))
                {
                    fundef = lines[i].Split(' ').ToList();
                    fundef.RemoveAll(y => y.Contains("(") && y.Contains(")"));
                    if (fundef.Any(x => x.Contains("[]")) || (fundef.Any(x => x.Contains("<")) && fundef.Any(x => x.Contains(">"))))
                    {
                        if (lines[i].Contains("return null;") || lines[i].Contains("null;"))
                        {
                            string text = Replacefun(fundef);
                            lines[i] = lines[i].Replace("null", text);
                        }
                    }
                }
            }
            return lines;
        }
        public string[] RSPEC3256(string[] lines)
        {
            string secondpara = "";
            List<string> checks_condition = new List<string> { "while", "if", "switch" };
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("\"\".Equals("))
                {
                    Console.WriteLine("entered string part1:" + lines[i]);
                    secondpara = Findsubstring(lines[i], ".Equals(", ")", false);
                    Console.WriteLine("secondpara:" + secondpara);
                    lines[i] = lines[i].Replace("\"\".Equals(" + secondpara + ")", "(" + secondpara + "!= null && " + secondpara + ".Length > 0)");
                    Console.WriteLine("result:" + lines[i]);
                }
                if (lines[i].Contains(".Equals(\"\")") || lines[i].Contains(".Equals(string.Empty)"))
                {
                    secondpara = Findsubstring(lines[i], ".Equals(", ")", false);
                    string updatedtext = lines[i].Replace(secondpara, " ");
                    //finding the first parameter of Equals function
                    if (checks_condition.Any(x => lines[i].Contains(x)))
                    {
                        string substr = Findsubstring(updatedtext, "(", ")", true);
                        List<string> list1 = substr.Split(' ').ToList();
                        string Equalscondition = list1.FirstOrDefault(t => t.Contains(".Equals("));
                        List<string> list2 = Equalscondition.Split('.').ToList();
                        string firstpara = Regex.Replace(list2[0], "[!(&|]", "");
                        lines[i] = lines[i].Replace(firstpara + ".Equals(" + secondpara + ")", "string.IsNullOrEmpty(" + firstpara + ")");
                    }
                }
            }
            return lines;
        }
        public string[] RSPEC1698(string[] lines)
        {
            List<string> templates = new List<string>();
            string firstpara = "", secondpara = "";
            int index, j, f = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("public override bool Equals(object") || lines[i].Contains("public override bool Equals(System.Object"))
                {
                    for (j = i - 1; j >= 0; j--)
                    {
                        if (lines[j].Contains("class"))
                        {
                            string modifiedtext = Regex.Replace(lines[j], "[:,]", "");
                            List<string> Splitlist = modifiedtext.Split(' ').ToList();
                            Splitlist.RemoveAll(string.IsNullOrWhiteSpace);
                            int startindex = (Splitlist.FindIndex(a => a.Contains("class"))) + 1;
                            for (int x = startindex; x < Splitlist.Count; x++)
                                templates.Add(Splitlist[x]);
                        }
                    }
                }
                if (templates.Count != 0 && lines[i].Contains("=="))
                {
                    string condition = Findsubstring(lines[i], "(", ")", true);
                    string removespace = RemoveWhiteSpace(condition, "==");
                    List<string> conditionSplit = removespace.Split(' ').ToList();
                    index = conditionSplit.FindIndex(x => x.Contains("=="));
                    List<string> SplitbyEquality = Regex.Split(conditionSplit[index], "==").ToList();
                    firstpara = Regex.Replace(SplitbyEquality[0], "[!()&|]", "");
                    secondpara = Regex.Replace(SplitbyEquality[1], "[!()&|]", "");
                    f = 0;
                    for (j = i - 1; j >= 0 && f == 0; j--)
                    {
                        if ((templates.Any(x => lines[j].Contains(x)) && lines[j].Contains(firstpara)) || (templates.Any(x => lines[j].Contains(x)) && lines[j].Contains(secondpara)))
                        {
                            lines[i] = lines[i].Replace(firstpara, "").Replace(secondpara, "").Replace("==", "object.Equals(" + firstpara + "," + secondpara + ")");
                            f = 1;
                        }
                    }
                }
            }
            return lines;
        }
        public string[] RSPEC3449(string[] lines)
        {
            string secondpara, removespace = "", operand = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("<<") || lines[i].Contains("<<=") || lines[i].Contains(">>") || lines[i].Contains(">>="))
                {
                    if (lines[i].Contains("<<"))
                    {
                        removespace = RemoveWhiteSpace(lines[i], "<<");
                        operand = "<<";
                    }
                    if (lines[i].Contains("<<="))
                    {
                        removespace = RemoveWhiteSpace(lines[i], "<<=");
                        operand = "<<=";
                    }
                    if (lines[i].Contains(">>"))
                    {
                        removespace = RemoveWhiteSpace(lines[i], ">>");
                        operand = ">>";
                    }
                    if (lines[i].Contains(">>="))
                    {
                        removespace = RemoveWhiteSpace(lines[i], ">>=");
                        operand = ">>=";
                    }

                    List<string> SplitbySpace = removespace.Split(' ').ToList();
                    int index = SplitbySpace.FindIndex(x => x.Contains(operand));
                    List<string> SplitbyShiftop = Regex.Split(SplitbySpace[index], operand).ToList();
                    secondpara = Regex.Replace(SplitbyShiftop[1], "[!()&|;]", "");
                    if (!(int.TryParse(secondpara, out int value)))
                        lines[i] = lines[i] + "// Noncompliant : right operand needs to be int";
                }
            }
            return lines;
        }
        public string[] RSPEC107(string[] lines)
        {
            int count;
            List<string> parameterlist = new List<string>();
            string parametersubstring;
            for (int i = 0; i < lines.Count(); i++)
            {
                count = i + 1;
                if ((count < lines.Count() && lines[count].TrimStart().StartsWith("{")) || (lines[i].Contains("{") && lines[i].Contains("}")))
                {
                    if (lines[i].Contains("(") && lines[i].Contains(")"))
                    {
                        parametersubstring = Findsubstring(lines[i], "(", ")", true);
                        parameterlist = parametersubstring.Split(',').ToList();
                        if (parameterlist != null)
                        {
                            if (parameterlist.Count > 4)  //maximum parameters = 4
                                lines[i] = lines[i].Replace(parameterlist[parameterlist.Count - 1], "").Replace(",)", ")"); ;
                        }
                    }
                }
            }
            return lines;
        }

        public string[] RSPEC1481(string[] lines) //22/10/2020
        {
            List<string> datatypes = new List<string>() { "int", "string", "float", "char", "string[]", "int[]", "var" };
            string expression = string.Empty;
            int ins = 0;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("=") && !lines[line].Contains("//") && datatypes.Any(x => lines[line].Substring(0, lines[line].IndexOf("=")).Contains(x)) && !lines[line].Contains("for") && lines[line][lines[line].IndexOf("=") + 1] != '>')
                {
                    string test = lines[line];
                    int count_expression = 0;
                    string start = string.Empty;
                    string oi = datatypes.Find(x => lines[line].Contains(x));
                    int index_datatype = lines[line].IndexOf(oi);
                    int index_blank = lines[line].IndexOf(" ", index_datatype);
                    int index_equals = lines[line].IndexOf("=");
                    expression = lines[line].Substring(index_blank + 1, index_equals - index_blank - 1).Trim();
                    //string eo = lines[line];
                    for (int j = line + 1; j < lines.Count(); j++)
                    {
                        ins = lines[j].IndexOf(expression);
                        if (lines[j].Contains(expression))
                        {
                            if (lines[ins + 1] != "=" || lines[ins + 2] != "=")
                            {
                                string ep_test = lines[j];
                                count_expression++;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    if (count_expression == 0)
                    {
                        lines[line] = lines[line].Replace(lines[line], lines[line] + "// This is dead code and should be removed as it is not used anywhere");
                    }
                }
            }
            return lines;
        }

        public string[] RSPEC1066(string[] lines) // 22/10/2020
        {
            int brace_index = 0;
            int index_parent = 0;
            int else_marked = 0;
            int marked = -1;
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("if") && marked == -1 && lines[i].Contains("(") && (!Char.IsLetter(lines[i][lines[i].IndexOf("if") - 1]) || lines[i][lines[i].IndexOf("if") - 1] == ' ' || lines[i][lines[i].IndexOf("if") - 1] == '\t'))
                {
                    //char ch = lines[i][lines[i].IndexOf("if") - 1];
                    //bool checks = Char.IsLetter(lines[i][lines[i].IndexOf("if") - 1]);
                    string express = string.Empty;
                    string brace = lines[i + 1];
                    brace_index = 0;

                    int if_index = 0;
                    if (lines[i + 2].Contains("if") && (!Char.IsLetter(lines[i + 2][lines[i].IndexOf("if") - 1]) || lines[i + 2][lines[i].IndexOf("if") - 1] == ' ' || lines[i + 2][lines[i].IndexOf("if") - 1] == '\t'))
                    {
                        brace_index++;
                        for (int j = i + 2; j < lines.Count(); j++)
                        {
                            //string test = lines[j];
                            if (brace_index == 0)
                            {
                                break;
                            }
                            if (if_index > 1)
                            {
                                break;
                            }
                            if (lines[j].Contains("}"))
                            {
                                brace_index--;
                            }
                            if (lines[j].Contains("{"))
                            {
                                brace_index++;
                            }
                            if (lines[j].Contains("if"))
                            {
                                if_index++;
                            }
                            if (lines[j].Contains("else"))
                            {
                                else_marked = 1;
                            }
                            if (lines[j].Contains("try"))
                            {
                                else_marked = 1;
                            }
                            express += lines[j] + "\n";
                        }
                        if (if_index > 1 || if_index == 0 || else_marked == 1)
                        {
                            else_marked = 0;
                            continue;
                        }
                        else
                        {
                            int index = express.IndexOf(")");
                            string if_expression = express.Substring(express.IndexOf("if") + 2, index - (express.IndexOf("if") + 2) + 1);
                            if_expression = System.Text.RegularExpressions.Regex.Replace(if_expression, @"\t|\n|\r", "");
                            int hello = lines[i].LastIndexOf(")");
                            //lines[i][hello] = ':';
                            lines[i] = lines[i].Remove(hello, 1).Insert(hello, " && " + if_expression + ")) // Fixed: Merged Nested If to one single if to improve user readability");
                            //lines[i] = lines[i].Replace(")", " && "+if_expression + ") // Fixed: Merged Nested If to one single if to improve user readability");
                            marked = 0;
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (lines[i].Contains("{") && marked == 0 && index_parent == 1)
                {
                    index_parent++;
                    lines[i] = lines[i].Replace(lines[i], string.Empty);
                }
                if (lines[i].Contains("{") && marked == 0 && lines[i - 1].Contains("if"))
                {
                    index_parent++;
                }
                if (lines[i].Contains("if") && marked == 0)
                {
                    lines[i] = lines[i].Replace(lines[i], string.Empty);
                }
                if (lines[i].Contains("}") && marked == 0)
                {
                    index_parent--;
                    if (index_parent == 0)
                    {
                        marked = -1;
                    }
                    else
                    {
                        lines[i] = lines[i].Replace(lines[i], string.Empty);
                        //index_parent--;
                    }
                }

            }
            return lines;
        }
        public string[] RSPEC1118(string[] lines) //26/20/2020
        {
            int static_methods = 0;
            int total_methods = 0;
            int paranthesis = 1;
            List<string> datatypes = new List<string>() { "int", "string", "float", "char", "string[]", "int[]", "List" };
            List<string> access_types = new List<string>() { "public", "protected", "private" };
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("class") && !lines[line].Contains("static"))
                {
                    for (int i = line + 2; i < lines.Count(); i++)
                    {
                        if (paranthesis == 0)
                        {
                            paranthesis = 1;
                            break;
                        }
                        if (lines[i].Contains("{"))
                        {
                            paranthesis++;
                        }
                        if (lines[i].Contains("}"))
                        {
                            paranthesis--;
                        }
                        if (access_types.Any(x => lines[i].Contains(x)) && lines[i].Contains("(") && (lines[i + 1].Contains("{") || lines[i].Contains("{")) && datatypes.Any(x => lines[i].Substring(0, lines[i].IndexOf("(")).Contains(x)))
                        {
                            total_methods++;
                        }
                        if (access_types.Any(x => lines[i].Contains(x)) && lines[i].Contains("(") && lines[i].Contains("static") && (lines[i + 1].Contains("{") || lines[i].Contains("{")) && datatypes.Any(x => lines[i].Substring(0, lines[i].IndexOf("(")).Contains(x)))
                        {
                            static_methods++;

                        }

                    }
                    if (total_methods == static_methods && total_methods != 0)
                    {
                        lines[line] = lines[line].Replace("class", "static class");
                        lines[line] = lines[line] + " // FIXED: Class with all static methods must not be instantiated";
                    }
                    else
                    {
                        total_methods = 0;
                        static_methods = 0;
                        continue;
                    }
                }

            }

            return lines;
        }
        public string[] RSPEC1301(string[] lines) // 26/10/2020
        {
            int paranthesis = 1;
            int number_cases = 0;
            int x = 0;
            string switch_variable;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("switch") && !lines[line].Contains("//"))
                {
                    switch_variable = lines[line].Substring(lines[line].IndexOf("(") + 1).Replace(")", "").Trim();
                    for (int i = line + 2; i < lines.Count(); i++)
                    {
                        if (paranthesis == 0)
                        {
                            paranthesis = 1;
                            x = i;
                            break;
                        }
                        if (lines[i].Contains("{"))
                        {
                            paranthesis++;
                        }
                        if (lines[i].Contains("}"))
                        {
                            paranthesis--;
                        }
                        if (lines[i].Contains("case"))
                        {
                            number_cases++;
                        }
                    }
                    if (number_cases > 2)
                    {
                        number_cases = 0;
                        continue;
                    }
                    else
                    {
                        for (int j = line; j < x; j++)
                        {
                            if (lines[j].Contains("switch") || lines[j].Contains("{") || lines[j].Contains("}"))
                            {
                                lines[j] = string.Empty;
                            }
                            if (lines[j].Contains("case"))
                            {
                                lines[j] = lines[j].Replace("case", "if(" + switch_variable + " == ");
                                lines[j] = lines[j].Replace(":", ") // FIXED: Converted the switch case to if case to increase user understandability(Switch case should have atleast 3 cases) \n{\n");
                            }
                            if (lines[j].Contains("break"))
                            {
                                lines[j] = lines[j].Replace("break;", "}\n");
                            }
                            if (lines[j].Contains("default"))
                            {
                                lines[j] = lines[j].Replace("default:", "else\n{\n");
                            }
                        }
                    }
                }

            }
            return lines;
        }

        public string[] RSPEC1871(string[] lines)
        {
            int paranthesis = 1;
            List<string> case_definitions = new List<string>();
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("switch") && !lines[line].Contains("//"))
                {
                    string str = string.Empty;
                    for (int i = line + 2; i < lines.Count(); i++)
                    {
                        if (paranthesis == 0)
                        {
                            paranthesis = 1;
                            break;
                        }
                        else if (lines[i].Contains("{"))
                        {
                            paranthesis++;
                        }
                        else if (lines[i].Contains("}"))
                        {
                            case_definitions.Add(str);
                            str = string.Empty;
                            paranthesis--;
                        }
                        else if (lines[i].Contains("case"))
                        {
                            if (str != string.Empty)
                            {
                                case_definitions.Add(str);
                                str = lines[i].Substring(lines[i].IndexOf(":") + 1);
                            }
                            else
                            {
                                str += lines[i].Substring(lines[i].IndexOf(":") + 1);
                            }
                        }
                        else if (lines[i].Contains("default"))
                        {
                            if (str != string.Empty)
                            {
                                case_definitions.Add(str);
                                str = lines[i].Substring(lines[i].IndexOf(":") + 1);
                            }
                            else
                            {
                                str += lines[i].Substring(lines[i].IndexOf(":") + 1);
                            }
                        }
                        else if (lines[i].Contains("break"))
                        {
                            case_definitions.Add(str);
                            str = string.Empty;


                        }
                        else
                        {
                            str += lines[i].Trim();
                        }
                    }
                    if (case_definitions.Count != case_definitions.Distinct().Count())
                    {
                        lines[line] = lines[line] + " // NonCompliant: The Switch cases have duplicate implementations";
                        case_definitions.Clear();
                    }
                }

            }
            return lines;
        }
        public string[] SecureModeandPaddingScheme(string[] lines)
        {
            string objsearchstring = "AesManaged";
            string objsearchstring1 = "RSACryptoServiceProvider";
            string objaes = "";
            string objrsa = "";
            for (int line = 0; line < lines.Count(); line++)
            {
                int flag = 0;
                if (lines[line].Contains(objsearchstring1))
                {
                    int index = lines[line].IndexOf(objsearchstring1);
                    index += 25;
                    int eqindex = lines[line].IndexOf("=");
                    int len = eqindex - index;
                    objrsa = lines[line].Substring(index, len);
                    objrsa = objrsa.Trim();
                }
                if (objrsa != "")
                {
                    if (lines[line].Contains(objrsa + ".Encrypt"))
                    {
                        int index = lines[line].IndexOf(".Encrypt");
                        index += 8;
                        int eqindex = lines[line].Length;
                        int len = eqindex - index;
                        string expectedstring = lines[line].Substring(index, len).Trim();
                        expectedstring = expectedstring.Replace(" ", "");
                        var param = expectedstring.Split(',');
                        if (param[1].Contains("false"))
                        {
                            string str = lines[line];
                            lines[line] = str.Replace("false", "true");
                            lines[line] += " // FIXED, RSA encryption algorithm should be used with OAEP padding scheme by setting second param to true";
                        }

                    }
                }
                if (lines[line].Contains(objsearchstring))
                {
                    int index = lines[line].IndexOf(objsearchstring);
                    index += 10;
                    int eqindex = lines[line].IndexOf("=");
                    int len = eqindex - index;
                    objaes = lines[line].Substring(index, len);
                    objaes = objaes.Trim();
                }
                if (objaes != "")
                {
                    if (lines[line].Contains(objaes + ".Mode"))
                    {
                        var expstring = lines[line].Trim();
                        if (expstring.Contains("CipherMode.ECB"))
                        {
                            flag = 1;
                            lines[line] += " // Noncomplaint, ECB mode is vulnerable because it doesn't provide serious message confidentiality. \n" + "/*Try to use GCM mode under AES encription technique like --->\n var aesGcm = new AesGcm(key);\n or \nGcmBlockCipher blockCipher = new GcmBlockCipher(new AesFastEngine());\nblockCipher.Init(true, new AeadParameters(new KeyParameter(secretKey), 128, iv, null));*/";
                        }
                        if (expstring.Contains("CipherMode.CBC"))
                        {
                            line++;
                            if (lines[line].Contains(objaes + ".Padding"))
                            {

                                int eqindex = lines[line].IndexOf("=");
                                int index = lines[line].IndexOf(";");
                                int len = index - eqindex;
                                string paddingmode = lines[line].Substring(eqindex + 1, len);
                                if (paddingmode.Contains("PaddingMode.PKCS7") || paddingmode.Contains("PaddingMode.PKCS5"))
                                {
                                    lines[line] += " // Noncomplaint, Cipher Block Chaining (CBC) mode with PKCS#5 padding (or PKCS#7) is susceptible to padding oracle attacks.";
                                }

                            }
                        }
                    }
                }
                if (lines[line].Contains("CipherMode.ECB") && flag != 1)
                {
                    string str = lines[line];
                    str = str.Replace(" ", String.Empty);
                    if (str.Contains("Mode=CipherMode.ECB"))
                    {
                        lines[line] += " // Noncomplaint, ECB mode is vulnerable because it doesn't provide serious message confidentiality. \n" + "/*Try to use GCM mode under AES encription technique like --->\n var aesGcm = new AesGcm(key);\n or \nGcmBlockCipher blockCipher = new GcmBlockCipher(new AesFastEngine());\nblockCipher.Init(true, new AeadParameters(new KeyParameter(secretKey), 128, iv, null));*/";
                    }
                }
                if (lines[line].Contains("Padding"))
                {

                    string str = lines[line];
                    str = str.Replace(" ", String.Empty);
                    if (str.Contains("Padding=PaddingMode.PKCS7") || str.Contains("Padding=PaddingMode.PKCS"))
                    {
                        lines[line] += " // Noncomplaint, Cipher Block Chaining (CBC) mode with PKCS#5 padding (or PKCS#7) is susceptible to padding oracle attacks.";
                    }
                }

            }
            return lines;
        }
        public string[] CipherAlgoShouldbeRobust(string[] lines)
        {
            string searchstring = "TripleDESCryptoServiceProvider()";
            string searchstring1 = "DESCryptoServiceProvider()";
            string searchstring2 = "RC2CryptoServiceProvider()";

            for (int line = 0; line < lines.Count(); line++)
            {
                int flag = 0;
                if (lines[line].Contains(searchstring))
                {
                    lines[line] += " // Noncompliant: Triple DES is vulnerable to meet-in-the-middle attack. \n\t\t/*<==TRY THIS==>\n\t\tvar AES = new AesCryptoServiceProvider();*/";
                    flag = 1;
                }
                if (lines[line].Contains(searchstring1) && flag != 1)
                {
                    lines[line] += " // Noncompliant: DES works with 56-bit keys allow attacks via exhaustive search. \n\t\t/*<==TRY THIS==>\n\t\tvar AES = new AesCryptoServiceProvider();*/";
                }
                if (lines[line].Contains(searchstring2))
                {
                    lines[line] += " // Noncompliant: RC2 is vulnerable to a related-key attack. \n\t\t/*<==TRY THIS==>\n\t\tvar AES = new AesCryptoServiceProvider();*/";
                }

            }
            return lines;
        }
        public string[] CheckHttp(string[] var)
        {
            int flag = 0;
            string objsearchstring = "HttpCookie";
            string objaes = "";
            string objrsa = "";
            int indexorig = 0;
            //int ctr = var.Count();
            for (var i = 0; i < var.Count(); i++)
            {
                if (var[i].Contains(objsearchstring))
                {
                    indexorig = i;
                    int index = var[i].IndexOf(objsearchstring);
                    index += 10;
                    int eqindex = var[i].IndexOf("=");
                    int len = eqindex - index;
                    objaes = var[i].Substring(index, len);
                    objaes = objaes.Trim();
                }
                if (objaes != "")
                {
                    if (var[i].Contains(objrsa + ".HttpOnly"))
                    {
                        int index = var[i].IndexOf("=");
                        int index1 = var[i].IndexOf(";");
                        string prev = var[i];
                        string subbed = var[i].Substring(index + 1, index1 - index - 1);
                        subbed = subbed.Replace(" ", "");
                        if (subbed == "false")
                        {
                            string str = var[i];
                            var[i] = str.Replace(subbed + ";", "true; // Fixed:this sensitive cookie was created with the httponly flag set to false and so it can be stolen easily in case of XSS vulnerability thus it has been set to true to improve the security");
                        }
                        if (subbed == "true")
                        {
                            var[i] += "// Compliant: the sensitive cookie is protected against theft thanks to the HttpOnly property set to true (HttpOnly = true)";
                        }
                    }
                    else
                    {
                        flag++;
                    }
                }
            }
            if (flag == var.Count() - indexorig)
            {
                var[indexorig] += "\n\t" + objaes + ".HttpOnly= true; // Fixed:The object of HttpCookie was not defined in the program, Thus added the value to improve security";
                return var;
            }
            else
            {
                return var;
            }
        }
        public string[] MD5andSHABasedAlgos(string[] var)
        {
            string objsearchstring = "MD5CryptoServiceProvider()";
            string objsearchstring1 = "(HashAlgorithm)CryptoConfig.CreateFromName(\"MD5\")";
            string objsearchstring2 = "SHA1Managed()";
            string objsearchstring3 = "HashAlgorithm.Create(\"SHA1\")";
            string objaes = "";
            for (var i = 0; i < var.Count(); i++)
            {
                if (var[i].Contains(objsearchstring) || var[i].Contains(objsearchstring1) || var[i].Contains(objsearchstring2) || var[i].Contains(objsearchstring3))
                {
                    int ctr = 0;
                    int eqindex = var[i].IndexOf("=");
                    string help = var[i].Substring(0, eqindex);
                    int p = 0;
                    for (int j = eqindex - 1; j >= 0; j--)
                    {
                        if (help[j] == ' ' && j != eqindex - 1)
                        {
                            ctr = j;
                            break;
                        }
                    }
                    int index = ctr;
                    int len = eqindex - index;
                    objaes = var[i].Substring(index, len);
                    objaes = objaes.Trim();
                    var[i] += "//Noncompliant \n\t/* Below are the compliant solutions that can be used- \n\t var " + objaes + "= new SHA256Managed();\n\t var " + objaes + "= (HashAlgorithm)CryptoConfig.CreateFromName(\"SHA256Managed\");\n\t var " + objaes + "= HashAlgorithm.Create(\"SHA256Managed\");*/";
                }
            }
            return var;
        }
        public string[] CodeInjection(string[] var)
        {
            string objsearchstring = "Request.QueryString";
            List<string> parameters = new List<string>(10);
            parameters.Add("int");
            parameters.Add("string");
            parameters.Add("char");
            parameters.Add("float");
            parameters.Add("decimal");
            parameters.Add("class");
            parameters.Add("public");
            parameters.Add("return");
            parameters.Add("List");
            parameters.Add("bool");
            List<string> parameters1 = new List<string>(3);
            parameters1.Add("Append");
            parameters1.Add("Replace");
            parameters1.Add("Insert");
            string objaes = "";
            string obj = "";
            int indexif = 0;
            int indexparam = -1;
            int ctr = 0;
            for (var i = 0; i < var.Count(); i++)
            {
                if (var[i].Contains(objsearchstring))
                {
                    //indexorig = i;
                    var[i] += "//KINDLY MAKE SURE THAT THIS USER INPUT/QUERY PARAMETER IS VALIDATED IN THE PROGRAM SO THAT ANY MALICIOUS INPUT BY A USER MAY NOT HARM THE APPLICATION";
                    if (indexparam == -1)
                        indexparam = i;
                    int index = var[i].IndexOf(" ");
                    //index += 10;
                    int eqindex = var[i].IndexOf("=");
                    int len = eqindex - index;
                    string test = "";
                    test = var[i].Substring(index, len);
                    test = test.Trim();
                    if (index == 0)
                    {
                        index = test.IndexOf(" ");
                        //eqindex = test.IndexOf("=");
                        len = test.Length - index;
                        objaes = test.Substring(index, len);
                        objaes = objaes.Trim();
                    }
                    else
                    {
                        objaes = test;
                    }

                    //var[i] += "//Noncompliant \n\t/* Below are the compliant solutions that can be used- \n\t var " + objaes + "= new SHA256Managed();\n\t var " + objaes + "= (HashAlgorithm)CryptoConfig.CreateFromName(\"SHA256Managed\");\n\t var " + objaes + "= HashAlgorithm.Create(\"SHA256Managed\");*/";
                }
                if (objaes != "")
                {
                    if (var[i].Contains("StringBuilder"))
                    {
                        int index = var[i].IndexOf("StringBuilder");
                        //index += 10;
                        index = var[i].IndexOf(" ", index);
                        int eqindex = var[i].IndexOf("=");
                        int len = eqindex - index;
                        //string test = "";
                        obj = var[i].Substring(index, len);
                        obj = obj.Trim();
                        //if (index == 0)
                        //{
                        //    index = test.IndexOf(" ");
                        //    //eqindex = test.IndexOf("=");
                        //    len = test.Length - index;
                        //    obj = test.Substring(index, len);
                        //    obj = obj.Trim();
                        //}
                        //else
                        //{
                        //    obj = test;
                        //}
                    }
                    if (var[i].Contains(objaes))
                    {
                        int index = var[i].IndexOf(objaes);
                        if (var[i].Contains("if"))
                        {
                            int indexpara1 = var[i].IndexOf("(");
                            int indexpara2 = var[i].IndexOf(")");
                            if ((index > indexpara1) && (index < indexpara2))
                            {
                                indexif = var[i].IndexOf("if");
                            }
                        }
                        if (parameters1.Any(str => var[i].Contains(obj + "." + str)))
                        {
                            int indexpara1 = var[i].IndexOf("(");
                            int indexpara2 = var[i].IndexOf(")");
                            if ((index > indexpara1) && (index < indexpara2) && parameters.Any(str => var[i].Contains(str)))
                            {
                                if (indexif == 0)
                                {
                                    var[i] += "// NONCOMPLIANT!! KINDLY DONOT TRY TO USE UNVALIDATED USER INPUTS AND ALSO DONOT TRY TO CREATE DYNAMIC CODES IN THE PROGRAM AS IT SEVERELY INCRESES THE RISK OF CODE INJECTION ATTACKS";
                                }
                            }
                        }
                    }
                }
            }
            if (indexif == 0 && indexparam != -1)
            {
                String vam = var[indexparam];
                int colonindex = vam.IndexOf(";");
                var[indexparam] = vam.Replace(vam.Substring(colonindex), ";// NONCOMPLIANT ! THIS USER INPUT HAS NOT BEEN VALIDATED IN THE PROGRAM ! VALIDATE THE QUERY PARAMETER/USER INPUT SO THAT YOU ARE SURE THE DATA IS EXACTLY WHAT YOU WANT AND NOT MALICIOUS DATA THAT COULD DAMAGE THE APPLICATION");
            }
            return var;
        }
        public string[] HttpRequestValidationShouldbeEnabled(string[] lines)
        {
            string searchstring = "[HttpPost]";
            string searchstring1 = "[ValidateInput(false)]";


            for (int line = 0; line < lines.Count(); line++)
            {
                int liner = 0;
                int linew = 0;
                int flag = 0;
                int lib = 0;
                if (lines[line].Contains(searchstring))
                {
                    liner = line;
                    for (int linex = liner; linex < lines.Count(); linex++)
                    {

                        if (lines[linex].Contains("public") || lines[linex].Contains("private") || lines[linex].Contains("protected"))
                        {
                            if (lines[linex].Contains("(") && lines[linex].Contains(")"))
                            {
                                linew = linex;
                                string str = "";
                                int stindex = lines[linex].IndexOf('(');
                                stindex++;
                                int endindex = lines[linex].IndexOf(')');
                                if (endindex >= stindex)
                                {
                                    int len = endindex - stindex;
                                    string paramslist = lines[linex].Substring(stindex, len);
                                    paramslist = paramslist.Trim();
                                    str = paramslist.Replace(" ", "");

                                    if (str.Length > 0)
                                    {

                                        for (int lineT = liner; lineT < linew; lineT++)
                                        {
                                            if (lines[lineT].Contains(searchstring1) && flag != 1)
                                            {

                                                flag = 1;
                                                string qstr = lines[lineT];
                                                lines[lineT] = qstr.Replace("false", "true");
                                                lines[lineT] += " // FIXED, validate HTTP requests to prevent potentially dangerous content to perform a cross-site scripting (XSS) attack";

                                            }

                                        }


                                    }
                                    else
                                    {
                                        lib = 1;
                                        break;
                                    }
                                }
                            }

                        }

                    }
                    if (flag != 1 && linew != 0 && lib == 0)
                    {
                        lines[liner] += " // Noncomplaint \n[ValidateInput(true)]  // FIXED, added this code snippet to validate HTTP requests to prevent from a cross-site scripting (XSS) attack";

                    }
                }
            }
            return lines;
        }

        private string[] MakeProperties(string[] lines)
        {
            string searchPublic = "public";
            var foundPublic = lines.Where(x => x.Contains(searchPublic));
            if (foundPublic.Count() > 0)
            {
                foreach (var item in foundPublic)
                {
                    if (!item.Contains("("))
                    {
                        if (item.Contains(";"))
                        {
                            var words = item.Trim().Split(' ');
                            var type = words[1];
                            var name = words[2].Replace(';', ' ');
                            var fname = name.First().ToString().ToUpper() + name.Substring(1);
                            var property =
                                "public  " + type + " " + fname + "\n" +
                                "{\n   " +
                "get { return " + name + "; }\n   " +
                "set { " + name + " = value; }\n}";

                            lines = lines.Select(s => s != item ? s : property).ToArray();
                        }
                    }
                }
            }

            return lines;
        }

        private static string[] ChangedGuid(string[] lines)
        {
            string searchguid = "new Guid()";
            var foundguid = lines.Where(x => x.Contains(searchguid));
            if (foundguid.Count() > 0)
            {

                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(searchguid))
                    {

                        string systax = lines[line];
                        lines[line] = systax.Replace(@"new Guid()", "Guid.Empty") + " //Avoid using Guid struct instead used Guid.NewGuid() or new Guid(bytes). FIXED.";

                        line++;
                    }
                }
            }
            return lines;
        }


        private static string[] OptionalParam(string[] lines)
        {


            int linecount = 0;

            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("[Optional]") && lines[line].Contains("ref"))
                {

                    lines[line] += " The use of ref in combination with [Optional] is both confusing and contradictory. [Optional] indicates that the parameter doesn't have to be provided, while ref mean that the parameter will be used to return data to the caller";

                }
                if (lines[line].Contains("[Optional]") && lines[line].Contains("out"))
                {

                    lines[line] += " The use of out in combination with [Optional] is both confusing and contradictory. [Optional] indicates that the parameter doesn't have to be provided, while out mean that the parameter will be used to return data to the caller";

                }



            }
            return lines;
        }

        private static string[] Nestedempty(string[] lines)
        {
            string openbracket = "(";
            string closingbracket = ")";
            int linecount = 0;

            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("for"))
                {
                    linecount = line;
                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains(openbracket) && lines[line].Contains(closingbracket))
                        {
                            line++;
                            if (lines[line].Contains("{"))
                            {


                                linecount = line + 1;
                                for (line = linecount; line < lines.Count(); line++)
                                {
                                    if (lines[line].Contains("\n") || lines[line].Contains(" ") || lines[line].Contains("\t"))
                                    {
                                        if (lines[line].Contains("  "))
                                        { continue; }

                                    }
                                    else if (lines[line].Contains("}"))
                                    {
                                        lines[linecount] += "//Nested blocks of code should not be left empty";
                                        break;
                                    }
                                    else
                                    { break; }
                                }
                            }

                        }
                        else { break; }
                    }
                }

            }



            return lines;
        }



        private static string[] Whitespace(string[] lines)
        {

            string searchEx = "string";
            string searchparan = "}";
            int linecount = 0;
            int count = 0;
            //var founditems = lines.Where(x => (x.Contains("\n") || x.Contains(" ") || x.Contains(null)));
            var foundEx = lines.Where(x => (x.Contains(searchEx)));


            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("string") && lines[line].Contains("="))
                {
                    linecount = line;
                    var chartest = lines[line].ToCharArray();
                    for (var char1 = 0; char1 < chartest.Count(); char1++)
                    {
                        if (chartest[char1] != ' ')
                        {
                            count++;
                            continue;

                        }
                        if (chartest[char1] == ' ')
                        {

                            lines[linecount] += String.Join("", chartest).Substring(0, char1) + "Whitespace and control characters in string literals should be explicit";
                            break;
                        }
                        else if (chartest[char1] == ';')
                        { break; }

                        else
                        { continue; }
                    }


                }

            }

            return lines;

        }

        private static string[] Assemblyload(string[] lines)
        {

            int linecount = 0;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("Assembly.LoadWithPartialName") || lines[line].Contains("Assembly.LoadFrom") || lines[line].Contains("Assembly.LoadFile"))
                {
                    lines[line] = lines[line] + " The parameter to Assembly.Load includes the full specification of the dll to be loaded. Use another method, and you might end up with a dll other than the one you expected.";

                }


            }
            return lines;

        }
        private void ManyConstructorsCheck(string[] lines)
        {
            var values12 = lines.Where(x => x.Contains("class "));
            string classnameis1 = string.Empty;
            foreach (var classname in values12)
            {
                var words = classname.Trim().Split(' ');
                classnameis1 = words[2].ToString();
            }
            var noofcons = lines.Where(x => x.Contains(classnameis1 + "("));

            if (noofcons.Count() > 5)
            {
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains("class "))
                    {
                        line++;
                        lines[line] = lines[line] + "\n //Classes should not have too many Constructors. FIXED.";
                        line = lines.Count();
                    }
                }
            }
        }

        private void BreakInLoop(string[] lines)
        {
            bool bCase = false;
            bool bForLoop = false;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("case "))
                {
                    bCase = true;
                }
                if (lines[line].Contains("for ("))
                {
                    bForLoop = true;
                    bCase = false;
                }
                if (lines[line].Contains("break;"))
                {
                    if (!bCase && bForLoop)
                    {
                        lines[line] = "// Removed break;  FIXED.";
                        bForLoop = false;
                        bCase = false;
                    }
                }
            }
        }

        //Avoid instantiations inside loop
        private void StringBuilderInLoop(string[] lines, string searchWord)
        {
            var foundSingBuilder = lines.Where(x => x.Contains(searchWord));
            if (foundSingBuilder.Count() > 0)
            {
                bool bForLoop = false;
                int foundOpenBracket = 0;
                int foundCloseBracket = 0;
                int forlocation = 0;
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains("for ("))
                    {
                        forlocation = line;
                        bForLoop = true;
                    }
                    if (lines[line].Contains("{"))
                    {
                        foundOpenBracket++;
                    }
                    if (lines[line].Contains("}"))
                    {
                        foundCloseBracket++;
                    }
                    if (lines[line].Contains(searchWord) && bForLoop)
                    {
                        if ((foundOpenBracket - foundCloseBracket) > 0)
                        {
                            lines[forlocation] = lines[line] + "\n" + lines[forlocation];
                            lines[line] = "// Removed StringBuilder -FIXED.";
                            bForLoop = false;
                        }
                    }
                }
            }
        }

        private string ThreadSleepInLookCheck(string[] lines)
        {
            string searchWord = "Thread.Sleep(";
            var foundThreadSleep = lines.Where(x => x.Contains(searchWord));
            if (foundThreadSleep.Count() > 0)
            {
                bool bForLoop = false;
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains("for ("))
                    {
                        bForLoop = true;
                    }
                    if (lines[line].Contains(searchWord) && bForLoop)
                    {
                        lines[line] = "// Removed sleep -FIXED.";
                        bForLoop = false;
                    }
                }
            }

            return searchWord;
        }

        private void ManualRefactorComments(string[] lines)
        {
            StringBuilder strSuggestions = new StringBuilder();
            string searchWord = "SqlConnection";
            if (objrulesconfig.LargeClass == "true")
            {
                strSuggestions.AppendLine("\n//Classes should not have too many Methods.");
            }
            if (objrulesconfig.LargeMethod == "true")
            {
                strSuggestions.AppendLine("//Please make sure to reduce the size of methods length.");
            }
            if (objrulesconfig.Unreferenced == "true")
            {
                strSuggestions.AppendLine("//Remove Artifacts that are not used.");
            }
            if (objrulesconfig.Formatter == "true")
            {
                strSuggestions.AppendLine("//Review all inputs. Don't use inputs in formatters.");
            }
            if (objrulesconfig.SQLConnection == "true")
            {
                var foundSQLConn = lines.Where(x => x.Contains(searchWord));
                if (foundSQLConn.Count() > 0)
                {
                    strSuggestions.AppendLine("//Please make sure to close the SQL connection within the same MethodClasses.");
                }
            }

            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("class "))
                {
                    line++;
                    lines[line] = lines[line] + strSuggestions.ToString();
                    line = lines.Count();
                }
            }
        }

        private string PrivateConstructor(string[] lines)
        {
            string searchWord = "static";
            var foundStatic = lines.Where(x => x.Contains(searchWord));

            foreach (var staticvar in foundStatic)
            {
                if (staticvar.Contains("("))
                {
                    var values7 = lines.Where(x => x.Contains("class "));
                    foreach (var classname in values7)
                    {
                        var words = classname.Trim().Split(' ');
                        var classnameis = "private " + words[2];
                        var values8 = lines.Where(x => x.Contains(classnameis));
                        if (values8.Count() <= 0)
                        {
                            for (int line = 0; line < lines.Count(); line++)
                            {
                                if (lines[line].Trim() == classname.Trim())
                                {
                                    line++;
                                    lines[line] = lines[line] + "\nprivate " + words[2].Trim() + "()\n{// Avoid instantiation of the class\n}";
                                    line = lines.Count();
                                }
                            }
                        }
                    }
                }
            }

            return searchWord;
        }

        //Avoid String concatenation in loops(.NET)
        private string[] StringConcatenationCheck(string stringconcet, string[] lines)
        {

            var values1 = lines.Where(x => x.Contains(stringconcet));
            foreach (var d in values1)
            {
                var dd = d.Trim();
                var words = dd.Split(' ');
                var variblename = words[0].Trim();
                var values2 = lines.Where(x => x.Contains("tring " + variblename.ToString() + " ="));
                if (values2.Count() > 0)
                {
                    var wordsLin1 = "StringBuilder " + variblename + " = new StringBuilder();//FIXED";
                    if (objrulesconfig.StringConcatenationInLoop == "true")
                    {
                        foreach (var ss in values2)
                        {
                            lines = lines.Select(s => s != ss ? s : wordsLin1).ToArray();
                        }
                    }
                    var values3 = lines.Where(x => x.Contains(variblename.ToString() + " +="));
                    if (objrulesconfig.LargeStringConcatenation == "true")
                    {
                        if (values3.Count() > 0)
                        {
                            foreach (var ss in values3)
                            {
                                int pFrom = ss.IndexOf("+=") + "+=".Length;
                                int pTo = ss.LastIndexOf(";");

                                String result = ss.Substring(pFrom, pTo - pFrom);
                                var words1 = ss.Trim().Split(' ');
                                var newword = variblename.ToString().Trim() + ".Append(" + result + ");       // FIXED";
                                lines = lines.Select(s => s != ss ? s : newword).ToArray();
                            }
                        }
                    }
                }
            }

            return lines;
        }

        private string[] AvoidStringEmptyCheck(string stringToCheck1, string[] lines)
        {
            var values = lines.Where(x => x.Contains(stringToCheck1));
            foreach (var d in values)
            {
                var dd = d;
                string variablec = string.Empty;
                var words = dd.Split(' ');
                int loc = dd.IndexOf('(');
                while (loc != -1)
                {
                    Console.WriteLine(loc);
                    char character = dd[loc + 1];
                    loc += 1;
                    string gotspace = "NO";
                    if (character != 32)
                    {
                        if (variablec != "")
                            variablec = variablec + character.ToString();
                        else
                            variablec = character.ToString();
                        gotspace = "NO";
                    }
                    else
                    {
                        gotspace = "Yes";
                        if ((variablec != "") && (gotspace == "Yes"))
                        {
                            loc = -1;
                        }
                    }
                }
                var variblename = words[2];
                var wordsLin = "if (String.IsNullOrEmpty(" + variablec + ")) //FIXED";
                lines = lines.Select(s => s != d ? s : wordsLin).ToArray();
            }

            return lines;
        }

        private void WriteToFile(string destinationFile, string[] lines)
        {
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    writer.WriteLine(lines[currentLine - 1]);
                }
            }
        }

        private void InsertLineForNamespace(string[] lines, string stringCatch)
        {
            stringCatch = lines[0] + "\nusing Serilog;";
            lines[0] = stringCatch;
        }

        #region Oldcode InsertLine commented

        //private void InsertLine(string[] lines, string stringCatch)
        //{
        //    var values5 = lines.Where(x => x.Trim().Contains(stringCatch));
        //    if (values5.Count() > 0)
        //    {
        //        for (int line = 0; line < lines.Count(); line++)// string anyCatch in lines)
        //        {
        //            if (lines[line].Contains(stringCatch))
        //            {
        //                bool openBraket = false;
        //                bool closeBraket = false;
        //                bool foundSystex = false;
        //                line++;
        //                for (int lineT = line; lineT < lines.Count(); lineT++)
        //                {
        //                    if (lines[lineT].Contains("{"))
        //                    {
        //                        openBraket = true;
        //                        foundSystex = false;
        //                    }
        //                    else if (lines[lineT].Contains("}"))
        //                    {
        //                        line = lineT;
        //                        closeBraket = true;
        //                        if (!foundSystex)
        //                        {
        //                            if (stringCatch == "Dispose()")
        //                            {
        //                                lines[lineT] = "Dispose(true);\n GC.SupressFinalize(this);//Fixed Dispose\n}";

        //                            }
        //                            if (stringCatch == "catch (")
        //                            {
        //                                if (log4NetChecked == "Checked")
        //                                {
        //                                    lines[lineT] = "Log.Error(Ex.ToString()); \n}";
        //                                    Catchblock = "YES";
        //                                }
        //                                else
        //                                {
        //                                    lines[lineT] = "//Dosomething here; //Fixed. \n} ";
        //                                }

        //                                foundSystex = false;
        //                            }
        //                            if (stringCatch == "finally")
        //                            {
        //                                lines[lineT] = "//Dosomething here; //Fixed. \n} ";
        //                                foundSystex = false;
        //                            }
        //                        }
        //                        //lineT = lines.Count();
        //                    }
        //                    else if (!string.IsNullOrEmpty(lines[lineT].Trim()))
        //                    {
        //                        foundSystex = true;
        //                    }
        //                    line = lineT;
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion Oldcode InsertLine commented

        private void InsertLine(string[] lines, string stringCatch)
        {
            var Trimlines = lines.Where(x => x.Trim().ToLower().Contains(stringCatch));
            string catchI = "catch";

            if (Trimlines.Count() > 0)
            {
                for (int line = 0; line < lines.Count(); line++)// string anyCatch in lines)
                {
                    if (lines[line].ToLower().Contains(stringCatch))
                    {
                        string variablec = "";

                        bool catch_block = lines[line].ToLower().Contains(stringCatch);
                        int loc = lines[line].IndexOf(')');
                        while (loc != -1)
                        {
                            Console.WriteLine(loc);
                            char character = lines[line][loc - 1];
                            loc -= 1;
                            string gotspace = "NO";
                            if (character != 32)
                            {
                                if (variablec != "")
                                    variablec = character.ToString() + variablec;
                                else
                                    variablec = character.ToString();
                                gotspace = "NO";
                            }
                            else
                            {
                                gotspace = "Yes";
                                if ((variablec != "") && (gotspace == "Yes"))
                                {
                                    loc = -1;
                                }
                            }
                        }
                        bool openBraket = false;
                        bool closeBraket = false;
                        bool foundSystex = false;
                        line++;
                        for (int lineT = line; lineT < lines.Count(); lineT++)
                        {
                            if (closeBraket != true)
                            {
                                if (lines[lineT].Contains("{"))
                                {
                                    openBraket = true;
                                    foundSystex = false;
                                }
                                else if (lines[lineT].Contains("}"))
                                {
                                    line = lineT;
                                    closeBraket = true;
                                    if (!foundSystex)
                                    {
                                        if (stringCatch == "Dispose()")
                                        {
                                            lines[lineT] = "Dispose(true);\n GC.SupressFinalize(this);//Fixed Dispose\n}";
                                        }
                                        if (catch_block)
                                        {
                                            if (serilogChecked == "Checked")
                                            {
                                                lines[lineT] = "Log.Error(" + variablec + ".ToString()); \n}";
                                                Catchblock = "YES";
                                            }
                                            else
                                            {
                                                lines[lineT] = "//Dosomething here; //Fixed. \n} ";
                                            }

                                            foundSystex = false;
                                        }
                                        if (stringCatch == "finally")
                                        {
                                            lines[lineT] = "//Dosomething here; //Fixed. \n} ";
                                            foundSystex = false;
                                        }
                                    }
                                    //lineT = lines.Count();
                                }
                                else if (!string.IsNullOrEmpty(lines[lineT].Trim()))
                                {
                                    foundSystex = true;
                                    string newline = lines[lineT];
                                    if (serilogChecked == "Checked")
                                    {
                                        lines[lineT] = "Log.Error(" + variablec + ".ToString()); \n" + newline;
                                        Catchblock = "YES";
                                    }
                                    closeBraket = true;
                                }
                                line = lineT;
                            }
                            else
                            {
                                lineT = lines.Count();
                            }
                        }
                    }
                }
            }
        }

        public void Replacecode(string FilePath, string oldCode, string newCode)
        {
            string text;

            using (StreamReader sr = new StreamReader(FilePath))
            {
                text = sr.ReadToEnd();
                sr.Close();
            }

            text = text.Replace(oldCode, newCode);
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        private void StartUpFileLog(string[] lines, string targetPath)
        {
            using (PowerShell powershell = PowerShell.Create())
            {
                //targetPath = @"C:\Projects\New folder (2)\devnxt2_1\devNXT\wwwroot\UploadedRemediation\ExtractCode\devNXTBankingPortal\devNXTBankingPortal";
                string dir_Main = targetPath;

                powershell.AddScript(String.Format(@"cd {0}; dotnet add package Serilog; dotnet add package Serilog.Sinks.RollingFile;
                              dotnet restore", dir_Main)).Invoke();

                string namespaceold = @"using System;";

                string strold = @"Configuration = configuration";
                string strnew = @"
                            Configuration = configuration;
                            Log.Logger = new LoggerConfiguration()
                            .WriteTo.RollingFile(Path.Combine(Directory.GetCurrentDirectory() + @""\wwwroot"", ""log -{ Date}.txt""))
                            .CreateLogger();";
                var values5 = lines.Where(x => x.Trim().Contains(strold));
                if (values5.Count() > 0)
                {
                    for (int line = 0; line < lines.Count(); line++)// string anyCatch in lines)
                    {
                        if (lines[line].Contains(namespaceold))
                        {
                            lines[line] = namespaceold + "\n" + "using Serilog;" + "\n" + "using System.IO;";
                        }
                        else if (lines[line].Contains(strold))
                        {
                            lines[line] = strnew;
                        }
                    }
                }
            }
        }

        private void StartUpFileLog4Net(string[] lines, string targetPath)
        {
            using (PowerShell powershell = PowerShell.Create())
            {

                string dir_Main = targetPath;

                powershell.AddScript(String.Format(@"cd {0};dotnet add package Microsoft.Extensions.Logging.Log4Net.AspNetCore --version 3.1.0", dir_Main)).Invoke();

                string namespaceold = @"using System;";

                string strold = @"public void Configure(IApplicationBuilder app, IWebHostEnvironment env)";
                string strnew = @"public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)";
                string strold1 = @"app.UseRouting();";
                string strnew1 = @"app.UseRouting();loggerFactory.AddLog4Net();";
                var values5 = lines.Where(x => x.Trim().Contains(strold));
                if (values5.Count() > 0)
                {
                    for (int line = 0; line < lines.Count(); line++)// string anyCatch in lines)
                    {
                        if (lines[line].Contains(namespaceold))
                        {
                            lines[line] = namespaceold + "\n" + "using Microsoft.Extensions.Logging;";
                        }
                        else if (lines[line].Contains(strold))
                        {
                            lines[line] = strnew;
                        }
                        else if (lines[line].Contains(strold1))
                        {
                            lines[line] = strnew1;
                        }
                    }
                }
            }
        }


        private string[] AvoidUnderscore(string[] lines)
        {
            string searchPublic = "_";
            var foundPublic = lines.Where(x => x.Contains(searchPublic));
            if (foundPublic.Count() > 0)
            {

                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(searchPublic))
                    {

                        string systax = lines[line];
                        lines[line] = systax.Replace(@"_", "") + " //Avoid underscore in syntax. FIXED.";

                        line++;
                    }
                }
            }
            return lines;
        }

        private string[] PascalCaseNaming(string[] lines)
        {
            string searchPublic = "_";
            var foundPublic = lines.Where(x => (x.Contains("public") || x.Contains("Private") || x.Contains("static") || x.Contains("namespace") || x.Contains("class") || x.Contains("(")) && !(x.Contains(";") || x.Contains("if") || x.Contains("switch") || x.Contains("using") || x.Contains("foreach") || x.Contains("catch") || x.Contains("#") || x.Contains("||") || x.Contains("//") || x.Contains("return")));
            if (foundPublic.Count() > 0)
            {
                foreach (var item in foundPublic)
                {
                    if (item.Contains("namespace"))
                    {
                        var indx = item.IndexOf("namespace ") + 10;
                        var le1n = item.Length;
                        var str1 = item.Substring(indx, (le1n - indx)).Split(new char[] { ' ' }).Select(a => a.Trim()).ToArray();
                        str1 = str1.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        var str = str1[0];
                        if (Char.IsLower(str.ToCharArray()[0]))
                        {
                            var name = Char.ToUpper(str[0]) + str.Substring(1);
                            var ind = Array.IndexOf(lines, item);
                            var i = lines[ind];
                            lines[ind] = item.Replace(str, name) + "//Namespace should be in Pascal case. FIXED.";
                            //item = 
                        }
                    }
                    if (item.Contains("class"))
                    {
                        var indx = item.IndexOf("class ") + 6;
                        var le1n = item.Length;
                        var str1 = item.Substring(indx, (le1n - indx)).Split(new char[] { ' ' }).Select(a => a.Trim()).ToArray();
                        str1 = str1.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        var str = str1[0];
                        if (Char.IsLower(str.ToCharArray()[0]))
                        {
                            var name = Char.ToUpper(str[0]) + str.Substring(1);
                            var ind = Array.IndexOf(lines, item);
                            var i = lines[ind];
                            lines[ind] = item.Replace(str, name) + "//Class name should be in Pascal case. FIXED.";
                            //item = 
                        }

                    }
                    else
                    {
                        var ary = item.Split(' ');
                        var idxof = ary.Where(x => x.Contains("(")).ToArray()[0];
                        var id1 = idxof.IndexOf("(");
                        var item1 = idxof.Substring(0, id1);
                        if (Char.IsLower(item1.ToCharArray()[0]))
                        {
                            var name = Char.ToUpper(item1[0]) + item1.Substring(1);
                            var ind = Array.IndexOf(lines, item);
                            var i = lines[ind];
                            lines[ind] = item.Replace(item1, name) + "//Method name should be in Pascal case. FIXED.";
                            //item = 
                        }

                    }
                }
            }
            return lines;
        }

        private string[] CamelCaseNaming(string[] lines)
        {
            string searchPublic = "_";
            var foundPublic = lines.Where(x => x.Contains(searchPublic));
            if (foundPublic.Count() > 0)
            {

                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(searchPublic))
                    {

                        string systax = lines[line];
                        lines[line] = systax.Replace(@"_", "") + " //Avoid underscore in syntax. FIXED.";

                        line++;
                    }
                }
            }
            return lines;
        }

        private string[] AvoidSingleCharVariable(string[] lines)
        {
            string searchPublic = "=";
            var foundPublic = lines.Where(x => x.Contains(searchPublic));
            if (foundPublic.Count() > 0)
            {
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(searchPublic))
                    {
                        string[] strList = { searchPublic };
                        String[] strlist = lines[line].Split(strList,
                        StringSplitOptions.RemoveEmptyEntries);
                        string part = lines[line].Substring(0, lines[line].IndexOf('='));
                        string[] parts = Regex.Replace(part, " {2,}", " ").Split(' ');

                        var word = parts[parts.Length - 1];
                        if (word.Length == 1)
                        {
                            if (word != "!" && word != ">" && word != "<")
                            {
                                lines[line] = lines[line] + " //Avoid to dedeclare any single character varaible. FIXED.";
                            }
                        }
                        line++;
                    }
                }
            }
            return lines;
        }
        private void nuget(string[] lines)
        {

            lines[0] = lines[0] + "\nusing Microsoft.AspNetCore.Mvc;\nusing Microsoft.AspNet.Mvc;";

            for (int line = 0; line < lines.Count(); line++)
            {

                if (lines[line].Contains("ActionResult"))
                {
                    lines[line].Replace("ActionResult", "IActionResult");


                }

            }
        }
        private void DirectAccesstoDbCheck(string[] lines)
        {
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("insert ") || lines[line].Contains("update ") || lines[line].Contains("delete ") || lines[line].Contains("select "))
                {
                    if (lines[line].Contains("//"))
                    {
                        int loc = lines[line].IndexOf("//");
                        lines[line] = lines[line].Remove(loc, lines[line].Length - loc);
                    }
                    lines[line] = lines[line] + " //Access to the DB VIOLATION.";

                }
            }
        }

        private void AddStaticForInlineMethod(string[] lines)
        {
            bool foundRightRoundBrace = false;
            for (int i = 0; i < lines.Length; i++)
            {

                if (lines[i].Contains(')'))
                {
                    foundRightRoundBrace = true;
                }
                if (foundRightRoundBrace && !lines[i].Contains("IActionResult"))
                {
                    if (lines[i].Contains(')') && lines[i].Contains('{') && lines[i].Contains("return ") && lines[i].Contains('}'))
                    {
                        lines[i] = "static " + lines[i] + " //Fixed";
                        //break;
                    }
                    else if (lines[i].Contains('{') && lines[i].Contains("return ") && lines[i].Contains('}'))
                    {
                        lines[i - 1] = "static " + lines[i - 1] + " //Fixed";
                        //break;
                    }
                    else if (lines[i].Contains('{') && !lines[i].Contains("return ") && !lines[i - 1].Contains("IActionResult") && !lines[i - 2].Contains("IActionResult"))
                    {
                        i++;
                        if (lines[i].Contains("return "))
                        {
                            if (lines[i - 2].Contains("//"))
                            {
                                int loc = lines[i - 2].IndexOf("//");
                                lines[i - 2] = lines[i - 2].Remove(loc, lines[i - 2].Length - loc);
                            }

                            lines[i - 2] = "static " + lines[i - 2].Trim() + " //Fixed";
                            //break;
                        }
                    }
                }
            }
        }

        private void AvoidcalltoAcceptChangesinaloop(string[] lines)
        {
            bool openBraket = false;
            bool closeBraket = false;
            bool foundSystex = false;
            string acceptanceline = "";
            bool bForLoop = false;
            for (int line = 0; line < lines.Count(); line++)
            {

                if (lines[line].Contains("for ("))
                {
                    bForLoop = true;

                }
                if (bForLoop)
                {
                    if (lines[line].Contains("{"))
                    {
                        openBraket = true;
                        foundSystex = false;
                    }
                    if (lines[line].Contains(".AcceptChanges()"))
                    {
                        acceptanceline = lines[line];
                        lines[line] = "";
                        foundSystex = true;
                    }
                    if (openBraket && foundSystex)
                    {
                        if (lines[line].Contains("}"))
                        {
                            lines[line] = lines[line] + "\n" + acceptanceline;//Avoid call to AcceptChanges in a loop
                        }
                    }
                }

            }
        }


        private string[] DisableConstraintsBeforeMergingDataSet(string[] lines)
        {
            string SearchMerging = ".Merge";
            var foundPublic = lines.Where(x => x.Contains(SearchMerging));
            if (foundPublic.Count() > 0)
            {
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(SearchMerging))
                    {
                        string[] strList = { SearchMerging };
                        String[] strlist = lines[line].Split(strList,
                        StringSplitOptions.RemoveEmptyEntries);
                        string varpart = strlist[0].Trim();
                        string beforeline = varpart + ".EnforceConstraints = false;" + "\n";
                        string afterline = varpart + ".EnforceConstraints = true;";
                        string finalline = beforeline + lines[line] + "\n" + afterline;


                        lines[line] = finalline + " //Disable Constraints Before Merging DataSet. FIXED.";

                        line++;
                    }
                }
            }
            return lines;
        }

        public void AvoidUsingGotoStatement(string[] lines)
        {
            string Searchgoto = "goto";
            var foundgoto = lines.Where(x => x.Contains(Searchgoto));
            if (foundgoto.Count() > 0)
            {
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(Searchgoto))
                    {

                        lines[line] = lines[line] + " //Avoid using goto statement. FIXED.";

                    }
                }
            }
        }







        private void AvoidArtifactsWithTooManyParameters(string[] lines)
        {
            string searchPublic = "_";
            var foundPublic = lines.Where(x => (((x.Contains("public") || x.Contains("Private")) && x.Contains("(")) ||
            !(x.Contains("class")) && !(x.Contains(";"))));
            if (foundPublic.Count() > 0)
            {
                bool openBraket = false;
                bool closeBraket = false;
                bool foundSystex = false;
                string allparameters = "";


                for (int line = 0; line < lines.Count(); line++)
                {
                    bool foundPublicparameter = false;
                    bool singleline = false;
                    if ((lines[line].Contains("public ") || lines[line].Contains("private ")) &&
                        lines[line].Contains("(") && (!lines[line].Contains("class")) &&
                        (!lines[line].Contains(";")))
                    {
                        foundPublicparameter = true;
                    }

                    if (foundPublicparameter)
                    {
                        foundSystex = true;
                        if (lines[line].Contains("(") && lines[line].Contains(")"))
                        {
                            singleline = true;
                        }
                        else
                        {
                            singleline = false;
                        }
                        allparameters = lines[line].ToString();



                    }
                    if (!closeBraket && foundSystex)
                    {
                        if (!lines[line].Contains("("))
                        {
                            allparameters = allparameters + lines[line];
                        }

                        if (lines[line].Contains(")"))
                        {
                            closeBraket = true;
                            string[] splittedValue = allparameters.Split('(');

                            string splitValue = splittedValue[1].ToString();
                            splittedValue = splitValue.Split(')');

                            splitValue = splittedValue[0].ToString();

                            splittedValue = splitValue.Split(',');
                            if (splittedValue.Count() > 9)
                            {
                                lines[line] = lines[line] + "\n" + "\\Avoid Artifacts with too many parameters. fixed.";
                            }
                            closeBraket = false;
                            if (singleline)
                            {
                                allparameters = "";
                                foundSystex = false;
                            }

                        }
                    }

                }


            }
        }


        private void Namespaceuppercase(string[] lines)
        {


            for (int line = 0; line < lines.Count(); line++)
            {

                if (lines[line].Contains("namespace"))
                {
                    int index = lines[line].IndexOf("namespace") + 10;

                    while (Char.IsWhiteSpace(lines[line], index))
                    {
                        index++;
                    }
                    char name = lines[line][index];
                    if (Char.IsLower(name))
                    {
                        lines[line] = lines[line] + "  //Namespaces should start with an uppercase character";

                    }
                }

            }

        }

        private void PrivateFields(string[] lines)
        {

            string text = lines.ToString();
            StringBuilder sb = new StringBuilder(text);

            string remediateMSG = string.Empty;
            char[] charline = text.ToCharArray();
            string value = "private ";
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = text.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }
            foreach (int publicStartPos in indexes)
            {
                remediateMSG = string.Empty;
                int tempPos = publicStartPos, flag = 0, upper = 0;
                while (true)
                {
                    if ((text[tempPos] == ':') || (text[tempPos] == '{') || (text[tempPos] == ',') || (text[tempPos] == ';') || (text[tempPos] == '('))
                    {

                        char found = text[tempPos];
                        int apnd = tempPos + 1;
                        if (found == ':')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsUpper(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");

                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == '{')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsUpper(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == ',')
                        { }
                        if (found == ';')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsUpper(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == '(')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsUpper(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Private Fields should not start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            remediateMSG = string.Empty;
                            break;
                        }

                        break;

                    }
                    else
                    {
                        tempPos++;

                    }



                }

            }
            string newtext = sb.ToString();

        }
        private void PublicFields(string[] lines)
        {
            string text = lines.ToString();
            StringBuilder sb = new StringBuilder(text);
            string remediateMSG = string.Empty;
            char[] charline = text.ToCharArray();
            string value = "public ";
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = text.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }
            foreach (int publicStartPos in indexes)
            {
                remediateMSG = string.Empty;
                int tempPos = publicStartPos, flag = 0, upper = 0;
                while (true)
                {
                    if ((text[tempPos] == ':') || (text[tempPos] == '{') || (text[tempPos] == ',') || (text[tempPos] == ';') || (text[tempPos] == '('))
                    {

                        char found = text[tempPos];
                        int apnd = tempPos + 1;
                        if (found == ':')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsLower(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");

                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");

                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == '{')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsLower(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == ',')
                        { }
                        if (found == ';')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsLower(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        if (found == '(')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsLower(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Public Fields should start with an uppercase character and should not include any underscore*/");
                            }
                            Console.WriteLine(remediateMSG);
                            remediateMSG = string.Empty;
                            break;
                        }

                        break;

                    }
                    else
                    {
                        tempPos++;

                    }



                }

            }
            string newtext = sb.ToString();
        }
        private void Eventsnamingconvention(string[] lines)
        {
            string text = lines.ToString();
            string remediateMSG = string.Empty;
            char[] charline = text.ToCharArray();
            StringBuilder sb = new StringBuilder(text);
            string value = "event ";
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = text.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }
            foreach (int publicStartPos in indexes)
            {
                remediateMSG = string.Empty;
                int tempPos = publicStartPos, flag = 0, upper = 0;
                while (true)
                {
                    if ((text[tempPos] == ';'))
                    {

                        char found = text[tempPos];
                        int apnd = tempPos + 1;
                        if (found == ';')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    break;
                                }

                            }
                            if (Char.IsLower(text[upper]))
                            {
                                sb.Insert(apnd, "/*Names of Events should start with an uppercase character and should not include any underscore*/");

                            }
                            if (flag == 1)
                            {
                                sb.Insert(apnd, "/*Names of Events should start with an uppercase character and should not include any underscore*/");

                            }
                            Console.WriteLine(remediateMSG);
                            break;
                        }
                        break;

                    }
                    else
                    {
                        tempPos++;

                    }



                }

            }
            string newtext = sb.ToString();
        }
        private void EnumerationItemsnamingconvention(string[] lines)
        {
            string text = lines.ToString();
            string remediateMSG = string.Empty;
            char[] charline = text.ToCharArray();
            StringBuilder sb = new StringBuilder(text);
            string value = "enum ";
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = text.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }
            foreach (int publicStartPos in indexes)
            {
                remediateMSG = string.Empty;
                int tempPos = publicStartPos, flag = 0, upper = 0;
                while (true)
                {
                    if ((text[tempPos] == ';'))
                    {

                        char found = text[tempPos];
                        int apnd = tempPos + 1;
                        if (found == ';')
                        {
                            while (true)
                            {
                                tempPos--;
                                if (text[tempPos] == ' ' || text[tempPos] == '\r' || text[tempPos] == '\t' || text[tempPos] == '\n' || text[tempPos] == '}' || text[tempPos] == ',')
                                {
                                }
                                else
                                {
                                    while (text[tempPos] != ' ')
                                    {
                                        if (text[tempPos] == '_')
                                            flag = 1;
                                        tempPos--;
                                    }
                                    upper = ++tempPos;
                                    if (Char.IsLower(text[upper]))
                                    {
                                        sb.Insert(apnd, "/*Names of Enumerations Items should start with an uppercase character and should not include any underscore*/");

                                    }
                                    if (flag == 1)
                                    {
                                        sb.Insert(apnd, "/*Names of Enumerations Items should start with an uppercase character and should not include any underscore*/");
                                    }
                                    Console.WriteLine(remediateMSG);
                                    remediateMSG = string.Empty;
                                    if (text[tempPos] == '{')
                                    { break; }

                                }

                            }

                        }
                        break;

                    }
                    else
                    {
                        tempPos++;

                    }



                }

            }
            string newtext = sb.ToString();
        }



        public string[] AvoidUsingGCStatement(string[] lines)
        {
            string SearchGC = "GC.Collect";
            var foundGC = lines.Where(x => x.Contains(SearchGC));
            if (foundGC.Count() > 0)
            {
                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(SearchGC))
                    {

                        lines[line] = lines[line] + " //Avoid using GC.Collect statement. FIXED.";

                    }
                }
            }
            return lines;
        }



        private string[] AvoidIs(string[] lines)
        {
            string searchthis = "this is";
            var foundthis = lines.Where(x => x.Contains(searchthis));
            if (foundthis.Count() > 0)
            {

                for (int line = 0; line < lines.Count(); line++)
                {
                    if (lines[line].Contains(searchthis))
                    {

                        string systax = lines[line];
                        lines[line] = systax.Replace(@" is", "") + " //Avoid is with this in syntax. FIXED.";

                        line++;
                    }
                }
            }
            return lines;
        }

        private static string[] Namespace(string[] lines)
        {
            string searchEx = "namespace";
            string searchparan = "{";
            int linecount = 0;

            //var founditems = lines.Where(x => (x.Contains("\n") || x.Contains(" ") || x.Contains(null)));
            var foundEx = lines.Where(x => (x.Contains(searchEx) || x.Contains(searchparan)));


            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains(searchEx) || lines[line].Contains(searchparan))
                {

                    linecount = line + 2;

                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains("\n") || lines[line].Contains(" ") || lines[line].Contains("\t"))
                        {
                            if (lines[line].Contains("  "))
                            { continue; }
                            else if (lines[line].Contains("}"))
                            {
                                lines[linecount] += "//namespace is empty";
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (lines[line].Contains("}"))
                        { lines[linecount] += "//namespace is empty"; }
                        else
                        { break; }
                    }


                    break;
                }






            }

            return lines;
        }


        public static string[] Methodempty(string[] lines)
        {
            string openbracket = "(";
            string closingbracket = ")";
            int linecount = 0;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains(openbracket) && lines[line].Contains(closingbracket))
                {
                    line++;
                    if (lines[line].Contains("{"))
                    {


                        linecount = line + 1;
                        for (line = linecount; line < lines.Count(); line++)
                        {
                            if (lines[line].Contains("\n") || lines[line].Contains(" ") || lines[line].Contains("\t"))
                            {
                                if (lines[line].Contains("  "))
                                { continue; }

                            }
                            else if (lines[line].Contains("}"))
                            {
                                lines[linecount] += "//method is empty";
                                break;
                            }
                            else
                            { break; }
                        }
                    }
                }

            }


            return lines;
        }



        public static string[] loop(string[] lines)
        {
            string openbracket = "for(";

            int linecount = 0;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains(openbracket))
                {
                    //line++;



                    linecount = line;
                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains("++") || lines[line].Contains(";"))
                        {

                            continue;

                        }
                        else if (lines[line].Contains(")"))
                        {
                            lines[linecount] += "//When only the condition expression is defined in a for loop, and the initialization and increment expressions are missing, a while loop should be used ";
                            break;
                        }
                        else
                        { break; }
                    }

                }

            }


            return lines;
        }


        private static string[] Empty(string[] lines)
        {
            string searchEx = "{";
            string searchparan = "}";
            int linecount = 0;

            //var founditems = lines.Where(x => (x.Contains("\n") || x.Contains(" ") || x.Contains(null)));
            var foundEx = lines.Where(x => (x.Contains(searchEx)));


            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("{"))
                {

                    linecount = line + 1;

                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains(");"))
                        {

                            string systax = lines[line];
                            lines[line] = systax.Replace(@";", "") + " // unwanted ; is removed. FIXED.";

                        }
                        else if (lines[line].Contains(";;"))
                        {
                            string systax = lines[line];
                            lines[line] = systax.Replace(@";;", ";") + " //extra ; is removed. FIXED.";

                        }
                        else if (lines[line].Contains(";"))
                        {

                            line++;
                            if (lines[line].Contains("}"))
                            {
                                lines[line] += "class is empty";
                            }
                            else
                            {
                                break;
                            }
                        }


                        else
                        { break; }
                    }


                    break;
                }






            }

            return lines;
        }

        private static string[] SwitchCase(string[] lines)
        {
            string searchEx = "{";
            string searchparan = "}";
            int linecount = 0;

            //var founditems = lines.Where(x => (x.Contains("\n") || x.Contains(" ") || x.Contains(null)));
            var foundEx = lines.Where(x => (x.Contains(searchEx)));


            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("switch"))
                {

                    linecount = line + 1;


                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains("{"))
                        {
                            line++;
                            int counter = 0;
                            for (line = linecount; line < lines.Count(); line++)
                            {

                                if (lines[line].Contains("case"))
                                {
                                    counter++;
                                    if (counter < 3)
                                    {
                                        continue;

                                    }
                                    else if (lines[line].Contains("]"))
                                    { continue; }
                                    else
                                    {
                                        lines[line] += "//dont use two many cases";
                                        break;
                                    }
                                }
                                //else if (lines[line].Contains("}"))
                                //{ break; }
                                //else { break; }
                            }

                        }
                        else
                        { break; }
                    }


                    break;
                }
            }

            return lines;
        }


        private static string[] Exceptioncase(string[] lines)
        {
            string ex = "Exception";
            int linecount = 0;
            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("get"))
                {
                    linecount = line + 1;
                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains("{"))
                        {
                            linecount = line;
                            for (line = linecount; line < lines.Count(); line++)
                            {
                                line++;
                                if (lines[line].Contains("throw") || lines[line].Contains("Exception"))
                                {
                                    lines[line] += "Exceptions should not be thrown from property getters";
                                    continue;

                                }
                                else { break; }
                            }

                        }
                        else { break; }
                    }

                }


            }
            return lines;

        }

        private static string[] AvoidExceptionFinally(string[] lines)
        {
            int linecount = 0;

            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("finally"))
                {
                    linecount = line + 1;
                    for (line = linecount; line < lines.Count(); line++)
                    {
                        if (lines[line].Contains("{"))
                        {
                            linecount = line;
                            for (line = linecount; line < lines.Count(); line++)
                            {

                                if (lines[line].Contains("throw"))
                                {
                                    lines[line] += "Exceptions should not be thrown in finally blocks";
                                    break;

                                }
                                else { continue; }
                            }
                        }


                    }
                }

            }
            return lines;

        }

        private static string[] AvoidMultidimensionalArray(string[] lines)
        {
            string searchEx = "{";
            string searchparan = "}";
            int linecount = 0;
            int count = 0;
            //var founditems = lines.Where(x => (x.Contains("\n") || x.Contains(" ") || x.Contains(null)));
            var foundEx = lines.Where(x => (x.Contains(searchEx)));


            for (int line = 0; line < lines.Count(); line++)
            {
                if (lines[line].Contains("public") && lines[line].Contains("(") && lines[line].Contains(")"))
                {
                    linecount = line;
                    var chartest = lines[line].ToCharArray();
                    for (var char1 = 0; char1 < chartest.Count(); char1++)
                    {
                        if (chartest[char1] == '[')
                        {
                            count++;

                        }
                        if (chartest[char1] == ']')
                        {
                            count++;
                        }


                        else
                        { continue; }
                    }
                    if (count == 4)
                    { lines[linecount] += "Public methods should not have multidimensional array parameters"; }


                }
            }

            return lines;
        }


        #endregion

    }
}
