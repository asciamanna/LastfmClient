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
      var response = GetFirstPageOfItems(user);
      var lastfmItems = response.Items.ToList();
      var totalPages = response.TotalPages;
      GetRemainingPagesOfItems(user, lastfmItems, totalPages);
      return lastfmItems;
    }

    private LastfmResponse<LastfmLibraryItem> GetFirstPageOfItems(string user) {
      return ParseItems(BuildUri(BaseUri, user, page: 1));
    }

    private void GetRemainingPagesOfItems(string user, List<LastfmLibraryItem> lastfmItems, int totalPages) {
      foreach (var pageNum in Enumerable.Range(2, totalPages - 1)) {
        lastfmItems.AddRange(ParseItems(BuildUri(BaseUri, user, pageNum)).Items);
      }
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
