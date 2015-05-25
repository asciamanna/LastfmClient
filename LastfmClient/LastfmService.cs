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
    LastfmMusicSource FindMusicSource(string user);
    LastfmAlbumInfo FindAlbumInfo(string artist, string album);
    LastfmArtistInfo FindArtistInfo(string artist);
  }

  public class LastfmService : ILastfmService {
    readonly ILastfmPageScraper pageScraper;
    readonly IRepositoryFactory repositoryFactory;
    readonly string apiKey;
    const string LastfmUserPageUrl = @"http://www.last.fm/user/";

    public LastfmService(string apiKey) : this(apiKey, new LastfmPageScraper(), new RepositoryFactory()) { }

    public LastfmService(string apiKey, ILastfmPageScraper pageScraper, IRepositoryFactory repositoryFactory) {
      if (string.IsNullOrEmpty(apiKey)) {
        throw new ArgumentException("An API key is required");
      }
      this.apiKey = apiKey;
      this.pageScraper = pageScraper;
      this.repositoryFactory = repositoryFactory;
    }

    public List<LastfmLibraryTrack> FindAllTracks(string user) {
      var libraryTrackRepository = repositoryFactory.CreateLibraryTrackRepository(apiKey);
      return libraryTrackRepository.GetItems(user).Cast<LastfmLibraryTrack>().ToList();
    }

    public List<LastfmLibraryAlbum> FindAllAlbums(string user) {
      var libraryAlbumRepository = repositoryFactory.CreateLibraryAlbumRepository(apiKey);
      return libraryAlbumRepository.GetItems(user).Cast<LastfmLibraryAlbum>().ToList();
    }

    public List<LastfmUserRecentTrack> FindRecentTracks(string user, int numberOfTracks) {
      FailFast.IfNotPositive(numberOfTracks, "numberOfTracks");
      var userRecentTrackRepository = repositoryFactory.CreateUserRecentTrackRepository(apiKey);
      return userRecentTrackRepository.GetItems(user, numberOfTracks).Cast<LastfmUserRecentTrack>().ToList();
    }

    public List<LastfmUserTopArtist> FindTopArtists(string user, int numberOfArtists) {
      FailFast.IfNotPositive(numberOfArtists, "numberOfArtists");
      var userTopArtistRepository = repositoryFactory.CreateUserTopArtistRepository(apiKey);
      return userTopArtistRepository.GetItems(user, numberOfArtists).Cast<LastfmUserTopArtist>().ToList();
    }

    public LastfmMusicSource FindMusicSource(string user) {
      return pageScraper.GetLastfmMusicSource(LastfmUserPageUrl + user);
    }

    public LastfmAlbumInfo FindAlbumInfo(string artist, string album) {
      var albumInfoRepository = repositoryFactory.CreateAlbumRepository(apiKey);
      return albumInfoRepository.GetInfo(artist, album);
    }

    public LastfmArtistInfo FindArtistInfo(string artist) {
      var artistInfoRepository = repositoryFactory.CreateArtistRepository(apiKey);
      return artistInfoRepository.GetInfo(artist);
    }
  }
}
