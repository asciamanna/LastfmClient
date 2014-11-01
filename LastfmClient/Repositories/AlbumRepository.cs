using System;
using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface IAlbumRepository {
    LastfmAlbumInfo GetInfo(string artist, string album);
  }

  public class AlbumRepository : IAlbumRepository {
    private readonly string apiKey;
    private readonly IRestClient restClient;
    private readonly IAlbumResponseParser parser;

    public AlbumRepository(string apiKey) : this(apiKey, new RestClient(), new AlbumResponseParser()) { }

    public AlbumRepository(string apiKey, IRestClient restClient, IAlbumResponseParser parser) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
    }

    public LastfmAlbumInfo GetInfo(string artist, string album) {
      var encodedArtist = Uri.EscapeDataString(artist);
      var encdodedAlbum = Uri.EscapeDataString(album);
      var uri = string.Format(LastfmUri.AlbumGetInfo, apiKey, encodedArtist, encdodedAlbum);
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
