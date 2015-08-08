using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface IArtistResponseParser {
    LastfmArtistInfo Parse(XElement xmlResponse);
  }

  public class ArtistResponseParser : BaseResponseParser, IArtistResponseParser {
    private readonly ILfmNodeErrorParser lfmNodeErrorParser;

    public ArtistResponseParser() : this(new LfmNodeErrorParser()) { }

    public ArtistResponseParser(ILfmNodeErrorParser lfmNodeErrorParser) {
      this.lfmNodeErrorParser = lfmNodeErrorParser;
    }

    public LastfmArtistInfo Parse(XElement xmlResponse) {
      lfmNodeErrorParser.Parse(xmlResponse);
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
      if (bioNode == null) return null;
      var yearFormedElement = bioNode.Element("yearformed");
      return yearFormedElement != null ? (int?) int.Parse(yearFormedElement.Value.Trim()) : null;
    }
  }
}
