using System;
using System.Collections.Generic;

namespace LastfmClient.Responses {
  public class LastfmUserRecentTracksResponse : LastfmResponse, ILastfmResponse {
    public List<LastfmUserRecentTrack> Tracks = new List<LastfmUserRecentTrack>();
  }

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
