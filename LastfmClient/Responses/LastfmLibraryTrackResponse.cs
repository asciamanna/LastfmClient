using System.Collections.Generic;

namespace LastfmClient.Responses {
  public class LastfmLibraryTrackResponse : LastfmResponse, ILastfmResponse {
    public List<LastfmLibraryTrack> Tracks = new List<LastfmLibraryTrack>();
  }

  public class LastfmLibraryTrack {
    public string Name;
    public string Artist;
    public string Album;
    public int PlayCount;
  }
}
