using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public abstract class BaseLibraryResponseParser : BaseResponseParser {
    private readonly ILfmNodeErrorParser lfmNodeErrorParser;

    protected BaseLibraryResponseParser(ILfmNodeErrorParser lfmNodeErrorParser) {
      this.lfmNodeErrorParser = lfmNodeErrorParser;
    }

    protected BaseLibraryResponseParser() : this(new LfmNodeErrorParser()) {}

    public LastfmResponse<LastfmLibraryItem> Parse(XElement xmlResponse) {
      lfmNodeErrorParser.Parse(xmlResponse);
      var collection = xmlResponse.DescendantsAndSelf(CollectionElementName);
      var collectionElement = collection.First();

      return CreateLibraryItem(collectionElement);
    }

    private LastfmResponse<LastfmLibraryItem> CreateLibraryItem(XElement collectionElement) {
      return new LastfmResponse<LastfmLibraryItem> {
        Page = Int32.Parse(collectionElement.Attribute("page").Value),
        PerPage = Int32.Parse(collectionElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(collectionElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(collectionElement.Attribute("total").Value),
        Items = CreateItems(collectionElement.Descendants(ItemElementName)),
      };
    }

    protected abstract string CollectionElementName { get; }

    protected abstract string ItemElementName { get; }
    
    protected abstract IEnumerable<LastfmLibraryItem> CreateItems(IEnumerable<XElement> items);  
  }
}
