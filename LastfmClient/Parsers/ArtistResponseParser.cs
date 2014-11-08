using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface IArtistResponseParser {
    LastfmArtistInfo Parse(XElement xmlResponse);
  }

  public class ArtistResponseParser : BaseResponseParser, IArtistResponseParser {
   
    public LastfmArtistInfo Parse(XElement xmlResponse) {
      ParseLfmNodeForErrors(xmlResponse);
      var artistInfo = xmlResponse.DescendantsAndSelf("artist").First();
      return new LastfmArtistInfo {
        Name = artistInfo.Element("name").Value,
        Mbid = artistInfo.Element("mbid").Value,
        BioSummary = ParseBioSummary(artistInfo),
        PlaceFormed = ParsePlaceFormed(artistInfo),
        YearFormed = ParseYearFormed(artistInfo)
      };
    }

    private static string ParseBioSummary(XElement artistInfo) {
      var bioNode = artistInfo.Descendants("bio").FirstOrDefault();
      return bioNode == null ?
        string.Empty : bioNode.Element("summary").Value.Trim();
    }
    
    private string ParsePlaceFormed(XElement artistInfo) {
      var bioNode = artistInfo.Descendants("bio").FirstOrDefault();
      if (bioNode != null) {
        var placeFormedElement = bioNode.Element("placeformed");
        if (placeFormedElement != null) {
          return placeFormedElement.Value.Trim();
        }
      }
      return string.Empty;
    }

    private int? ParseYearFormed(XElement artistInfo) {
      var bioNode = artistInfo.Descendants("bio").FirstOrDefault();
      if (bioNode != null) {
        var yearFormedElement = bioNode.Element("yearformed");
        if (yearFormedElement != null) {
          return int.Parse(yearFormedElement.Value.Trim());
        }
      }
      return null;
    }

    private static DateTime? ParseDateString(string date) {
      if (string.IsNullOrWhiteSpace(date)) {
        return null;
      }
      return DateTime.Parse(date);
    }
  }
}
