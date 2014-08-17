using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class UserRecentTrackRepository : UserRepository {

    public UserRecentTrackRepository(string apiKey) : base(apiKey) { }

    public UserRecentTrackRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator) :
      base(apiKey, restClient, parser, pageCalculator) { }

    protected override string BaseUri {
      get { return LastfmUri.UserRecentTracks; }
    }

    protected override LastfmResponse<LastfmUserItem> ParseItems(string uri) {
      return parser.ParseRecentTracks(restClient.DownloadData(uri));
    }
  }
}
