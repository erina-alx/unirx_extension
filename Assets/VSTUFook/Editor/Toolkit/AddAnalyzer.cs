using System;
using System.Linq;
using System.Xml.Linq;

namespace VSTUFook.Toolkit
{
    public abstract class AddAnalyzer :
        IVSTUProjectFook
    {
        private readonly string dllPath;

        protected AddAnalyzer(string dllPath)
        {
            if (dllPath == null)
                throw new ArgumentNullException("dllPath");

            this.dllPath = dllPath;
        }

        public XDocument Fook(XDocument document)
        {
            var project = document
                .Descendants()
                .First(e => e.Name.LocalName == "Project");

            project.Add(CreateItemGroup());

            return XDocument.Parse(project.ToString().Replace("<ItemGroup xmlns=\"\">", "<ItemGroup>"));
        }

        private XElement CreateItemGroup()
        {
            return new XElement(
                "ItemGroup",
                new XElement(
                    "Analyzer",
                    new XAttribute(
                        "Include",
                        dllPath
                        )
                    )
                );
        }
    }
}
