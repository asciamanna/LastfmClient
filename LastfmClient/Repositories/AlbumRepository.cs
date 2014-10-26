using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface IAlbumRepository {
    LastfmAlbumInfo GetInfo(string artist, string album);
  }

  public class AlbumRepository : IAlbumRepository {
    private readonly string apiKey;
    private readonly IRestClient restClient;
    private readonly ILastfmResponseParser parser;

    public AlbumRepository(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser()) { }

    public AlbumRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
    }
    public LastfmAlbumInfo GetInfo(string artist, string album) {
      var encodedArtist = Uri.EscapeDataString(artist);
      var encdodedAlbum = Uri.EscapeDataString(album);
      var uri = string.Format(LastfmUri.AlbumGetInfo, apiKey, encodedArtist, encdodedAlbum);
      return parser.ParseAlbumInfo(restClient.DownloadData(uri));
    }
  }
}
