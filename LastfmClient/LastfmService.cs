using System;
using System.Collections.Generic;
using System.Linq;
using LastfmClient.Repositories;
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
    readonly IRepositoryFactory repositoryFactory;
    readonly string apiKey;
    const string lastfmUserPageUrl = @"http://www.last.fm/user/";

    public LastfmService(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser(), new PageCalculator(), new LastfmPageScraper(), new RepositoryFactory()) { }

    public LastfmService(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator, ILastfmPageScraper pageScraper, IRepositoryFactory repositoryFactory) {
      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required");
      }

      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
      this.pageCalculator = pageCalculator;
      this.pageScraper = pageScraper;
      this.repositoryFactory = repositoryFactory;
    }

    public List<LastfmLibraryTrack> FindAllTracks(string user) {
      var libraryTracksRepository = repositoryFactory.CreateLibraryTracksRepository(apiKey);
      return libraryTracksRepository.GetItems(user).Cast<LastfmLibraryTrack>().ToList();
    }

    public List<LastfmLibraryAlbum> FindAllAlbums(string user) {
      var libraryAlbumsRepository = repositoryFactory.CreateLibraryAlbumsRepository(apiKey);
      return libraryAlbumsRepository.GetItems(user).Cast<LastfmLibraryAlbum>().ToList();
    }

    public List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks) {
      FailFast.IfNotPositive(numberOfTracks, "numberOfTracks");
      var page = 1;
      var recentTracks = new List<LastfmUserRecentTrack>();
      var uri = BuildUri(LastfmUri.UserRecentTracks, user, page);
      var response = parser.ParseRecentTracks(restClient.DownloadData(uri));

      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfTracks);
      recentTracks.AddRange(response.Items);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(LastfmUri.UserRecentTracks, user, pageNum);
        recentTracks.AddRange(parser.ParseRecentTracks(restClient.DownloadData(uri)).Items);
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
      topArtists.AddRange(response.Items);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(LastfmUri.UserTopArtists, user, pageNum);
        topArtists.AddRange(parser.ParseTopArtists(restClient.DownloadData(uri)).Items);
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
