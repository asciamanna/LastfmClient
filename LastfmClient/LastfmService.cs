using System;
using System.Collections.Generic;
using System.Linq;

namespace LastfmClient {
  public interface ILastfmService {
    List<LastfmLibraryTrack> FindAllTracks(string user);
    List<LastfmLibraryAlbum> FindAllAlbums(string user);
  }

  public class LastfmService : ILastfmService {
    readonly IRestClient restClient;
    readonly ILastfmResponseParser parser;
    readonly string apiKey;
    const string libraryTracksUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key={0}&user={1}&page={2}";
    const string libraryAlbumsUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key={0}&user={1}&page={2}";

    public LastfmService(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser()) { }

    public LastfmService(string apiKey, IRestClient restClient, ILastfmResponseParser parser) {
      this.restClient = restClient;
      this.parser = parser;

      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required.");
      }
      this.apiKey = apiKey;
    }
    
    //TODO refactor commonality out of the FindAll methods

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

    string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }
  }
}
