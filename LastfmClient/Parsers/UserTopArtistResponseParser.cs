using System;
using System.Collections.Generic;
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
      var topArtists = new List<LastfmUserItem>();
      foreach (var artistElement in items) {
        topArtists.Add(CreateUserTopArtist(artistElement));
      }
      return topArtists;
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
