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
  public class UserTopArtistRepositoryTest {

    [Test]
    public void FindTopArtists_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<IUserResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var pageCalculator = MockRepository.GenerateMock<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstArtist = "Miles Davis";
      var secondArtist = "Devo";
      var lastfmResponse1 = CreateResponse(firstArtist);
      var lastfmResponse2 = CreateResponse(secondArtist);
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse1, 2)).Return(2);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);

      parser.Expect(p => p.Parse(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.Parse(response2)).Return(lastfmResponse2);

      var topArtists = new UserTopArtistRepository("key", restClient, parser, pageCalculator).GetItems("me", 2);

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();

      Assert.That(topArtists.Count(), Is.EqualTo(2));
      Assert.That(topArtists[0].Name, Is.EqualTo(firstArtist));
      Assert.That(topArtists[1].Name, Is.EqualTo(secondArtist));
    }

    [Test]
    public void GetItems_Returns_Exactly_The_Number_Of_Tracks_Requested_Even_If_Retrieves_More_From_Lastfm() {
      var parser = MockRepository.GenerateStub<IUserResponseParser>();
      var restClient = MockRepository.GenerateStub<IRestClient>();
      var pageCalculator = MockRepository.GenerateStub<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=1";
      var response = new XElement("Response1");
      var firstArtist = "Ramones";
      var secondArtist = "Misfits";
      var lastfmResponse = new LastfmResponse<LastfmUserItem> {
        Items = new List<LastfmUserItem> { 
          new LastfmUserTopArtist { Name = firstArtist }, 
          new LastfmUserTopArtist { Name = secondArtist } 
        }
      };
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse, 1)).Return(1);
      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response);
      parser.Expect(p => p.Parse(response)).Return(lastfmResponse);

      var topArtists = new UserTopArtistRepository("key", restClient, parser, pageCalculator).GetItems("me", 1);

      Assert.That(topArtists.Count(), Is.EqualTo(1));
      Assert.That(topArtists.First().Name, Is.EqualTo(firstArtist));
    }

    private static LastfmResponse<LastfmUserItem> CreateResponse(string name) {
      return new LastfmResponse<LastfmUserItem> { Items = new List<LastfmUserItem> { new LastfmUserTopArtist { Name = name } } };
    }
  }
}
