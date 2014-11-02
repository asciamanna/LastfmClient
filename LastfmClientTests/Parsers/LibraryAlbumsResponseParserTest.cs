using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class LibraryAlbumsResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void ParseAlbums_Counts() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumResponse.xml");
      var result = new LibraryAlbumsResponseParser().Parse(xelement);
      Assert.That(result.Status, Is.EqualTo("ok"));
      Assert.That(result.Page, Is.EqualTo(1));
      Assert.That(result.PerPage, Is.EqualTo(50));
      Assert.That(result.TotalPages, Is.EqualTo(19));
      Assert.That(result.TotalRecords, Is.EqualTo(929));
    }

    [Test]
    public void ParseAlbums_AlbumInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumResponse.xml");
      var result = new LibraryAlbumsResponseParser().Parse(xelement);
      var album = result.Items.First() as LastfmLibraryAlbum;
      Assert.That(album.Artist, Is.EqualTo("OSI"));
      Assert.That(album.Name, Is.EqualTo("Blood"));
      Assert.That(album.ArtworkLocation, Is.EqualTo("http://userserve-ak.last.fm/serve/300x300/82850719.jpg"));
      Assert.That(album.PlayCount, Is.EqualTo(520));
    }

    [Test]
    public void Parse_When_Lastfm_Error_Throw_Exception() {
      var xelement = XElement.Load(testFilePath + "lastfmInvalidApiKey.xml");

      var exception = Assert.Throws<LastfmException>(() => new LibraryAlbumsResponseParser().Parse(xelement));
      Assert.That(exception.ErrorCode, Is.EqualTo(10));
      Assert.That(exception.Message, Is.EqualTo("Invalid API key - You must be granted a valid key by last.fm"));
    }
  }
}
