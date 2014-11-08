using System;
using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class ArtistResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void ParseArtistInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmArtistInfoResponseWithFormationData.xml");

      var result = new ArtistResponseParser().Parse(xelement);

      Assert.That(result.Name, Is.EqualTo("Ramones"));
      Assert.That(result.Mbid, Is.StringStarting("d6ed7887"));
      Assert.That(result.BioSummary, Is.StringStarting(@"The Ramones were a "));
      Assert.That(result.YearFormed, Is.EqualTo(1974));
      Assert.That(result.PlaceFormed, Is.EqualTo("Queens, New York, United States"));
    }

    [Test]
    public void ParseArtistInfo_When_No_Formation_Data() {
      var xelement = XElement.Load(testFilePath + "lastfmArtistInfoResponseNoFormationData.xml");

      var result = new ArtistResponseParser().Parse(xelement);
      Assert.That(result.YearFormed, Is.Null);
      Assert.That(result.PlaceFormed, Is.Empty);
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
