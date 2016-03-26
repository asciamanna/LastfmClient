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
  public class AlbumRepositoryTest {
    private const string ApiKey = "API Key";
    private IRestClient restClient;
    private IAlbumResponseParser albumResponseParser;
    private AlbumRepository subject;

    [SetUp]
    public void SetUp() {
      restClient = MockRepository.GenerateStub<IRestClient>();
      albumResponseParser = MockRepository.GenerateStub<IAlbumResponseParser>();
      subject = new AlbumRepository(ApiKey, restClient, albumResponseParser);
    }

    [Test]
    public void GetInfo_Encodes_Album_And_Artist() {
      const string artist = "Bobby Hutcherson";
      const string album = "San Francisco";
      var response = new XElement("XMLResponse");
      var expectedUri = CreateExpectedUri(ApiKey, Uri.EscapeDataString(artist), Uri.EscapeDataString(album));
      var albumInfo = new LastfmAlbumInfo();
      restClient.Stub(rc => rc.DownloadData(expectedUri)).Return(response);
      albumResponseParser.Stub(p => p.Parse(response)).Return(albumInfo);

      var result = subject.GetInfo(artist, album);

      Assert.That(result, Is.SameAs(albumInfo));
    }

    private static string CreateExpectedUri(string apiKey, string artist, string album) {
      return string.Format(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key={0}&artist={1}&album={2}",
         apiKey, artist, album);
    }
  }
}
