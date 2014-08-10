using LastfmClient;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;
using System;
using LastfmClient.Responses;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmResponseParserTest {
    [Test]
    public void ParseTracks_Counts() {
      var xelement = XElement.Load(@"lastfmTrackResponse.xml");
      var result = new LastfmResponseParser().ParseTracks(xelement);
      Assert.That(result.Status, Is.EqualTo("ok"));
      Assert.That(result.Page, Is.EqualTo(1));
      Assert.That(result.PerPage, Is.EqualTo(50));
      Assert.That(result.TotalPages, Is.EqualTo(120));
      Assert.That(result.TotalRecords, Is.EqualTo(5980));
      Assert.That(result.Items.Count(), Is.EqualTo(31));
    }

    [Test]
    public void ParseTracks_TrackInfo() {
      var xelement = XElement.Load(@"lastfmTrackResponse.xml");
      var result = new LastfmResponseParser().ParseTracks(xelement);
      var firstTrack = result.Items.First();
      Assert.That(firstTrack.Name, Is.EqualTo("Terminal"));
      Assert.That(firstTrack.Artist, Is.EqualTo("OSI"));
      Assert.That(firstTrack.Album, Is.EqualTo("Blood"));
      Assert.That(firstTrack.PlayCount, Is.EqualTo(90));
    }

    [Test]
    public void ParseAlbums_Counts() {
      var xelement = XElement.Load(@"lastfmAlbumResponse.xml");
      var result = new LastfmResponseParser().ParseAlbums(xelement);
      Assert.That(result.Status, Is.EqualTo("ok"));
      Assert.That(result.Page, Is.EqualTo(1));
      Assert.That(result.PerPage, Is.EqualTo(50));
      Assert.That(result.TotalPages, Is.EqualTo(19));
      Assert.That(result.TotalRecords, Is.EqualTo(929));
    }

    [Test]
    public void ParseAlbums_AlbumInfo() {
      var xelement = XElement.Load(@"lastfmAlbumResponse.xml");
      var result = new LastfmResponseParser().ParseAlbums(xelement);
      var album = result.Items.First();
      Assert.That(album.Artist, Is.EqualTo("OSI"));
      Assert.That(album.Name, Is.EqualTo("Blood"));
      Assert.That(album.ArtworkLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/82850719.jpg"));
      Assert.That(album.PlayCount, Is.EqualTo(520));
    }

    [Test]
    public void ParseRecentTracks_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(@"lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Items.First();
      Assert.That(recentTrack.ExtraLargeAlbumArtLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.ExtraLargeAlbumArtLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(recentTrack.LargeAlbumArtLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.LargeAlbumArtLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(recentTrack.MediumAlbumArtLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.MediumAlbumArtLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(recentTrack.SmallAlbumArtLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.SmallAlbumArtLocation, Is.Not.StringMatching("\\s+$"));
    }

    [Test]
    public void ParseRecentTracks_TrackInfo() {
      var xelement = XElement.Load(@"lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Items.First();
      Assert.That(recentTrack.IsNowPlaying, Is.False);
      Assert.That(recentTrack.Name, Is.EqualTo("Sophisticated Lady"));
      Assert.That(recentTrack.Album, Is.EqualTo("Thelonious Monk Plays Duke Ellington"));
      Assert.That(recentTrack.ExtraLargeAlbumArtLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/94649493.png"));
      Assert.That(recentTrack.LargeAlbumArtLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/126/94649493.png"));
      Assert.That(recentTrack.MediumAlbumArtLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/64s/94649493.png"));
      Assert.That(recentTrack.SmallAlbumArtLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/34s/94649493.png"));
      Assert.That(recentTrack.LastPlayed, Is.EqualTo(new DateTime(2014, 4, 12, 2, 36, 0)));
    }

    [Test]
    public void ParseRecentTracks_Parses_NowPlaying() {
      var xelement = XElement.Load(@"lastfmRecentTracksResponseNowPlaying.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Items.First();
      Assert.That(recentTrack.IsNowPlaying, Is.True);
    }

    [Test]
    public void ParseTopArtists_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(@"lastfmUserTopArtistsResponse.xml");
      var result = new LastfmResponseParser().ParseTopArtists(xelement);
      var topArtist = result.Items.First();
      Assert.That(topArtist.MegaArtistImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.MegaArtistImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.ExtraLargeArtistImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.ExtraLargeArtistImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.LargeArtistImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.LargeArtistImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.MediumArtistImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.MediumArtistImageLocation, Is.Not.StringMatching("\\s+$"));

      Assert.That(topArtist.SmallArtistImageLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(topArtist.SmallArtistImageLocation, Is.Not.StringMatching("\\s+$"));
    }

    [Test]
    public void ParseTopArtists() {
      var xelement = XElement.Load(@"lastfmUserTopArtistsResponse.xml");
      var result = new LastfmResponseParser().ParseTopArtists(xelement);
      var topArtist = result.Items.First();

      Assert.That(topArtist.Name, Is.EqualTo("Miles Davis"));
      Assert.That(topArtist.PlayCount, Is.EqualTo(3247));
      Assert.That(topArtist.Rank, Is.EqualTo(1));
      Assert.That(topArtist.MegaArtistImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/500/11251985/Miles+Davis+Miles.jpg"));
      Assert.That(topArtist.ExtraLargeArtistImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/252/11251985.jpg"));
      Assert.That(topArtist.LargeArtistImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/126/11251985.jpg"));
      Assert.That(topArtist.MediumArtistImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/64/11251985.jpg"));
      Assert.That(topArtist.SmallArtistImageLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/34/11251985.jpg"));
    }
  }
}
