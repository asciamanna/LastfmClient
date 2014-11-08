using System;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Repositories;
using LastfmClient.Responses;
using NUnit.Framework;
using Rhino.Mocks;

namespace LastfmClientTests.Repositories {
  [TestFixture]
  public class ArtistRepositoryTest {
    [Test]
    public void GetInfo_Encodes_Artist() {
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var parser = MockRepository.GenerateMock<IArtistResponseParser>();
      var apiKey = "key";
      var artist = "Bobby Hutcherson";
      var response = new XElement("Response");
      var expectedUri = string.Format(@"http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&api_key={0}&artist={1}",
        apiKey, Uri.EscapeDataString(artist));
      var artistInfo = new LastfmArtistInfo();
      
      restClient.Stub(rc => rc.DownloadData(expectedUri)).Return(response);
      parser.Stub(p => p.Parse(response)).Return(artistInfo);
      var repository = new ArtistRepository(apiKey, restClient, parser);

      Assert.That(repository.GetInfo(artist), Is.SameAs(artistInfo));
    }
  }
}
