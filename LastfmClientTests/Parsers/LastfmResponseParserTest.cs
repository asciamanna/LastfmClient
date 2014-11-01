using System;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class LastfmResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void ParseRecentTracks_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(testFilePath + "lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
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
    public void ParseRecentTracks_TrackInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
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
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Items.First() as LastfmUserRecentTrack;
      Assert.That(recentTrack.IsNowPlaying, Is.True);
    }

    [Test]
    public void ParseTopArtists_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(testFilePath + "lastfmUserTopArtistsResponse.xml");
      var result = new LastfmResponseParser().ParseTopArtists(xelement);
      var topArtist = result.Items.First() as LastfmUserTopArtist;
      Assert.That(topArtist.MegaImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.MegaImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.ExtraLargeImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.ExtraLargeImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.LargeImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.LargeImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.MediumImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.MediumImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.SmallImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.SmallImageLocation, Is.Not.StringMatching("\\s+$"));
    }

    [Test]
    public void ParseTopArtists() {
      var xelement = XElement.Load(testFilePath + "lastfmUserTopArtistsResponse.xml");
      var result = new LastfmResponseParser().ParseTopArtists(xelement);
      var topArtist = result.Items.First() as LastfmUserTopArtist;

      Assert.That(topArtist.Name, Is.EqualTo("Miles Davis"));
      Assert.That(topArtist.PlayCount, Is.EqualTo(3247));
      Assert.That(topArtist.Rank, Is.EqualTo(1));
      Assert.That(topArtist.MegaImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/500/11251985/Miles+Davis+Miles.jpg"));
      Assert.That(topArtist.ExtraLargeImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/252/11251985.jpg"));
      Assert.That(topArtist.LargeImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/126/11251985.jpg"));
      Assert.That(topArtist.MediumImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/64/11251985.jpg"));
      Assert.That(topArtist.SmallImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/34/11251985.jpg"));
    }

    [Test]
    public void ParseAlbumInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumInfoResponse.xml");

      var result = new LastfmResponseParser().ParseAlbumInfo(xelement);

      Assert.That(result.Name, Is.EqualTo("San Francisco"));
      Assert.That(result.Artist, Is.EqualTo("Bobby Hutcherson"));
      Assert.That(result.Mbid, Is.Empty);
      Assert.That(result.ReleaseDate.Value.Date, Is.EqualTo(new DateTime(1994, 2, 28).Date));
      Assert.That(result.WikiSummary, Is.StringStarting("Bobby Hutcherson - Vibraphone, Marimba, Percussion"));
    }

    [Test]
    public void ParseAlbumInfo_When_No_Release_Date() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumInfoResponseNoReleaseDate.xml");

      var result = new LastfmResponseParser().ParseAlbumInfo(xelement);
      Assert.That(result.ReleaseDate, Is.Null);
    }
  }
}
