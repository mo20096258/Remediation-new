using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class RulesConfiguration
    {
        public string DisposeRule { get; set; }
        public string PrivateConstructor { get; set; }
        public string BreakInFor { get; set; }
        public string ThreadSleep { get; set; }
        public string EmptyFinal { get; set; }
        public string EmptyCatch { get; set; }
        public string StringConcatenationInLoop { get; set; }
        public string LargeStringConcatenation { get; set; }
        public string StringEmpty { get; set; }
        public string InstantiationsFor { get; set; }
        public string Formatter { get; set; }
        public string PublicField { get; set; }
        public string UnreferencedArtifacts { get; set; }
        public string LargeClass { get; set; }
        public string Unreferenced { get; set; }
        public string ManyConstructors { get; set; }
        public string SQLConnection { get; set; }
        public string LargeMethod { get; set; }
        public string AvoidSingleCharVariable { get; set; }
        public string AvoidUnderscore { get; set; }
        public string CamelCaseNaming { get; set; }
        public string DirectAccesstoDbCheck { get; set; }
        public string DisableConstraintsBeforeMergingDataSet { get; set; }
        public string AvoidcalltoAcceptChangesinaloop { get; set; }
        public string AvoidUsingGotoStatement { get; set; }
        public string AvoidArtifactsWithTooManyParameters { get; set; }
        public string Namespaceuppercase { get; set; }
        public string nuget { get; set; }

        public string UnusedVariables { get; set; }
        public string CollapsibleIfToBeMerged { get; set; }
        public string SwitchToHaveAtleastThreeCases { get; set; }

        public string UtilityClassesNotInstantiated { get; set; }
        public string DuplicateImplementation { get; set; }
        public string IndexOfChecks { get; set; }
        public string ForLoopCounterRightDirection { get; set; }
        public string NonConstantStaticField { get; set; }
        public string GetHashCodeMutableField { get; set; }
        public string EmptyArrayandCollection { get; set; }
        public string stringIsNullOrEmpty { get; set; }
        public string EqualsOverridden { get; set; }
        public string RightOperandOfShiftOperator { get; set; }
        public string AvoidMethodwithManyParameters { get; set; }

    }
}
