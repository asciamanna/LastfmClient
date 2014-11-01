using System;
using LastfmClient;
using LastfmClient.Responses;
using NUnit.Framework;
using Rhino.Mocks;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmServiceTest {
    [Test]
    public void Object_Constructed_Without_An_API_Key_Throws_Exception() {
      var exception = Assert.Throws<ArgumentException>(() => new LastfmService(string.Empty, null, null));
      Assert.That(exception.Message, Is.EqualTo("An API key is required"));
    }

    [TestCase(-5)]
    [TestCase(0)]
    public void FindRecentTracks_Only_Takes_Positive_Values_For_Number_Of_Recent_Tracks(int numberOfTracks) {
      var exception = Assert.Throws<ArgumentException>(() => new LastfmService("key", null, null).FindRecentTracks("me", numberOfTracks));
      Assert.That(exception.Message, Is.EqualTo("numberOfTracks must be a positive integer."));
    }

    [TestCase(-11)]
    [TestCase(0)]
    public void FindTopArtists_Only_Takes_Positive_Values_For_Number_Of_Top_Artists(int numberOfArtists) {
      var exception = Assert.Throws<ArgumentException>(() => new LastfmService("key", null, null).FindTopArtists("me", numberOfArtists));
      Assert.That(exception.Message, Is.EqualTo("numberOfArtists must be a positive integer."));
    }

    [Test]
    public void FindCurrentlyPlayingFrom_Delegates_To_PageScraper() {
      var pageScraper = MockRepository.GenerateStub<ILastfmPageScraper>();
      var service = new LastfmService("key", pageScraper, null);
      var user = "testingUser";
      var scraperResult = new LastfmPlayingFrom { MusicServiceName = "Spotify", MusicServiceUrl = @"http://www.spotify.com" };

      pageScraper.Stub(ps => ps.GetLastfmPlayingFromInfo("http://www.last.fm/user/" + user)).Return(scraperResult);

      var response = service.FindCurrentlyPlayingFrom(user);
      Assert.That(response.MusicServiceName, Is.EqualTo(scraperResult.MusicServiceName));
      Assert.That(response.MusicServiceUrl, Is.EqualTo(scraperResult.MusicServiceUrl));
    }
  }
}
