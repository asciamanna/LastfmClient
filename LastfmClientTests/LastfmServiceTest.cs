using LastfmClient;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmServiceTest {
    [Test]
    public void Service_Constructed_Without_An_API_Key_Throws_Exception() {
      Assert.Throws<ArgumentException>(() => new LastfmService(string.Empty, null, null), "An API key is required");
    }

    [Test]
    public void FindAllTracks_Calls_Rest_Svc_Once_For_Each_Page() {
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

      var tracks = new LastfmService("key", restClient, parser).FindAllTracks("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.AreEqual(2, tracks.Count());
      Assert.AreEqual("Tracky1", tracks[0].Name);
      Assert.AreEqual("Tracky2", tracks[1].Name);
    }

    [Test]
    public void FindAllAlbums_Calls_Rest_Svc_Once_For_Each_Page() {
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

      var albums = new LastfmService("key", restClient, parser).FindAllAlbums("me");

      restClient.VerifyAllExpectations();
      parser.VerifyAllExpectations();
      Assert.AreEqual(2, albums.Count());
      Assert.AreEqual(firstAlbum, albums[0].Name);
      Assert.AreEqual(secondAlbum, albums[1].Name);
    }
  }
}
