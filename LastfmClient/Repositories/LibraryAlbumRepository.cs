using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryAlbumRepository : LibraryRepository {
    public LibraryAlbumRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser) : base(apiKey, restClient, parser) { }
    
    public LibraryAlbumRepository(string apiKey) : base(apiKey) { }

    protected override string BaseUri {
      get { return LastfmUri.LibraryAlbums; }
    }

    protected override LastfmResponse<LastfmLibraryItem> ParseItems(string uri) {
      return parser.ParseAlbums(restClient.DownloadData(uri));
    }
  }
}
