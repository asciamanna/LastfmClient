using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface IAlbumResponseParser {
    LastfmAlbumInfo Parse(XElement xmlResponse);
  }

  public class AlbumResponseParser : IAlbumResponseParser {
   
    public LastfmAlbumInfo Parse(XElement xmlResponse) {
      var albumInfo = xmlResponse.DescendantsAndSelf("album").First();
      return new LastfmAlbumInfo {
        Name = albumInfo.Element("name").Value,
        Artist = albumInfo.Element("artist").Value,
        Mbid = albumInfo.Element("mbid").Value,
        ReleaseDate = ParseDateString(albumInfo.Element("releasedate").Value),
        WikiSummary = albumInfo.Descendants("wiki").First().Element("summary").Value.Trim()
      };
    }
    
    private static DateTime? ParseDateString(string date) {
      if (string.IsNullOrWhiteSpace(date)) {
        return null;
      }
      return DateTime.Parse(date);
    }
  }
}
