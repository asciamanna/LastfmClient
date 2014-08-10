using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmUserRecentTrack {
    public bool IsNowPlaying = false;
    public string Name;
    public string Artist;
    public string Album;
    public DateTime? LastPlayed;
    public string ExtraLargeAlbumArtLocation;
    public string LargeAlbumArtLocation;
    public string MediumAlbumArtLocation;
    public string SmallAlbumArtLocation;
  }
}
