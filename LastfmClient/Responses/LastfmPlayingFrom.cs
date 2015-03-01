
namespace LastfmClient.Responses {
  public class LastfmPlayingFrom {
    public LastfmPlayingFrom() {
      MusicServiceName = string.Empty;
      MusicServiceUrl = string.Empty;
    }
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }
  }
}
