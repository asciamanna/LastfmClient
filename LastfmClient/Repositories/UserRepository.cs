using System.Collections.Generic;
using System.Linq;
using LastfmClient.Parsers;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface IUserRepository {
    List<LastfmUserItem> GetItems(string user, int numberOfItems);
  }
  public abstract class UserRepository : IUserRepository {
    private readonly string apiKey;
    private readonly IRestClient restClient;
    private readonly IUserResponseParser parser;
    private readonly IPageCalculator pageCalculator;

    protected UserRepository(string apiKey, IRestClient restClient, IUserResponseParser parser, IPageCalculator pageCalculator) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
      this.pageCalculator = pageCalculator;
    }

    public List<LastfmUserItem> GetItems(string user, int numberOfItems) {
      var response = GetFirstPageOfItems(user);
      var items = response.Items.ToList();
      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfItems);
      GetRemainingPagesOfItems(user, items, numberOfPagesToRetrieve);
      return items.Take(numberOfItems).ToList();
    }

    private void GetRemainingPagesOfItems(string user, List<LastfmUserItem> items, int numberOfPagesToRetrieve) {
      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        items.AddRange(ParseItems(BuildUri(BaseUri, user, pageNum)).Items);
      }
    }

    private LastfmResponse<LastfmUserItem> GetFirstPageOfItems(string user) {
      var response = ParseItems(BuildUri(BaseUri, user, page: 1));
      return response;
    }

    protected abstract string BaseUri { get; }

    private LastfmResponse<LastfmUserItem> ParseItems(string uri) {
      return parser.Parse(restClient.DownloadData(uri));
    }

    string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }
  }
}
