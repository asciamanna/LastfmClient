using LastfmClient.Parsers;

namespace LastfmClient.Repositories {
  public class UserRecentTrackRepository : UserRepository {
    public UserRecentTrackRepository(string apiKey) : this(apiKey, new RestClient(), new UserRecentTracksResponseParser(), new PageCalculator()) { }

    public UserRecentTrackRepository(string apiKey, IRestClient restClient, IUserResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, restClient, parser, pageCalculator) { }

    protected override string BaseUri {
      get { return LastfmUri.UserRecentTracks; }
    }
  }
}
