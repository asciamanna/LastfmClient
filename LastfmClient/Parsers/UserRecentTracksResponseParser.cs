using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface IUserResponseParser {
    LastfmResponse<LastfmUserItem> Parse(XElement xmlResponse);
  }
  public class UserRecentTracksResponseParser : BaseUserResponseParser, IUserResponseParser {
    protected override string CollectionElementName {
      get { return "recenttracks"; }
    }

    protected override string ItemElementName {
      get { return "track"; }
    }

    protected override IEnumerable<LastfmUserItem> CreateItems(IEnumerable<XElement> items) {
      var recentTracks = new List<LastfmUserRecentTrack>();
      foreach (var track in items) {
        recentTracks.Add(new LastfmUserRecentTrack {
          IsNowPlaying = ParseNowPlayingAttribute(track),
          Name = track.Element("name").Value,
          Album = track.Element("album").Value,
          Artist = track.Element("artist").Value,
          ExtraLargeImageLocation = ParseImageLocation(track, "extralarge"),
          LargeImageLocation = ParseImageLocation(track, "large"),
          MediumImageLocation = ParseImageLocation(track, "medium"),
          SmallImageLocation = ParseImageLocation(track, "small"),
          LastPlayed = ParseDateAsUTC(track)
        });
      }
      return recentTracks;
    }

    private static bool ParseNowPlayingAttribute(XElement track) {
      if (track.Attribute("nowplaying") != null) {
        return track.Attribute("nowplaying").Value == "true";
      }
      return false;
    }

    private static DateTime? ParseDateAsUTC(XElement track) {
      if (track.Element("date") == null) {
        return null;
      }
      var date = DateTime.Parse(track.Element("date").Value);
      return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
  }
}
