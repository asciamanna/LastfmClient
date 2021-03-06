﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface ILibraryResponseParser {
    LastfmResponse<LastfmLibraryItem> Parse(XElement xmlResponse);
  }
  public class LibraryTracksResponseParser : BaseLibraryResponseParser, ILibraryResponseParser {
    protected override string CollectionElementName {
      get { return "tracks"; }
    }

    protected override string ItemElementName {
      get { return "track"; }
    }

    protected override IEnumerable<LastfmLibraryItem> CreateItems(IEnumerable<XElement> items) {
      return items.Where(t => t.Element("album") != null).Select(CreateLibraryTrack).ToList();
    }

    private static LastfmLibraryTrack CreateLibraryTrack(XElement track) {
      return new LastfmLibraryTrack {
        Name = track.Element("name").Value,
        Album = track.Element("album").Element("name").Value,
        Artist = track.Element("artist").Element("name").Value,
        PlayCount = Int32.Parse(track.Element("playcount").Value),
      };
    }
  }
}
