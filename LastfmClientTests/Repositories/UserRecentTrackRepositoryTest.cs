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
  public class UserRecentTrackRepositoryTest {
    [Test]
    public void GetItems_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<IUserResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var pageCalculator = MockRepository.GenerateMock<IPageCalculator>();

      const string firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
      const string secondUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      const string firstTrack = "My Favorite Things";
      const string secondTrack = "But Not For Me";
      var lastfmResponse1 = TestHelper.CreateLastfmUserItemResponse<LastfmUserRecentTrack>(firstTrack);
      var lastfmResponse2 = TestHelper.CreateLastfmUserItemResponse<LastfmUserRecentTrack>(secondTrack);
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse1, 2)).Return(2);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);

      parser.Expect(p => p.Parse(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.Parse(response2)).Return(lastfmResponse2);

      var recentTracks = new UserRecentTrackRepository("key", restClient, parser, pageCalculator).GetItems("me", 2);

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();

      Assert.That(recentTracks.Count(), Is.EqualTo(2));
      Assert.That(recentTracks[0].Name, Is.EqualTo(firstTrack));
      Assert.That(recentTracks[1].Name, Is.EqualTo(secondTrack));
    }

    [Test]
    public void GetItems_Returns_Exactly_The_Number_Of_Tracks_Requested_Even_If_Retrieves_More_From_Lastfm() {
      var parser = MockRepository.GenerateStub<IUserResponseParser>();
      var restClient = MockRepository.GenerateStub<IRestClient>();
      var pageCalculator = MockRepository.GenerateStub<IPageCalculator>();

      const string firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
      var response = new XElement("Response1");
      const string firstTrack = "My Favorite Things";
      const string secondTrack = "But Not For Me";
      var lastfmResponse = TestHelper.CreateLastfmUserItemResponse<LastfmUserRecentTrack>(firstTrack, secondTrack);
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse, 1)).Return(1);
      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response);
      parser.Expect(p => p.Parse(response)).Return(lastfmResponse);

      var recentTracks = new UserRecentTrackRepository("key", restClient, parser, pageCalculator).GetItems("me", 1);

      Assert.That(recentTracks.Count(), Is.EqualTo(1));
      Assert.That(recentTracks.First().Name, Is.EqualTo(firstTrack));
    }
  }
}
