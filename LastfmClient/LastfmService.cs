using System;
using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface ILastfmService {
    List<LastfmLibraryTrack> FindAllTracks(string user);
    List<LastfmLibraryAlbum> FindAllAlbums(string user);
    List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks);
    List<LastfmUserTopArtist> FindTopArtists(string user, int numberOfArtists);
    LastfmPlayingFrom FindCurrentlyPlayingFrom(string user);
  }

  public class LastfmService : ILastfmService {
    readonly IRestClient restClient;
    readonly ILastfmResponseParser parser;
    readonly IPageCalculator pageCalculator;
    readonly ILastfmPageScraper pageScraper;
    readonly string apiKey;
    const string lastfmUserPageUrl = @"http://www.last.fm/user/";

    public LastfmService(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser(), new PageCalculator(), new LastfmPageScraper()) { }

    public LastfmService(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator, ILastfmPageScraper pageScraper) {
      this.restClient = restClient;
      this.parser = parser;
      this.pageCalculator = pageCalculator;
      this.pageScraper = pageScraper;

      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required");
      }
      this.apiKey = apiKey;
    }

    public List<LastfmLibraryTrack> FindAllTracks(string user) {
      var page = 1;
      var tracks = new List<LastfmLibraryTrack>();
      var uri = BuildUri(LastfmUri.LibraryTracks, user, page);
      var response = parser.ParseTracks(restClient.DownloadData(uri));
      tracks.AddRange(response.Tracks);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(LastfmUri.LibraryTracks, user, pageNum);
        tracks.AddRange(parser.ParseTracks(restClient.DownloadData(uri)).Tracks);
      }
      return tracks;
    }

    public List<LastfmLibraryAlbum> FindAllAlbums(string user) {
      var page = 1;
      var albums = new List<LastfmLibraryAlbum>();
      var uri = BuildUri(LastfmUri.LibraryAlbums, user, page);
      var response = parser.ParseAlbums(restClient.DownloadData(uri));
      albums.AddRange(response.Albums);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(LastfmUri.LibraryAlbums, user, pageNum);
        albums.AddRange(parser.ParseAlbums(restClient.DownloadData(uri)).Albums);
      }
      return albums;
    }

    public List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks) {
      FailFast.IfNotPositive(numberOfTracks, "numberOfTracks");
      var page = 1;
      var recentTracks = new List<LastfmUserRecentTrack>();
      var uri = BuildUri(LastfmUri.UserRecentTracks, user, page);
      var response = parser.ParseRecentTracks(restClient.DownloadData(uri));

      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfTracks);
      recentTracks.AddRange(response.Tracks);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(LastfmUri.UserRecentTracks, user, pageNum);
        recentTracks.AddRange(parser.ParseRecentTracks(restClient.DownloadData(uri)).Tracks);
      }
      return recentTracks.Take(numberOfTracks).ToList();
    }

    public List<LastfmUserTopArtist> FindTopArtists(string user, int numberOfArtists) {
      FailFast.IfNotPositive(numberOfArtists, "numberOfArtists");
      var page = 1;
      var topArtists = new List<LastfmUserTopArtist>();
      var uri = BuildUri(LastfmUri.UserTopArtists, user, page);
      var response = parser.ParseTopArtists(restClient.DownloadData(uri));

      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfArtists);
      topArtists.AddRange(response.TopArtists);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(LastfmUri.UserTopArtists, user, pageNum);
        topArtists.AddRange(parser.ParseTopArtists(restClient.DownloadData(uri)).TopArtists);
      }
      return topArtists.Take(numberOfArtists).ToList();
    }

    public LastfmPlayingFrom FindCurrentlyPlayingFrom(string user) {
      return pageScraper.GetLastfmPlayingFromInfo(lastfmUserPageUrl + user);
    }

    string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }
  }
}
