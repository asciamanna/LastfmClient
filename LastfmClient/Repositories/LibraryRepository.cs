using System.Collections.Generic;
using System.Linq;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface ILibraryRepository {
    List<LastfmLibraryItem> GetItems(string user); 
  }

  public abstract class LibraryRepository : ILibraryRepository {
    private readonly string apiKey;
    protected readonly IRestClient restClient;
    protected readonly ILastfmResponseParser parser;

    protected LibraryRepository(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser()) { }

    protected LibraryRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser) {
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

    private string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }

    protected abstract string BaseUri { get; }

    protected abstract LastfmResponse<LastfmLibraryItem> ParseItems(string uri);
  }
}
