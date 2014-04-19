using System;
using System.Collections.Generic;
using System.Linq;

namespace LastfmClient {
  public interface ILastfmService {
    List<LastfmLibraryTrack> FindAllTracks(string user);
    List<LastfmLibraryAlbum> FindAllAlbums(string user);
    List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks);
  }

  public class LastfmService : ILastfmService {
    readonly IRestClient restClient;
    readonly ILastfmResponseParser parser;
    readonly IPageCalculator pageCalculator;
    readonly string apiKey;
    const string libraryTracksUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key={0}&user={1}&page={2}";
    const string libraryAlbumsUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key={0}&user={1}&page={2}";
    const string userRecentTracksUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user={1}&api_key={0}&page={2}";

    public LastfmService(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser(), new PageCalculator()) { }

    public LastfmService(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator) {
      this.restClient = restClient;
      this.parser = parser;
      this.pageCalculator = pageCalculator;

      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required.");
      }
      this.apiKey = apiKey;
    }

    public List<LastfmLibraryTrack> FindAllTracks(string user) {
      var page = 1;
      var tracks = new List<LastfmLibraryTrack>();
      var uri = BuildUri(libraryTracksUri, user, page);
      var response = parser.ParseTracks(restClient.DownloadData(uri));
      tracks.AddRange(response.Tracks);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(libraryTracksUri, user, pageNum);
        tracks.AddRange(parser.ParseTracks(restClient.DownloadData(uri)).Tracks);
      }
      return tracks;
    }

    public List<LastfmLibraryAlbum> FindAllAlbums(string user) {
      var page = 1;
      var albums = new List<LastfmLibraryAlbum>();
      var uri = BuildUri(libraryAlbumsUri, user, page);
      var response = parser.ParseAlbums(restClient.DownloadData(uri));
      albums.AddRange(response.Albums);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(libraryAlbumsUri, user, pageNum);
        albums.AddRange(parser.ParseAlbums(restClient.DownloadData(uri)).Albums);
      }
      return albums;
    }

    public List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks) {
      if (numberOfTracks < 1) {
        throw new ArgumentException("numberOfTracks must be greater than 0");
      }

      var page = 1;
      var recentTracks = new List<LastfmUserRecentTrack>();
      var uri = BuildUri(userRecentTracksUri, user, page);
      var response = parser.ParseRecentTracks(restClient.DownloadData(uri));

      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfTracks);
      recentTracks.AddRange(response.Tracks);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(userRecentTracksUri, user, pageNum);
        recentTracks.AddRange(parser.ParseRecentTracks(restClient.DownloadData(uri)).Tracks);
      }
      return recentTracks.Take(numberOfTracks).ToList();
    }

    string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }
  }
}
