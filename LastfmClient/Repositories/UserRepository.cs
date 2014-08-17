using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Responses;

namespace LastfmClient.Repositories {
  public interface IUserRepository {
    List<LastfmUserItem> GetItems(string user, int numberOfItems);
  }
  public abstract class UserRepository : IUserRepository {
    private readonly string apiKey;
    protected readonly IRestClient restClient;
    protected readonly ILastfmResponseParser parser;
    private readonly IPageCalculator pageCalculator;

    public UserRepository(string apiKey) : this(apiKey, new RestClient(), new LastfmResponseParser(), new PageCalculator()) {
      this.apiKey = apiKey;
    }

    public UserRepository(string apiKey, IRestClient restClient, ILastfmResponseParser parser, IPageCalculator pageCalculator) {
      this.apiKey = apiKey;
      this.restClient = restClient;
      this.parser = parser;
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
