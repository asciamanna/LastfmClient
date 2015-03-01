using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public class LibraryAlbumsResponseParser : BaseLibraryResponseParser, ILibraryResponseParser {
    protected override string CollectionElementName {
      get { return "albums"; }
    }

    protected override string ItemElementName {
      get { return "album"; }
    }

    protected override IEnumerable<LastfmLibraryItem> CreateItems(IEnumerable<XElement> items) {
      var libraryAlbums = new List<LastfmLibraryItem>();
      foreach (var albumElement in items) {
        libraryAlbums.Add(CreateLibraryAlbum(albumElement));
      }
      return libraryAlbums;
    }

    private static LastfmLibraryAlbum CreateLibraryAlbum(XElement albumElement) {
      return new LastfmLibraryAlbum {
        Name = albumElement.Element("name").Value,
        Artist = albumElement.Element("artist").Element("name").Value,
        PlayCount = Int32.Parse(albumElement.Element("playcount").Value),
        ArtworkLocation = albumElement.Elements("image").Where(e => e.Attribute("size").Value == "extralarge").FirstOrDefault().Value
      };
    }
  }
}
