using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryAlbumRepository : LibraryRepository {
    private readonly IRestClient restClient;
    private readonly ILibraryResponseParser parser;

    public LibraryAlbumRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) : base(apiKey) {
      this.restClient = restClient;
      this.parser = parser;
    }
    
    public LibraryAlbumRepository(string apiKey) : this(apiKey, new RestClient(), new LibraryAlbumsResponseParser()) { }

    protected override string BaseUri {
      get { return LastfmUri.LibraryAlbums; }
    }

    protected override LastfmResponse<LastfmLibraryItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
