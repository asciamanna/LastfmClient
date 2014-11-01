using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryAlbumRepository : LibraryRepository {
    private readonly IRestClient restClient;
    private readonly ILibraryResponseParser parser;
    
    public LibraryAlbumRepository(string apiKey) : this(apiKey, new RestClient(), new LibraryAlbumsResponseParser()) { }

    public LibraryAlbumRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) : base(apiKey, restClient, parser) {
      this.restClient = restClient;
      this.parser = parser;
    }
    
    protected override string BaseUri {
      get { return LastfmUri.LibraryAlbums; }
    }
  }
}
