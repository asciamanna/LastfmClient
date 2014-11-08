using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface IArtistRepository {
    LastfmArtistInfo GetInfo(string artist);
  }

  public class ArtistRepository : IArtistRepository {
    private readonly string apiKey;
    private readonly IRestClient restClient;
    private readonly IArtistResponseParser parser;

    public ArtistRepository(string apiKey) : this(apiKey, new RestClient(), new ArtistResponseParser()) { }

    public ArtistRepository(string apiKey, IRestClient restClient, IArtistResponseParser parser) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
    }

    public LastfmArtistInfo GetInfo(string artist) {
      var encodedArtist = Uri.EscapeDataString(artist);
       var uri = string.Format(LastfmUri.ArtistGetInfo, apiKey, encodedArtist);
       return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
