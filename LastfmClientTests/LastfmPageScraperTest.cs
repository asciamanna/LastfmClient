using System;
using LastfmClient;
using NUnit.Framework;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmPageScraperTest {
    private LastfmPageScraper scraper;
    private string testFilePath = TestHelper.TestFilePath;

    [SetUp]
    public void SetUp() {
      scraper = new LastfmPageScraper();
    }

    [Test]
    public void GetLastfmMusicSourceFromFile_While_User_Is_Currently_Playing() {
      var listeningFrom = scraper.GetLastfmMusicSourceFromFile(testFilePath + "lastfmUserPageCurrentlyPlaying.html");
      
      Assert.That(listeningFrom.MusicServiceName, Is.EqualTo("Spotify"));
      Assert.That(listeningFrom.MusicServiceUrl, Is.EqualTo(@"http://www.spotify.com/"));
    }

    [Test]
    public void GetLastfmMusicSourceFromFile_While_User_Is_Not_Playing_Returns_Empty_Result() {
      var listeningFrom = scraper.GetLastfmMusicSourceFromFile(testFilePath + "lastfmUserPageNotCurrentlyPlaying.html");
      
      Assert.That(listeningFrom.MusicServiceName, Is.Null.Or.Empty);
      Assert.That(listeningFrom.MusicServiceUrl, Is.Null.Or.Empty);
    }

    [Test]
    public void GetLastfmMusicSourceFromFile_Translates_Lastfm_Relative_URLs() {
      var listeningFrom = scraper.GetLastfmMusicSourceFromFile(testFilePath + "lastfmUserPageWithRelativeUrl.html");
      
      Assert.That(listeningFrom.MusicServiceUrl, Is.EqualTo(@"http://www.last.fm/download"));
    }

    [Test, Category("Integration")]
    public void GetLastfmMusicSource_User_Page_Exists() {
      var pageUrl = @"http://www.last.fm/user/asciamanna";
      
      Assert.DoesNotThrow(() => scraper.GetLastfmMusicSource(pageUrl));
    }

    [Test, Category("Integration")]
    public void GetLastfmMusicSource_User_Page_Does_Not_Exist_Returns_Empty_Result() {
      var badPageUrl = @"http://www.last.fm/user/MadeUpNameDoesNotExistArgh";
      var result = scraper.GetLastfmMusicSource(badPageUrl);
      
      Assert.That(result.MusicServiceName, Is.Null.Or.Empty);
      Assert.That(result.MusicServiceUrl, Is.Null.Or.Empty);
    }
  }
}
