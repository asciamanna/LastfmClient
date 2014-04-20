using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface IPageCalculator {
    int Calculate(LastfmUserRecentTracksResponse response, int numberOfTracks);
  }

  public class PageCalculator : IPageCalculator {
    public int Calculate(LastfmUserRecentTracksResponse response, int numberOfTracks) {
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
