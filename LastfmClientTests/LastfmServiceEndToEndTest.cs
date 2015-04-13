using System.Configuration;
using System.Linq;
using LastfmClient;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests {
  [TestFixture, Explicit, Category("EndToEnd")]
  public class LastfmServiceEndToEndTest {
    private string apiKey;
    private LastfmService service;

    [SetUp]
    public void SetUp() {
      apiKey = ConfigurationManager.AppSettings["LastfmApiKey"];
      Assert.That(apiKey, Is.Not.Empty, "The End To End tests require a Last fm API key to be defined in the user.config");
      service = new LastfmService(apiKey);
    }

    [Test]
    public void FindRecentTracks_EndToEnd() {
      var results = service.FindRecentTracks("asciamanna", 2);
      
      Assert.That(results.Count, Is.EqualTo(2));
      Assert.That(results.First(), Is.TypeOf<LastfmUserRecentTrack>());
    }

    [Test]
    public void FindTopArtists_EndToEnd() {
      var results = service.FindTopArtists("asciamanna", 2);
      
      Assert.That(results.Count, Is.EqualTo(2));
      Assert.That(results.First(), Is.TypeOf<LastfmUserTopArtist>());
    }

    [Test]
    public void FindMusicSource_EndToEnd() {
      var result = service.FindMusicSource("asciamanna");
      
      Assert.That(result.MusicServiceName, Is.Not.Null);
      Assert.That(result.MusicServiceUrl, Is.Not.Null);
    }

    [Test]
    public void FindAlbumInfo_EndToEnd() {
      const string artist = "Bobby Hutcherson";
      const string album = "Happenings";
      
      var result = service.FindAlbumInfo(artist, album);
      
      Assert.That(result.Artist, Is.EqualTo(artist));
      Assert.That(result.Name, Is.EqualTo(album));
    }

    [Test]
    public void FindArtistInfo_EndToEnd() {
      const string artist = "John Coltrane";
      
      var result = service.FindArtistInfo(artist);
      
      Assert.That(result.Name, Is.EqualTo(artist));
      Assert.That(result.BioSummary, Is.Not.Empty);
    }
  }
}
