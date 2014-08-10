using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmUserTopArtist : LastfmUserItem {
    public int Rank;
    public int PlayCount;
    public string MegaImageLocation;
  }
}
