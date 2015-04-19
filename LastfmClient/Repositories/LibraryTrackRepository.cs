using LastfmClient.Parsers;

namespace LastfmClient.Repositories {
  public class LibraryTrackRepository : LibraryRepository {
    public LibraryTrackRepository(string apiKey) : this(apiKey, new RestClient(), new LibraryTracksResponseParser()) { }

    public LibraryTrackRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) : base(apiKey, restClient, parser) { }

    protected override string BaseUri {
      get { return LastfmUri.LibraryTracks; }
    }
  }
}
