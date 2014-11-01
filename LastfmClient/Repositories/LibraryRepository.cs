using System.Collections.Generic;
using System.Linq;
using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface ILibraryRepository {
    List<LastfmLibraryItem> GetItems(string user);
  }

  public abstract class LibraryRepository : ILibraryRepository {
    private readonly string apiKey;
    private readonly IRestClient restClient;
    private readonly ILibraryResponseParser parser;

    protected LibraryRepository(string apiKey, IRestClient restClient, ILibraryResponseParser parser) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
    }

    public List<LastfmLibraryItem> GetItems(string user) {
      var page = 1;
      var lastfmItems = new List<LastfmLibraryItem>();
      var uri = BuildUri(BaseUri, user, page);
      var response = ParseItems(uri);
      lastfmItems.AddRange(response.Items);
      var totalPages = response.TotalPages;
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        uri = BuildUri(BaseUri, user, pageNum);
        lastfmItems.AddRange(ParseItems(uri).Items);
      }
      return lastfmItems;
    }
    protected abstract string BaseUri { get; }

    private string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }

    private LastfmResponse<LastfmLibraryItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }
  }
}
