using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class UserRecentTrackRepository : UserRepository {
    private readonly IRestClient restClient;
    private readonly IUserResponseParser parser;

    public UserRecentTrackRepository(string apiKey) : this(apiKey, new RestClient(), new UserRecentTracksResponseParser(), new PageCalculator()) { }

    public UserRecentTrackRepository(string apiKey, IRestClient restClient, IUserResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, pageCalculator) {
      this.restClient = restClient;
      this.parser = parser;
    }

    protected override string BaseUri {
      get { return LastfmUri.UserRecentTracks; }
    }

    protected override LastfmResponse<LastfmUserItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
