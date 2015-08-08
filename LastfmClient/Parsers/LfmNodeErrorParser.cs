using System.Xml.Linq;

namespace LastfmClient.Parsers {
  public interface ILfmNodeErrorParser {
    void Parse(XElement lfmNode);
  }

  public class LfmNodeErrorParser : ILfmNodeErrorParser {
    public void Parse(XElement lfmNode) {
      if (lfmNode.Attribute("status").Value == "failed") {
        var errorCode = int.Parse(lfmNode.Element("error").Attribute("code").Value);
        var message = lfmNode.Element("error").Value.Trim();
        throw new LastfmException(message) {ErrorCode = errorCode};
      }
    }
  }
}
