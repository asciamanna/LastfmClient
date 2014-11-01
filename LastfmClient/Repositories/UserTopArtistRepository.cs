using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class UserTopArtistRepository : UserRepository {
    private readonly IRestClient restClient;
    private readonly IUserResponseParser parser;

    public UserTopArtistRepository(string apiKey) : this(apiKey, new RestClient(), new UserTopArtistResponseParser(), new PageCalculator()) { }

    public UserTopArtistRepository(string apiKey, IRestClient restClient, IUserResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, pageCalculator) {
        this.restClient = restClient;
        this.parser = parser;
    }

    protected override string BaseUri {
      get { return LastfmUri.UserTopArtists; }
    }

    protected override LastfmResponse<LastfmUserItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
