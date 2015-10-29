using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StyleCop;

using StyleCop.CSharp;

namespace StyleCopNewRule
{




    [SourceAnalyzer(typeof(CsParser))]

    public class MyOwnCustomAnalyzer : SourceAnalyzer
    {

        public override void AnalyzeDocument(CodeDocument currentCodeDocument)
        {

            var codeDocument = (CsDocument)currentCodeDocument;

            if (codeDocument.RootElement != null && !codeDocument.RootElement.Generated)
            {

                codeDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.InspectCurrentElement), null, null);

            }

        }



        private bool InspectCurrentElement(CsElement element, CsElement parentElement, object context)
        {

            if (element.ElementType == ElementType.)
            {

                bool boolIsTryFound = false;

                bool boolIsCatchFound = false;



                var tempDocument = (CsDocument)element.Document;

                var objReader = tempDocument.SourceCode.Read();



                string strCode;

                while ((strCode = objReader.ReadLine()) != null)
                {

                    if (strCode.Contains("try"))
                    {

                        boolIsTryFound = true;

                    }



                    if (boolIsTryFound)
                    {

                        if (strCode.Contains("catch"))
                        {

                            boolIsCatchFound = true;

                        }

                    }

                }



                if ((boolIsTryFound) && (boolIsCatchFound == false))
                {

                    this.AddViolation(parentElement, "MyOwnCustomRule", "CatchShouldBeImplemented");

                }

            }

            return true;

        }



    }

}
