using System;
using System.Collections.Generic;
using System.Linq;

namespace LastfmClient {
  public interface ILastfmService {
    List<LastfmLibraryTrack> FindAllTracks(string user);
  }

  public class LastfmService : ILastfmService {
    readonly IRestClient restClient;
    readonly ILastfmResponseParser parser;
    readonly string apiKey;
    const string libraryTracksUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key={0}&user={1}&page={2}";

    public LastfmService(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser()) { }

    public LastfmService(string apiKey, IRestClient restClient, ILastfmResponseParser parser) {
      this.restClient = restClient;
      this.parser = parser;

      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required.");
      }
      this.apiKey = apiKey;
    }

    public List<LastfmLibraryTrack> FindAllTracks(string user) {
      var page = 1;
      var tracks = new List<LastfmLibraryTrack>();
      var uri = BuildUri(user, page);
      var response = parser.Parse(restClient.DownloadData(uri));
      tracks.AddRange(response.Tracks);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(user, pageNum);
        tracks.AddRange(parser.Parse(restClient.DownloadData(uri)).Tracks);
      }
      return tracks;
    }

    string BuildUri(string user, int page) {
      return string.Format(libraryTracksUri, apiKey, user, page);
    }
  }
}
