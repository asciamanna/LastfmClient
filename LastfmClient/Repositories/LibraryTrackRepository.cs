using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryTrackRepository : LibraryRepository {
    private readonly IRestClient restClient;
    private readonly ILibraryResponseParser parser;
    
    public LibraryTrackRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) : base(apiKey) {
      this.restClient = restClient;
      this.parser = parser;
    }

    public LibraryTrackRepository(string apiKey) : this(apiKey, new RestClient(), new LibraryTracksResponseParser()) { }
    
    protected override string BaseUri {
      get { return LastfmUri.LibraryTracks; }
    }

    protected override LastfmResponse<LastfmLibraryItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
