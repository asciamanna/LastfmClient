using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface ILastfmResponseParser {
    LastfmLibraryTrackResponse ParseTracks(XElement xmlResponse);
    LastfmLibraryAlbumResponse ParseAlbums(XElement xmlResponse);
    LastfmUserRecentTracksResponse ParseRecentTracks(XElement xmlResponse);
    LastfmUserTopArtistsResponse ParseTopArtists(XElement xmlResponse);
  }

  public class LastfmResponseParser : ILastfmResponseParser {
    public LastfmLibraryTrackResponse ParseTracks(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf("tracks");
      var tracksElement = tracks.First();

      return new LastfmLibraryTrackResponse {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Tracks = BuildTracks(tracks.Descendants("track")).ToList(),
      };
    }
    
    IEnumerable<LastfmLibraryTrack> BuildTracks(IEnumerable<XElement> tracks) {
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

    public LastfmLibraryAlbumResponse ParseAlbums(XElement xmlResponse) {
      var albums = xmlResponse.DescendantsAndSelf("albums");
      var albumsElement = albums.First();
    
      return new LastfmLibraryAlbumResponse {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(albumsElement.Attribute("page").Value),
        PerPage = Int32.Parse(albumsElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(albumsElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(albumsElement.Attribute("total").Value),
        Albums = BuildAlbums(albumsElement.Descendants("album")).ToList(),
      };
    }

    IEnumerable<LastfmLibraryAlbum> BuildAlbums(IEnumerable<XElement> albums) {
      var libraryAlbums = new List<LastfmLibraryAlbum>();
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

    public LastfmUserRecentTracksResponse ParseRecentTracks(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf("recenttracks");
      var tracksElement = tracks.First();

      return new LastfmUserRecentTracksResponse {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Tracks = BuildRecentTracks(tracksElement.Descendants("track")).ToList(),
      };
    }

    IEnumerable<LastfmUserRecentTrack> BuildRecentTracks(IEnumerable<XElement> tracks) {
      var recentTracks = new List<LastfmUserRecentTrack>();
      foreach (var track in tracks) {
        recentTracks.Add(new LastfmUserRecentTrack {
          IsNowPlaying = ParseNowPlayingAttribute(track),
          Name = track.Element("name").Value,
          Album = track.Element("album").Value,
          Artist = track.Element("artist").Value,
          ExtraLargeAlbumArtLocation = ParseImageLocation(track, "extralarge"),
          LargeAlbumArtLocation = ParseImageLocation(track, "large"),
          MediumAlbumArtLocation = ParseImageLocation(track, "medium"),
          SmallAlbumArtLocation = ParseImageLocation(track, "small"),
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

    private static string ParseImageLocation(XElement element, string sizeAttribute) {
      return element.Elements("image").Where(e => e.Attribute("size").Value == sizeAttribute).FirstOrDefault().Value.Trim();
    }

    public LastfmUserTopArtistsResponse ParseTopArtists(XElement xmlResponse) {
      var artists = xmlResponse.DescendantsAndSelf("topartists");
      var artistsElement = artists.First();

      return new LastfmUserTopArtistsResponse {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(artistsElement.Attribute("page").Value),
        PerPage = Int32.Parse(artistsElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(artistsElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(artistsElement.Attribute("total").Value),
        TopArtists = BuildTopArtists(artistsElement.Descendants("artist")).ToList(),
      };
    }

    IEnumerable<LastfmUserTopArtist> BuildTopArtists(IEnumerable<XElement> topArtistsElement) {
      var topArtists = new List<LastfmUserTopArtist>();
      foreach (var artistElement in topArtistsElement) {
        topArtists.Add(new LastfmUserTopArtist {
          Rank = Int32.Parse(artistElement.Attribute("rank").Value),
          Name = artistElement.Element("name").Value,
          PlayCount = Int32.Parse(artistElement.Element("playcount").Value),
          MegaArtistImageLocation = ParseImageLocation(artistElement, "mega"),
          ExtraLargeArtistImageLocation = ParseImageLocation(artistElement, "extralarge"),
          LargeArtistImageLocation = ParseImageLocation(artistElement, "large"),
          MediumArtistImageLocation = ParseImageLocation(artistElement, "medium"),
          SmallArtistImageLocation = ParseImageLocation(artistElement, "small"),
        });
      }
      return topArtists;
    }

    static DateTime? ParseDateAsUTC(XElement track) {
      if (track.Element("date") == null) {
        return null;
      }
      var date = DateTime.Parse(track.Element("date").Value);
      return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
  }
}
