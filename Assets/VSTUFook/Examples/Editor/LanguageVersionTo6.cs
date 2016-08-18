using System.Linq;
using System.Xml.Linq;

namespace VSTUFook
{
    [VSTUProjectFook(VSTUProjectKinds.All)]
    public class LanguageVersionTo6 :
        IVSTUProjectFook
    {
        public XDocument Fook(XDocument document)
        {
            document
                .Descendants()
                .First(e => e.Name.LocalName == "LangVersion")
                .Value = "6";

            return document;
        }
    }
}
