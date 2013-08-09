using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace VSTest2JUnit
{
    public static class VsTest2Xml
    {
        public static void Convert2Xml(IList<string> sourceFiles, string target)
        {

            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    var myXPathDoc = new XPathDocument(sourceFile);
                    var myXslTrans = new XslCompiledTransform();
                    myXslTrans.Load(basePath + "/XSLT/" + target + ".xslt");
                    var myWriter = new XmlTextWriter(GetXmlFileName(sourceFile), null);
                    myXslTrans.Transform(myXPathDoc, null, myWriter);
                }
                catch (Exception exception)
                {
                    //TODO: Log exception
                    Console.WriteLine(string.Format("Error converting file {0} to {1} xml", sourceFile, target));
                }
            }
        }

        private static string GetXmlFileName(string sourceFile)
        {
            return sourceFile.Substring(0, sourceFile.LastIndexOf(".")) + ".xml";
        }
    }
}
