using LastfmClient;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmResponseParserTest {
    [Test]
    public void ParseTracks_Counts() {
      var xelement = XElement.Load(@"lastfmTrackResponse.xml");
      var result = new LastfmResponseParser().ParseTracks(xelement);
      Assert.AreEqual("ok", result.Status);
      Assert.AreEqual(1, result.Page);
      Assert.AreEqual(50, result.PerPage);
      Assert.AreEqual(120, result.TotalPages);
      Assert.AreEqual(5980, result.TotalRecords);
      Assert.AreEqual(31, result.Tracks.Count());
    }

    [Test]
    public void ParseTracks_TrackInfo() {
      var xelement = XElement.Load(@"lastfmTrackResponse.xml");
      var result = new LastfmResponseParser().ParseTracks(xelement);
      var firstTrack = result.Tracks.First();
      Assert.AreEqual("Terminal", firstTrack.Name);
      Assert.AreEqual("OSI", firstTrack.Artist);
      Assert.AreEqual("Blood", firstTrack.Album);
      Assert.AreEqual(90, firstTrack.PlayCount);
    }

    [Test]
    public void ParseAlbums_Counts() {
      var xelement = XElement.Load(@"lastfmAlbumResponse.xml");
      var result = new LastfmResponseParser().ParseAlbums(xelement);
      Assert.AreEqual("ok", result.Status);
      Assert.AreEqual(1, result.Page);
      Assert.AreEqual(50, result.PerPage);
      Assert.AreEqual(19, result.TotalPages);
      Assert.AreEqual(929, result.TotalRecords);
    }

    [Test]
    public void ParseAlbums_AlbumInfo() {
      var xelement = XElement.Load(@"lastfmAlbumResponse.xml");
      var result = new LastfmResponseParser().ParseAlbums(xelement);
      var album = result.Albums.First();
      Assert.AreEqual("OSI", album.Artist);
      Assert.AreEqual("Blood", album.Name);
      Assert.AreEqual(520, album.PlayCount);
    }
  }
}
