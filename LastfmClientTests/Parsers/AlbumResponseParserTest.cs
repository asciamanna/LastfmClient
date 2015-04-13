using System;
using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class AlbumResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void ParseAlbumInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumInfoResponse.xml");

      var result = new AlbumResponseParser().Parse(xelement);

      Assert.That(result.Name, Is.EqualTo("San Francisco"));
      Assert.That(result.Artist, Is.EqualTo("Bobby Hutcherson"));
      Assert.That(result.Mbid, Is.Empty);
      Assert.That(result.ReleaseDate.Value.Date, Is.EqualTo(new DateTime(1994, 2, 28).Date));
      Assert.That(result.WikiSummary, Is.StringStarting("Bobby Hutcherson - Vibraphone, Marimba, Percussion"));
    }

    [Test]
    public void ParseAlbumInfo_When_No_Release_Date() {
      var xelement = XElement.Load(testFilePath + "lastfmAlbumInfoResponseNoReleaseDate.xml");

      var result = new AlbumResponseParser().Parse(xelement);

      Assert.That(result.ReleaseDate, Is.Null);
    }

    [Test]
    public void ParseAlbumInfo_When_Lastfm_Error_Throw_Exception() {
      var xelement = XElement.Load(testFilePath + "lastfmInvalidApiKey.xml");

      var exception = Assert.Throws<LastfmException>(() => new AlbumResponseParser().Parse(xelement));

      Assert.That(exception.ErrorCode, Is.EqualTo(10));
      Assert.That(exception.Message, Is.EqualTo("Invalid API key - You must be granted a valid key by last.fm"));
    }
  }
}
