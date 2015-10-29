using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StyleCop;
using StyleCop.CSharp;
using System.Globalization;

namespace NamingConventionsByChirag
{
    [SourceAnalyzer(typeof(CsParser))]
    public class NamingConventions : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csdocument = (CsDocument)document;
            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.AnalyzeSourceCodeForNamingConvention), null, null);
                csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.AnalyzeSourceCodeForComments), null, null);

            }
        }

        private bool AnalyzeSourceCodeForNamingConvention(CsElement element, CsElement parentElement, object context)
        {
            string sName = string.Empty;
            string sType = string.Empty;
            int lineNumber;

            if (element.ElementType == ElementType.Method)
            {
                IEnumerator<Variable> iEnumerator = element.Variables.GetEnumerator();

                while (iEnumerator.MoveNext())
                {
                    sType = (iEnumerator.Current).Type.ToString();
                    sName = (iEnumerator.Current).Name;
                    lineNumber = (iEnumerator.Current).Location.LineNumber;

                    ValidateNamingConventions(sType, sName, lineNumber, parentElement);
                }
            }
            return true;
        }

        private void ValidateNamingConventions(string sType, string sName, int lineNumber, CsElement parentElement)
        {
            switch (sType)
            {
                case "string":
                    if (!(sName.Length > 2 && sName.Substring(0, 1).Contains("s") && sName.Substring(1, 1).ToUpper(CultureInfo.InvariantCulture) == sName.Substring(1, 1)))
                    {
                        this.AddViolation(parentElement, "NamingConventionsString", "String variable should start with 's' and second letter capital.");
                    }
                    break;

                case "bool":
                    if (!(sName.Length > 2 && sName.Substring(0, 1).Contains("b") && sName.Substring(1, 1).ToUpper(CultureInfo.InvariantCulture) == sName.Substring(1, 1)))
                    {
                        this.AddViolation(parentElement, "NamingConventionsBool", "Bool variable should start with 'b' and second letter capital.");
                    }
                    break;

                default:
                    break;
            }
        }

        private bool AnalyzeSourceCodeForComments(CsElement element, CsElement parentElement, object context)
        {
            CsDocument csharpdocument = (CsDocument)element.Document;
            string commentLine = string.Empty;

            string disp = "Hello";

            for (var tokenNode = csharpdocument.Tokens.First; tokenNode != null; tokenNode = tokenNode.Next)
            {
                if (tokenNode.Value.CsTokenType == CsTokenType.SingleLineComment)
                {
                    commentLine = tokenNode.Value.Text.ToString();
                    disp = disp + "\n" + commentLine;
                    ValidateCommentForCreatedByElement(commentLine, parentElement);
                }
            }
            System.IO.File.WriteAllText(@"D:\yoyoyo.txt", disp);
            return true;
        }

        private void ValidateCommentForCreatedByElement(string commentLine, CsElement parentElement)
        {
            if ((commentLine.Substring(0, commentLine.Length - 1) == "CreatedBy"))
            {
                if (!(commentLine.Substring(0, commentLine.Length - 1) == "Chirag Lad."))
                {
                    //this.AddViolation(parentElement, "CreatedByRule", "Should only be created by Chirag Lad.");
                    System.IO.File.WriteAllText(@"D:\yoyoyo2.txt", "hit hua");

                }
            }
        }
    }
}
