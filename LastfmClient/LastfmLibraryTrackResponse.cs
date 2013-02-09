using System.Collections.Generic;

namespace LastfmClient {
  public class LastfmLibraryTrackResponse {
    public string Status;
    public int Page = 0;
    public int PerPage = 0;
    public int TotalPages = 0;
    public int TotalRecords = 0;
    public List<LastfmLibraryTrack> Tracks = new List<LastfmLibraryTrack>();
  }

  public class LastfmLibraryTrack {
    public string Name;
    public string Artist;
    public string Album;
    public int PlayCount;
  }
}
