using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmUserTopArtistsResponse : LastfmLibraryResponse {
    public List<LastfmUserTopArtist> TopArtists = new List<LastfmUserTopArtist>();
  }

  public class LastfmUserTopArtist {
    public string Name;
    public int Rank;
    public int PlayCount;
    public string ArtistImageLocation;
  }
}
