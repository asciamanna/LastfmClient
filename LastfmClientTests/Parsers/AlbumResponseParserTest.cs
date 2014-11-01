using System;
using System.Linq;
using System.Xml.Linq;
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
  }
}
