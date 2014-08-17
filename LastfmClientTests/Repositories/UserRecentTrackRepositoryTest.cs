using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LastfmClient;
using LastfmClient.Repositories;
using LastfmClient.Responses;
using NUnit.Framework;
using Rhino.Mocks;

namespace LastfmClientTests.Repositories {
  [TestFixture]
  public class UserRecentTrackRepositoryTest {
     [Test]
    public void GetItems_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var pageCalculator = MockRepository.GenerateMock<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstTrack = "My Favorite Things";
      var secondTrack = "But Not For Me";
      var lastfmResponse1 = BuildResponse(firstTrack);
      var lastfmResponse2 = BuildResponse(secondTrack);
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse1, 2)).Return(2);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);

      parser.Expect(p => p.ParseRecentTracks(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseRecentTracks(response2)).Return(lastfmResponse2);

      var recentTracks = new UserRecentTrackRepository("key", restClient, parser, pageCalculator).GetItems("me", 2);

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();

      Assert.That(recentTracks.Count(), Is.EqualTo(2));
      Assert.That(recentTracks[0].Name, Is.EqualTo(firstTrack));
      Assert.That(recentTracks[1].Name, Is.EqualTo(secondTrack));
    }

     [Test]
     public void GetItems_Returns_Exactly_The_Number_Of_Tracks_Requested_Even_If_Retrieves_More_From_Lastfm() {
       var parser = MockRepository.GenerateStub<ILastfmResponseParser>();
       var restClient = MockRepository.GenerateStub<IRestClient>();
       var pageCalculator = MockRepository.GenerateStub<IPageCalculator>();

       var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
       var response = new XElement("Response1");
       var firstTrack = "My Favorite Things";
       var secondTrack = "But Not For Me";
       var lastfmResponse = new LastfmResponse<LastfmUserItem> {
         Items = new List<LastfmUserItem> { 
           new LastfmUserRecentTrack { Name = firstTrack }, 
           new LastfmUserRecentTrack { Name = secondTrack } 
         }
       };
       pageCalculator.Stub(pc => pc.Calculate(lastfmResponse, 1)).Return(1);
       restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response);
       parser.Expect(p => p.ParseRecentTracks(response)).Return(lastfmResponse);

       var recentTracks = new UserRecentTrackRepository("key", restClient, parser, pageCalculator).GetItems("me", 1);

       Assert.That(recentTracks.Count(), Is.EqualTo(1));
       Assert.That(recentTracks.First().Name, Is.EqualTo(firstTrack));
     }

    private static LastfmResponse<LastfmUserItem> BuildResponse(string name) {
       return new LastfmResponse<LastfmUserItem> { Items = new List<LastfmUserItem> { new LastfmUserRecentTrack { Name = name } } };
    }
  }
}
