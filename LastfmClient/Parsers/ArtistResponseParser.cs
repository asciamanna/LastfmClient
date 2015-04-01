using System;
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
      var artistInfoElement = xmlResponse.DescendantsAndSelf("artist").First();
      return CreateArtistInfo(artistInfoElement);
    }

    private LastfmArtistInfo CreateArtistInfo(XElement artistInfoElement) {
      return new LastfmArtistInfo {
        Name = artistInfoElement.Element("name").Value,
        Mbid = artistInfoElement.Element("mbid").Value,
        BioSummary = ParseBioSummary(artistInfoElement),
        PlaceFormed = ParsePlaceFormed(artistInfoElement),
        YearFormed = ParseYearFormed(artistInfoElement)
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
