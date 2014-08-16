using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public class LibraryTrackRepository : LibraryRepository {
    public LibraryTrackRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser) : base(apiKey, restClient, parser) { }
    
    public LibraryTrackRepository(string apiKey) : base(apiKey) { }
    
    protected override string BaseUri {
      get { return LastfmUri.LibraryTracks; }
    }

    protected override LastfmResponse<LastfmLibraryItem> ParseItems(string uri) {
      return parser.ParseTracks(restClient.DownloadData(uri));
    }
  }
}
