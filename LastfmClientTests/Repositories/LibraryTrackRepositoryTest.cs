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
  public class LibraryTrackRepositoryTest {

    [Test]
    public void GetItems_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      
      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key=key&user=me&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key=key&user=me&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var lastfmResponse1 = CreateResponse("Track1");
      var lastfmResponse2 = CreateResponse("Track2");

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);
      parser.Expect(p => p.ParseTracks(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseTracks(response2)).Return(lastfmResponse2);

      var tracks = new LibraryTrackRepository("key", restClient, parser).GetItems("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.That(tracks.Count(), Is.EqualTo(2));
      Assert.That(tracks[0].Name, Is.EqualTo("Track1"));
      Assert.That(tracks[1].Name, Is.EqualTo("Track2"));
    }

    private static LastfmResponse<LastfmLibraryItem> CreateResponse(string name) {
      var lastfmResponse1 = new LastfmResponse<LastfmLibraryItem> { TotalPages = 2, Items = new List<LastfmLibraryItem> { new LastfmLibraryTrack { Name = name } } };
      return lastfmResponse1;
    }
  }
}
