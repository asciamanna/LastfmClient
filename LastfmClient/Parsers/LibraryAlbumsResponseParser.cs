using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public class LibraryAlbumsResponseParser : BaseLastfmResponseParser, ILibraryResponseParser {
    protected override string CollectionElementName {
      get { return "albums"; }
    }

    protected override string ItemElementName {
      get { return "album"; }
    }

    protected override IEnumerable<LastfmLibraryItem> CreateItems(IEnumerable<XElement> items) {
      var libraryAlbums = new List<LastfmLibraryItem>();
      foreach (var album in items) {
        libraryAlbums.Add(new LastfmLibraryAlbum {
          Name = album.Element("name").Value,
          Artist = album.Element("artist").Element("name").Value,
          PlayCount = Int32.Parse(album.Element("playcount").Value),
          ArtworkLocation = album.Elements("image").Where(e => e.Attribute("size").Value == "extralarge").FirstOrDefault().Value
        });
      }
      return libraryAlbums;
    }
  }
}
