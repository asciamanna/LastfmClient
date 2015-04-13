﻿using System;
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
    [Test]
    public void GetInfo_Encodes_Album_And_Artist() {
      var restClient = MockRepository.GenerateMock<IRestClient>();
      var parser = MockRepository.GenerateMock<IAlbumResponseParser>();
      const string apiKey = "key";
      const string artist = "Bobby Hutcherson";
      const string album = "San Francisco";
      var response = new XElement("Response");
      var expectedUri = CreateExpectedUri(apiKey, Uri.EscapeDataString(artist), Uri.EscapeDataString(album));
      var albumInfo = new LastfmAlbumInfo();
      restClient.Stub(rc => rc.DownloadData(expectedUri)).Return(response);
      parser.Stub(p => p.Parse(response)).Return(albumInfo);

      var repository = new AlbumRepository(apiKey, restClient, parser);

      Assert.That(repository.GetInfo(artist, album), Is.SameAs(albumInfo));
    }

    private static string CreateExpectedUri(string apiKey, string artist, string album) {
      return string.Format(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key={0}&artist={1}&album={2}",
         apiKey, artist, album);
    }
  }
}
