using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class UserTopArtistResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;

    [Test]
    public void Parse_Removes_Whitespace_from_Artwork_Location_URLs() {
      var xelement = XElement.Load(testFilePath + "lastfmUserTopArtistsResponse.xml");
      var result = new UserTopArtistResponseParser().Parse(xelement);
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
    public void Parse() {
      var xelement = XElement.Load(testFilePath + "lastfmUserTopArtistsResponse.xml");
      var result = new UserTopArtistResponseParser().Parse(xelement);
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
    public void Parse_When_Lastfm_Error_Throw_Exception() {
      var xelement = XElement.Load(testFilePath + "lastfmInvalidApiKey.xml");

      var exception = Assert.Throws<LastfmException>(() => new UserTopArtistResponseParser().Parse(xelement));
      Assert.That(exception.ErrorCode, Is.EqualTo(10));
      Assert.That(exception.Message, Is.EqualTo("Invalid API key - You must be granted a valid key by last.fm"));
    }
  }
}
