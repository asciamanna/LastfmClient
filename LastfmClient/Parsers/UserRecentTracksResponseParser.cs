﻿using System;
using System.Collections.Generic;
using System.Linq;
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
      return items.Select(CreateUserRecentTrack).ToList();
    }

    private LastfmUserRecentTrack CreateUserRecentTrack(XElement trackElement) {
      return new LastfmUserRecentTrack {
        IsNowPlaying = ParseNowPlayingAttribute(trackElement),
        Name = trackElement.Element("name").Value,
        Album = trackElement.Element("album").Value,
        Artist = trackElement.Element("artist").Value,
        ExtraLargeImageLocation = ParseImageLocation(trackElement, "extralarge"),
        LargeImageLocation = ParseImageLocation(trackElement, "large"),
        MediumImageLocation = ParseImageLocation(trackElement, "medium"),
        SmallImageLocation = ParseImageLocation(trackElement, "small"),
        LastPlayed = ParseDateAsUTC(trackElement)
      };
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
