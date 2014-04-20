﻿using LastfmClient;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastfmClient.Responses;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmServiceTest {
    [Test]
    public void Object_Constructed_Without_An_API_Key_Throws_Exception() {
      Assert.Throws<ArgumentException>(() => new LastfmService(string.Empty, null, null, null), "An API key is required");
    }

    [Test]
    public void FindAllTracks_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key=key&user=me&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key=key&user=me&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var lastfmResponse1 = new LastfmLibraryTrackResponse { TotalPages = 2, Tracks = new List<LastfmLibraryTrack> { new LastfmLibraryTrack { Name = "Tracky1" } } };
      var lastfmResponse2 = new LastfmLibraryTrackResponse { TotalPages = 2, Tracks = new List<LastfmLibraryTrack> { new LastfmLibraryTrack { Name = "Tracky2" } } };

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);
      parser.Expect(p => p.ParseTracks(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseTracks(response2)).Return(lastfmResponse2);

      var tracks = new LastfmService("key", restClient, parser, null).FindAllTracks("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.That(tracks.Count(), Is.EqualTo(2));
      Assert.That(tracks[0].Name, Is.EqualTo("Tracky1"));
      Assert.That(tracks[1].Name, Is.EqualTo("Tracky2"));
    }

    [Test]
    public void FindAllAlbums_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key=key&user=me&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key=key&user=me&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstAlbum = "Dexter Calling...";
      var secondAlbum = "Our Man In Paris";
      var lastfmResponse1 = new LastfmLibraryAlbumResponse { TotalPages = 2, Albums = new List<LastfmLibraryAlbum> { new LastfmLibraryAlbum { Name = firstAlbum } } };
      var lastfmResponse2 = new LastfmLibraryAlbumResponse { TotalPages = 2, Albums = new List<LastfmLibraryAlbum> { new LastfmLibraryAlbum { Name = secondAlbum } } };

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);
      parser.Expect(p => p.ParseAlbums(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseAlbums(response2)).Return(lastfmResponse2);

      var albums = new LastfmService("key", restClient, parser, null).FindAllAlbums("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.That(albums.Count(), Is.EqualTo(2));
      Assert.That(albums[0].Name, Is.EqualTo(firstAlbum));
      Assert.That(albums[1].Name, Is.EqualTo(secondAlbum));
    }

    [Test]
    public void FindRecentTracks_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var pageCalculator = MockRepository.GenerateMock<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstTrack = "My Favorite Things";
      var secondTrack = "But Not For Me";
      var lastfmResponse1 = new LastfmUserRecentTracksResponse { Tracks = new List<LastfmUserRecentTrack> { new LastfmUserRecentTrack { Name = firstTrack } } };
      var lastfmResponse2 = new LastfmUserRecentTracksResponse { Tracks = new List<LastfmUserRecentTrack> { new LastfmUserRecentTrack { Name = secondTrack } } };
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse1, 2)).Return(2);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);

      parser.Expect(p => p.ParseRecentTracks(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseRecentTracks(response2)).Return(lastfmResponse2);

      var recentTracks = new LastfmService("key", restClient, parser, pageCalculator).FindRecentTracks("me", 2);

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();

      Assert.That(recentTracks.Count(), Is.EqualTo(2));
      Assert.That(recentTracks[0].Name, Is.EqualTo(firstTrack));
      Assert.That(recentTracks[1].Name, Is.EqualTo(secondTrack));
    }

    [TestCase(-5)]
    [TestCase(0)]
    public void FindRecentTracks_Only_Takes_Positive_Values_For_Number_Of_Recent_Tracks(int numberOfTracks) {
      var exception = Assert.Throws<ArgumentException>(() => new LastfmService("key", null, null, null).FindRecentTracks("me", numberOfTracks));
      Assert.That(exception.Message, Is.EqualTo("numberOfTracks must be a positive integer."));
    }

    [Test]
    public void FindRecentTracks_Returns_Exactly_The_Number_Of_Tracks_Requested_Even_If_Retrieves_More_From_Lastfm() {
      var parser = MockRepository.GenerateStub<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateStub<IRestClient>();
      var pageCalculator = MockRepository.GenerateStub<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=me&api_key=key&page=1";
      var response = new XElement("Response1");
      var firstTrack = "My Favorite Things";
      var secondTrack = "But Not For Me";
      var lastfmResponse = new LastfmUserRecentTracksResponse { 
        Tracks = new List<LastfmUserRecentTrack> { 
          new LastfmUserRecentTrack { Name = firstTrack }, 
          new LastfmUserRecentTrack { Name = secondTrack } 
        } 
      };
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse, 1)).Return(1);
      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response);
      parser.Expect(p => p.ParseRecentTracks(response)).Return(lastfmResponse);

      var recentTracks = new LastfmService("key", restClient, parser, pageCalculator).FindRecentTracks("me", 1);

      Assert.That(recentTracks.Count(), Is.EqualTo(1));
      Assert.That(recentTracks.First().Name, Is.EqualTo(firstTrack));
    }

    [TestCase(-11)]
    [TestCase(0)]
    public void FindTopArtists_Only_Takes_Positive_Values_For_Number_Of_Top_Artists(int numberOfArtists) {
      var exception = Assert.Throws<ArgumentException>(() => new LastfmService("key", null, null, null).FindTopArtists("me", numberOfArtists));
      Assert.That(exception.Message, Is.EqualTo("numberOfArtists must be a positive integer."));
    }

    [Test]
    public void FindTopArtists_Calls_Rest_Service_Once_For_Each_Page() {
      var parser = MockRepository.GenerateMock<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var pageCalculator = MockRepository.GenerateMock<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=1";
      var secondUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=2";
      var response1 = new XElement("Response1");
      var response2 = new XElement("Response2");
      var firstArtist = "Miles Davis";
      var secondArtist = "Devo";
      var lastfmResponse1 = new LastfmUserTopArtistsResponse { TopArtists= new List<LastfmUserTopArtist> { new LastfmUserTopArtist { Name = firstArtist } } };
      var lastfmResponse2 = new LastfmUserTopArtistsResponse { TopArtists = new List<LastfmUserTopArtist> { new LastfmUserTopArtist { Name = secondArtist } } };
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse1, 2)).Return(2);

      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response1);
      restClient.Expect(rc => rc.DownloadData(secondUri)).Return(response2);

      parser.Expect(p => p.ParseTopArtists(response1)).Return(lastfmResponse1);
      parser.Expect(p => p.ParseTopArtists(response2)).Return(lastfmResponse2);

      var topArtists = new LastfmService("key", restClient, parser, pageCalculator).FindTopArtists("me", 2);

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();

      Assert.That(topArtists.Count(), Is.EqualTo(2));
      Assert.That(topArtists[0].Name, Is.EqualTo(firstArtist));
      Assert.That(topArtists[1].Name, Is.EqualTo(secondArtist));
    }

    [Test]
    public void FindTopArtists_Returns_Exactly_The_Number_Of_Tracks_Requested_Even_If_Retrieves_More_From_Lastfm() {
      var parser = MockRepository.GenerateStub<ILastfmResponseParser>();
      var restClient = MockRepository.GenerateStub<IRestClient>();
      var pageCalculator = MockRepository.GenerateStub<IPageCalculator>();

      var firstUri = @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=me&api_key=key&page=1";
      var response = new XElement("Response1");
      var firstArtist = "Ramones";
      var secondArtist = "Misfits";
      var lastfmResponse = new LastfmUserTopArtistsResponse {
        TopArtists = new List<LastfmUserTopArtist> { 
          new LastfmUserTopArtist { Name = firstArtist }, 
          new LastfmUserTopArtist { Name = secondArtist } 
        }
      };
      pageCalculator.Stub(pc => pc.Calculate(lastfmResponse, 1)).Return(1);
      restClient.Expect(rc => rc.DownloadData(firstUri)).Return(response);
      parser.Expect(p => p.ParseTopArtists(response)).Return(lastfmResponse);

      var topArtists = new LastfmService("key", restClient, parser, pageCalculator).FindTopArtists("me", 1);

      Assert.That(topArtists.Count(), Is.EqualTo(1));
      Assert.That(topArtists.First().Name, Is.EqualTo(firstArtist));
    }
  }
}
