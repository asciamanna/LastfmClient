﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public abstract class BaseUserResponseParser {
    private readonly LfmNodeErrorParser lfmNodeErrorParser;

    protected BaseUserResponseParser() : this(new LfmNodeErrorParser()) { }
    
    protected BaseUserResponseParser(LfmNodeErrorParser lfmNodeErrorParser) {
      this.lfmNodeErrorParser = lfmNodeErrorParser;
    }

    public LastfmResponse<LastfmUserItem> Parse(XElement xmlResponse) {
      lfmNodeErrorParser.Parse(xmlResponse);
      var tracks = xmlResponse.DescendantsAndSelf(CollectionElementName);
      var tracksElement = tracks.First();
      return CreateUserItem(tracksElement);
    }

    private LastfmResponse<LastfmUserItem> CreateUserItem(XElement tracksElement) {
      return new LastfmResponse<LastfmUserItem> {
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
