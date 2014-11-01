using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Parsers;
using LastfmClient.Repositories;
using LastfmClient.Responses;
using NUnit.Framework;
using Rhino.Mocks;

namespace LastfmClientTests.Repositories {
  [TestFixture]
  public class LibraryAlbumRepositoryTest {
    [Test]
    public void GetItems_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key=key&user=me&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key=key&user=me&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstAlbum = "Dexter Calling...";
      var secondAlbum = "Our Man In Paris";
      var lastfmResponse1 = CreateResponse(firstAlbum);
      var lastfmResponse2 = CreateResponse(secondAlbum);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);
      parser.Expect(p => p.ParseAlbums(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseAlbums(response2)).Return(lastfmResponse2);

      var albums = new LibraryAlbumRepository("key", restClient, parser).GetItems("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.That(albums.Count(), Is.EqualTo(2));
      Assert.That(albums[0].Name, Is.EqualTo(firstAlbum));
      Assert.That(albums[1].Name, Is.EqualTo(secondAlbum));
    }

    private static LastfmResponse<LastfmLibraryItem> CreateResponse(string name) {
      return new LastfmResponse<LastfmLibraryItem> { TotalPages = 2, Items = new List<LastfmLibraryItem> { new LastfmLibraryAlbum { Name = name } } };
    }
  }
}
