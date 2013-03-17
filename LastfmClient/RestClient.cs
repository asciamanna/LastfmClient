using System.IO;
using System.Net;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace LastfmClient {
  public interface IRestClient {
    XElement DownloadData(string uri);
  }
  public class RestClient : IRestClient {
    public XElement DownloadData(string uri) {
      using (var client = new WebClient()) {
        var rawData = client.DownloadData(uri);
        if (rawData != null) {
          return XElement.Parse(Encoding.UTF8.GetString(rawData));
        }
        return null;
      }
    }
  }
}
