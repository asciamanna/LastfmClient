using LastfmClient.Responses;

namespace LastfmClient {
  public interface IPageCalculator {
    int Calculate<T>(ILastfmResponse<T> response, int numberOfTracks);
  }

  public class PageCalculator : IPageCalculator {
    public int Calculate<T>(ILastfmResponse<T> response, int numberOfTracks) {
      if (numberOfTracks > response.TotalRecords) {
        return response.TotalPages;
      }
      var page = 1;
      while (numberOfTracks > response.PerPage * page) {
        page++;
      }
      return page;
    }
  }
}
