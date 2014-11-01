using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Parsers {
  public interface ILastfmResponseParser {
    LastfmResponse<LastfmLibraryItem> ParseTracks(XElement xmlResponse);
    LastfmResponse<LastfmLibraryItem> ParseAlbums(XElement xmlResponse);
    LastfmResponse<LastfmUserItem> ParseRecentTracks(XElement xmlResponse);
    LastfmResponse<LastfmUserItem> ParseTopArtists(XElement xmlResponse);
    LastfmAlbumInfo ParseAlbumInfo(XElement xmlResponse);
  }

  public class LastfmResponseParser : ILastfmResponseParser {
    public LastfmResponse<LastfmLibraryItem> ParseTracks(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf("tracks");
      var tracksElement = tracks.First();

      return new LastfmResponse<LastfmLibraryItem> {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Items = BuildTracks(tracks.Descendants("track")).ToList(),
      };
    }

    private IEnumerable<LastfmLibraryItem> BuildTracks(IEnumerable<XElement> tracks) {
      var libraryTracks = new List<LastfmLibraryTrack>();
      foreach (var track in tracks.Where(t => t.Element("album") != null)) {
        libraryTracks.Add(new LastfmLibraryTrack {
          Name = track.Element("name").Value,
          Album = track.Element("album").Element("name").Value,
          Artist = track.Element("artist").Element("name").Value,
          PlayCount = Int32.Parse(track.Element("playcount").Value),
        });
      }
      return libraryTracks;
    }

    public LastfmResponse<LastfmLibraryItem> ParseAlbums(XElement xmlResponse) {
      var albums = xmlResponse.DescendantsAndSelf("albums");
      var albumsElement = albums.First();

      return new LastfmResponse<LastfmLibraryItem> {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(albumsElement.Attribute("page").Value),
        PerPage = Int32.Parse(albumsElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(albumsElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(albumsElement.Attribute("total").Value),
        Items = BuildAlbums(albumsElement.Descendants("album")).ToList(),
      };
    }

    private IEnumerable<LastfmLibraryItem> BuildAlbums(IEnumerable<XElement> albums) {
      var libraryAlbums = new List<LastfmLibraryItem>();
      foreach (var album in albums) {
        libraryAlbums.Add(new LastfmLibraryAlbum {
          Name = album.Element("name").Value,
          Artist = album.Element("artist").Element("name").Value,
          PlayCount = Int32.Parse(album.Element("playcount").Value),
          ArtworkLocation = album.Elements("image").Where(e => e.Attribute("size").Value == "extralarge").FirstOrDefault().Value
        });
      }
      return libraryAlbums;
    }

    public LastfmResponse<LastfmUserItem> ParseRecentTracks(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf("recenttracks");
      var tracksElement = tracks.First();

      return new LastfmResponse<LastfmUserItem> {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Items = BuildRecentTracks(tracksElement.Descendants("track")).ToList(),
      };
    }

    private IEnumerable<LastfmUserItem> BuildRecentTracks(IEnumerable<XElement> tracks) {
      var recentTracks = new List<LastfmUserRecentTrack>();
      foreach (var track in tracks) {
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

    public LastfmAlbumInfo ParseAlbumInfo(XElement xmlResponse) {
      var albumInfo = xmlResponse.DescendantsAndSelf("album").First();
      return new LastfmAlbumInfo {
        Name = albumInfo.Element("name").Value,
        Artist = albumInfo.Element("artist").Value,
        Mbid = albumInfo.Element("mbid").Value,
        ReleaseDate = ParseDateString(albumInfo.Element("releasedate").Value),
        WikiSummary = albumInfo.Descendants("wiki").First().Element("summary").Value.Trim()
      };
    }

    private static bool ParseNowPlayingAttribute(XElement track) {
      if (track.Attribute("nowplaying") != null) {
        return track.Attribute("nowplaying").Value == "true";
      }
      return false;
    }

    private static string ParseImageLocation(XElement element, string sizeAttribute) {
      return element.Elements("image").Where(e => e.Attribute("size").Value == sizeAttribute).FirstOrDefault().Value.Trim();
    }

    public LastfmResponse<LastfmUserItem> ParseTopArtists(XElement xmlResponse) {
      var artists = xmlResponse.DescendantsAndSelf("topartists");
      var artistsElement = artists.First();

      return new LastfmResponse<LastfmUserItem> {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(artistsElement.Attribute("page").Value),
        PerPage = Int32.Parse(artistsElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(artistsElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(artistsElement.Attribute("total").Value),
        Items = BuildTopArtists(artistsElement.Descendants("artist")).ToList(),
      };
    }

    private IEnumerable<LastfmUserItem> BuildTopArtists(IEnumerable<XElement> topArtistsElement) {
      var topArtists = new List<LastfmUserItem>();
      foreach (var artistElement in topArtistsElement) {
        topArtists.Add(new LastfmUserTopArtist {
          Rank = Int32.Parse(artistElement.Attribute("rank").Value),
          Name = artistElement.Element("name").Value,
          PlayCount = Int32.Parse(artistElement.Element("playcount").Value),
          MegaImageLocation = ParseImageLocation(artistElement, "mega"),
          ExtraLargeImageLocation = ParseImageLocation(artistElement, "extralarge"),
          LargeImageLocation = ParseImageLocation(artistElement, "large"),
          MediumImageLocation = ParseImageLocation(artistElement, "medium"),
          SmallImageLocation = ParseImageLocation(artistElement, "small"),
        });
      }
      return topArtists;
    }

    private static DateTime? ParseDateAsUTC(XElement track) {
      if (track.Element("date") == null) {
        return null;
      }
      var date = DateTime.Parse(track.Element("date").Value);
      return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }

    private static DateTime? ParseDateString(string date) {
      if (string.IsNullOrWhiteSpace(date)) {
        return null;
      }
      return DateTime.Parse(date);
    }
  }
}
