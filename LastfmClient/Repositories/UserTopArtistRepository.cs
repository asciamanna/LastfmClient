using LastfmClient.Parsers;

namespace LastfmClient.Repositories {
  public class UserTopArtistRepository : UserRepository {
    public UserTopArtistRepository(string apiKey) : this(apiKey, new RestClient(), new UserTopArtistResponseParser(), new PageCalculator()) { }

    public UserTopArtistRepository(string apiKey, IRestClient restClient, IUserResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, restClient, parser, pageCalculator) { }

    protected override string BaseUri {
      get { return LastfmUri.UserTopArtists; }
    }
  }
}
