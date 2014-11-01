using System;

namespace LastfmClient.Responses {
  public class LastfmUserRecentTrack : LastfmUserItem {
    public bool IsNowPlaying = false;
    public string Artist;
    public string Album;
    public DateTime? LastPlayed;
  }
}
