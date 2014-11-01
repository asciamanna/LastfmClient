using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public abstract class BaseUserResponseParser {
    public LastfmResponse<LastfmUserItem> Parse(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf(CollectionElementName);
      var tracksElement = tracks.First();

      return new LastfmResponse<LastfmUserItem> {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Items = CreateItems(tracksElement.Descendants(ItemElementName)),
      };
    }

    protected abstract string CollectionElementName { get; }

    protected abstract string ItemElementName { get; }

    protected abstract IEnumerable<LastfmUserItem> CreateItems(IEnumerable<XElement> items);
    
    protected string ParseImageLocation(XElement element, string sizeAttribute) {
      return element.Elements("image").Where(e => e.Attribute("size").Value == sizeAttribute).FirstOrDefault().Value.Trim();
    }
  }
}
