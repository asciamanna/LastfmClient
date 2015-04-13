
using LastfmClient.Responses;
using System.Linq;

namespace LastfmClientTests {
  public static class TestHelper {
    public static string TestFilePath { get { return "LastfmTestResponses/"; } }

    public static LastfmResponse<LastfmUserItem> CreateLastfmUserItemResponse<T>(params string[] names) where T : LastfmUserItem, new() {
      var items = names.Select(an => new T { Name = an }).ToList();
      return new LastfmResponse<LastfmUserItem> { Items = items };
    }
  }
}
