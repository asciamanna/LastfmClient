using System;

namespace LastfmClient.Responses {
  public class LastfmAlbumInfo {
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Mbid { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string WikiSummary { get; set; }
  }
}
