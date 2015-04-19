using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public class UserTopArtistResponseParser : BaseUserResponseParser, IUserResponseParser {
    protected override string CollectionElementName {
      get { return "topartists"; }
    }

    protected override string ItemElementName {
      get { return "artist"; }
    }

    protected override IEnumerable<LastfmUserItem> CreateItems(IEnumerable<XElement> items) {
      return items.Select(CreateUserTopArtist).Cast<LastfmUserItem>().ToList();
    }

    private LastfmUserTopArtist CreateUserTopArtist(XElement artistElement) {
      return new LastfmUserTopArtist {
        Rank = Int32.Parse(artistElement.Attribute("rank").Value),
        Name = artistElement.Element("name").Value,
        PlayCount = Int32.Parse(artistElement.Element("playcount").Value),
        MegaImageLocation = ParseImageLocation(artistElement, "mega"),
        ExtraLargeImageLocation = ParseImageLocation(artistElement, "extralarge"),
        LargeImageLocation = ParseImageLocation(artistElement, "large"),
        MediumImageLocation = ParseImageLocation(artistElement, "medium"),
        SmallImageLocation = ParseImageLocation(artistElement, "small"),
      };
    }
  }
}
