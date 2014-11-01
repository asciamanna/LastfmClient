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
    private readonly IPageCalculator pageCalculator;

    protected UserRepository(string apiKey, IPageCalculator pageCalculator) {
      this.apiKey = apiKey;
      this.pageCalculator = pageCalculator;
    }

    public List<LastfmUserItem> GetItems(string user, int numberOfItems) {
      var page = 1;
      var items = new List<LastfmUserItem>();
      var uri = BuildUri(BaseUri, user, page);
      var response = ParseItems(uri);

      var numberOfPagesToRetrieve = pageCalculator.Calculate(response, numberOfItems);
      items.AddRange(response.Items);

      foreach (var pageNum in Enumerable.Range(2, numberOfPagesToRetrieve - 1)) {
        uri = BuildUri(BaseUri, user, pageNum);
        items.AddRange(ParseItems(uri).Items);
      }
      return items.Take(numberOfItems).ToList();
    }

    protected abstract string BaseUri { get; }

    protected abstract LastfmResponse<LastfmUserItem> ParseItems(string uri);
    
    //refactor buildUri -- move to URI class.
    string BuildUri(string baseUri, string user, int page) {
      return string.Format(baseUri, apiKey, user, page);
    }
  }
}
