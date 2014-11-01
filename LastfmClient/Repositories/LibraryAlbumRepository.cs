using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryAlbumRepository : LibraryRepository {
    public LibraryAlbumRepository(string apiKey) : this(apiKey, new RestClient(), new LibraryAlbumsResponseParser()) { }

    public LibraryAlbumRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) : base(apiKey, restClient, parser) { }
    
    protected override string BaseUri {
      get { return LastfmUri.LibraryAlbums; }
    }
  }
}
