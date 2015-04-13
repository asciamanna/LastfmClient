using System;
using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class UserRecentTracksResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void Parse_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(testFilePath + "lastfmUserRecenttracksResponse.xml");

      var result = new UserRecentTracksResponseParser().Parse(xelement);
      var recentTrack = result.Items.First();
      
      Assert.That(recentTrack.ExtraLargeImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.ExtraLargeImageLocation, Is.Not.StringMatching("\\s+$"));
      Assert.That(recentTrack.LargeImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.LargeImageLocation, Is.Not.StringMatching("\\s+$"));
      Assert.That(recentTrack.MediumImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.MediumImageLocation, Is.Not.StringMatching("\\s+$"));
      Assert.That(recentTrack.SmallImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.SmallImageLocation, Is.Not.StringMatching("\\s+$"));
    }

    [Test]
    public void Parse_TrackInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmUserRecenttracksResponse.xml");
      
      var result = new UserRecentTracksResponseParser().Parse(xelement);
      var recentTrack = result.Items.First() as LastfmUserRecentTrack;
      
      Assert.That(recentTrack.IsNowPlaying, Is.False);
      Assert.That(recentTrack.Name, Is.EqualTo("Sophisticated Lady"));
      Assert.That(recentTrack.Album, Is.EqualTo("Thelonious Monk Plays Duke Ellington"));
      Assert.That(recentTrack.ExtraLargeImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/94649493.png"));
      Assert.That(recentTrack.LargeImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/126/94649493.png"));
      Assert.That(recentTrack.MediumImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/64s/94649493.png"));
      Assert.That(recentTrack.SmallImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/34s/94649493.png"));
      Assert.That(recentTrack.LastPlayed, Is.EqualTo(new DateTime(2014, 4, 12, 2, 36, 0)));
    }

    [Test]
    public void ParseRecentTracks_Parses_NowPlaying() {
      var xelement = XElement.Load(testFilePath + "lastfmRecentTracksResponseNowPlaying.xml");
      
      var result = new UserRecentTracksResponseParser().Parse(xelement);
      var recentTrack = result.Items.ToList().First() as LastfmUserRecentTrack;
      
      Assert.That(recentTrack.IsNowPlaying, Is.True);
    }

    [Test]
    public void Parse_When_Lastfm_Error_Throw_Exception() {
      var xelement = XElement.Load(testFilePath + "lastfmInvalidApiKey.xml");

      var exception = Assert.Throws<LastfmException>(() => new UserRecentTracksResponseParser().Parse(xelement));
      
      Assert.That(exception.ErrorCode, Is.EqualTo(10));
      Assert.That(exception.Message, Is.EqualTo("Invalid API key - You must be granted a valid key by last.fm"));
    }
  }
}
