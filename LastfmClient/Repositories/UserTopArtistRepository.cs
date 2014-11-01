using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class UserTopArtistRepository : UserRepository {

    public UserTopArtistRepository(string apiKey) : base(apiKey) { }

    public UserTopArtistRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, restClient, parser, pageCalculator) { }

    protected override string BaseUri {
      get { return LastfmUri.UserTopArtists; }
    }

    protected override LastfmResponse<LastfmUserItem> ParseItems(string uri) {
      return parser.ParseTopArtists(restClient.DownloadData(uri));
    }
  }
}
