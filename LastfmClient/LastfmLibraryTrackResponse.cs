using System.Collections.Generic;

namespace LastfmClient {
  public class LastfmLibraryTrackResponse : LastfmLibraryResponse {
    public List<LastfmLibraryTrack> Tracks = new List<LastfmLibraryTrack>();
  }

  public class LastfmLibraryTrack {
    public string Name;
    public string Artist;
    public string Album;
    public int PlayCount;
  }
}
