using System;
using LastfmClient;
using NUnit.Framework;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmPageScraperTests {
    private LastfmPageScraper scraper;
    private string testFilePath = TestHelper.TestFilePath;

    [SetUp]
    public void SetUp() {
      scraper = new LastfmPageScraper();
    }

    [Test]
    public void GetLastfmPlayingFromInfoFromFile_While_User_Is_Currently_Playing() {
      var listeningFrom = scraper.GetLastfmPlayingFromInfoFromFile(testFilePath + "lastfmUserPageCurrentlyPlaying.html");
      Assert.That(listeningFrom.MusicServiceName, Is.EqualTo("Spotify"));
      Assert.That(listeningFrom.MusicServiceUrl, Is.EqualTo(@"http://www.spotify.com/"));
    }

    [Test]
    public void GetLastfmPlayingFromInfoFromFile_While_User_Is_Not_Playing_Returns_Empty_Result() {
      var listeningFrom = scraper.GetLastfmPlayingFromInfoFromFile(testFilePath + "lastfmUserPageNotCurrentlyPlaying.html");
      Assert.That(String.IsNullOrWhiteSpace(listeningFrom.MusicServiceName), Is.True);
      Assert.That(String.IsNullOrWhiteSpace(listeningFrom.MusicServiceUrl), Is.True);
    }

    [Test]
    public void GetLastfmPlayingFromInfoFromFile_Translates_Lastfm_Relative_URLs() {
      var listeningFrom = scraper.GetLastfmPlayingFromInfoFromFile(testFilePath + "lastfmUserPageWithRelativeUrl.html");
      Assert.That(listeningFrom.MusicServiceUrl, Is.EqualTo(@"http://www.last.fm/download"));
    }

    [Test, Category("Integration")]
    public void GetLastfmPlayingFromInfo_User_Page_Exists() {
      var pageUrl = @"http://www.last.fm/user/asciamanna";
      Assert.DoesNotThrow(() => scraper.GetLastfmPlayingFromInfo(pageUrl));
    }

    [Test, Category("Integration")]
    public void GetLastfmPlayingFromInfo_User_Page_Does_Not_Exist_Returns_Empty_Result() {
      var badPageUrl = @"http://www.last.fm/user/MadeUpNameDoesNotExistArgh";
      var result = scraper.GetLastfmPlayingFromInfo(badPageUrl);
      Assert.That(String.IsNullOrWhiteSpace(result.MusicServiceName), Is.True);
      Assert.That(String.IsNullOrWhiteSpace(result.MusicServiceUrl), Is.True);
    }
  }
}
