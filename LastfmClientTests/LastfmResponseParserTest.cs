using LastfmClient;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;
using System;

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
      Assert.That(result.Tracks.Count(), Is.EqualTo(31));
    }

    [Test]
    public void ParseTracks_TrackInfo() {
      var xelement = XElement.Load(@"lastfmTrackResponse.xml");
      var result = new LastfmResponseParser().ParseTracks(xelement);
      var firstTrack = result.Tracks.First();
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
      var album = result.Albums.First();
      Assert.That(album.Artist, Is.EqualTo("OSI"));
      Assert.That(album.Name, Is.EqualTo("Blood"));
      Assert.That(album.ArtworkLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/82850719.jpg"));
      Assert.That(album.PlayCount, Is.EqualTo(520));
    }

    [Test]
    public void ParseRecentTracks_Removes_Whitespace_from_Artwork_Location_URL() {
      var xelement = XElement.Load(@"lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Tracks.First();
      Assert.That(recentTrack.AlbumArtLocation, Is.Not.StringMatching("^\\s+"));
      Assert.That(recentTrack.AlbumArtLocation, Is.Not.StringMatching("\\s+$"));
    }

    [Test]
    public void ParseRecentTracks_TrackInfo() {
      var xelement = XElement.Load(@"lastfmUserRecenttracksResponse.xml");
      var result = new LastfmResponseParser().ParseRecentTracks(xelement);
      var recentTrack = result.Tracks.First();
      Assert.That(recentTrack.Name, Is.EqualTo("Sophisticated Lady"));
      Assert.That(recentTrack.Album, Is.EqualTo("Thelonious Monk Plays Duke Ellington"));
      Assert.That(recentTrack.AlbumArtLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/94649493.png"));
      Assert.That(recentTrack.LastPlayed, Is.EqualTo(new DateTime(2014, 4, 12, 2, 36, 0)));
    }
  }
}
