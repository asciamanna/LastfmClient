using LastfmClient;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmResponseParserTest {
    [Test]
    public void Parse() {
      var xelement = XElement.Load(@"lastfmResponse.xml");
      var result = new LastfmResponseParser().Parse(xelement);
      Assert.AreEqual("ok", result.Status);
      Assert.AreEqual(1, result.Page);
      Assert.AreEqual(50, result.PerPage);
      Assert.AreEqual(120, result.TotalPages);
      Assert.AreEqual(5980, result.TotalRecords);
      Assert.AreEqual(31, result.Tracks.Count());
    }

    [Test]
    public void Parse_Tracks() {
      var xelement = XElement.Load(@"lastfmResponse.xml");
      var result = new LastfmResponseParser().Parse(xelement);
      var firstTrack = result.Tracks.First();
      Assert.AreEqual("Terminal", firstTrack.Name);
      Assert.AreEqual("OSI", firstTrack.Artist);
      Assert.AreEqual("Blood", firstTrack.Album);
      Assert.AreEqual(90, firstTrack.PlayCount);
    }
  }
}
