using System.Xml.Linq;

namespace LastfmClient.Parsers {
  public abstract class BaseResponseParser {

    protected void ParseLfmNodeForErrors(XElement xmlResponse) {
      if (xmlResponse.Attribute("status").Value == "failed") {
        var errorCode = int.Parse(xmlResponse.Element("error").Attribute("code").Value);
        var message = xmlResponse.Element("error").Value.Trim();
        throw new LastfmException(message) { ErrorCode = errorCode };
      }
    }
  }
}
