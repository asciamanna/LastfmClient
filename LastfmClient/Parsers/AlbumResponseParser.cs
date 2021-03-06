﻿using System;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface IAlbumResponseParser {
    LastfmAlbumInfo Parse(XElement xmlResponse);
  }

  public class AlbumResponseParser : IAlbumResponseParser {
    private readonly ILfmNodeErrorParser errorParser;

    public AlbumResponseParser() : this(new LfmNodeErrorParser()) { }

    public AlbumResponseParser(ILfmNodeErrorParser errorParser) {
      this.errorParser = errorParser;
    }

    public LastfmAlbumInfo Parse(XElement xmlResponse) {
      errorParser.Parse(xmlResponse);
      var albumInfoElement = xmlResponse.DescendantsAndSelf("album").First();
      return CreateAlbumInfo(albumInfoElement);
    }

    private static LastfmAlbumInfo CreateAlbumInfo(XElement albumInfoElement) {
      return new LastfmAlbumInfo {
        Name = albumInfoElement.Element("name").Value,
        Artist = albumInfoElement.Element("artist").Value,
        Mbid = albumInfoElement.Element("mbid").Value,
        ReleaseDate = ParseDateString(albumInfoElement.Element("releasedate").Value),
        WikiSummary = albumInfoElement.Descendants("wiki").First().Element("summary").Value.Trim()
      };
    }
    
    private static DateTime? ParseDateString(string date) {
      return string.IsNullOrWhiteSpace(date) ? (DateTime?) null : DateTime.Parse(date);
    }
  }
}
