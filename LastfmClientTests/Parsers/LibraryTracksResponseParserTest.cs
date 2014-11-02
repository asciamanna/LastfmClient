using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Responses;
using NUnit.Framework;

namespace LastfmClientTests.Parsers {
  [TestFixture]
  public class LibraryTracksResponseParserTest {
    private string testFilePath = TestHelper.TestFilePath;
    
    [Test]
    public void ParseTracks_Counts() {
      var xelement = XElement.Load(testFilePath + "lastfmTrackResponse.xml");
      var result = new LibraryTracksResponseParser().Parse(xelement);
      Assert.That(result.Status, Is.EqualTo("ok"));
      Assert.That(result.Page, Is.EqualTo(1));
      Assert.That(result.PerPage, Is.EqualTo(50));
      Assert.That(result.TotalPages, Is.EqualTo(120));
      Assert.That(result.TotalRecords, Is.EqualTo(5980));
      Assert.That(result.Items.Count(), Is.EqualTo(31));
      Assert.That(result.Items.First(), Is.InstanceOf<LastfmLibraryTrack>());
    }

    [Test]
    public void ParseTracks_TrackInfo() {
      var xelement = XElement.Load(testFilePath + "lastfmTrackResponse.xml");
      var result = new LibraryTracksResponseParser().Parse(xelement);
      var firstTrack = result.Items.First() as LastfmLibraryTrack;
      Assert.That(firstTrack.Name, Is.EqualTo("Terminal"));
      Assert.That(firstTrack.Artist, Is.EqualTo("OSI"));
      Assert.That(firstTrack.Album, Is.EqualTo("Blood"));
      Assert.That(firstTrack.PlayCount, Is.EqualTo(90));
    }

    [Test]
    public void Parse_When_Lastfm_Error_Throw_Exception() {
      var xelement = XElement.Load(testFilePath + "lastfmInvalidApiKey.xml");

      var exception = Assert.Throws<LastfmException>(() => new LibraryTracksResponseParser().Parse(xelement));
      Assert.That(exception.ErrorCode, Is.EqualTo(10));
      Assert.That(exception.Message, Is.EqualTo("Invalid API key - You must be granted a valid key by last.fm"));
    }
  }
}
