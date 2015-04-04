
namespace LastfmClient.Responses {
  public class LastfmMusicSource {
    public LastfmMusicSource() {
      MusicServiceName = string.Empty;
      MusicServiceUrl = string.Empty;
    }
    public string MusicServiceName { get; set; }
    public string MusicServiceUrl { get; set; }
  }
}
