using System.Xml.Linq;

namespace VSTUFook
{
    // ReSharper disable once InconsistentNaming
    public interface IVSTUProjectFook
    {
        XDocument Fook(XDocument document);
    }
}
