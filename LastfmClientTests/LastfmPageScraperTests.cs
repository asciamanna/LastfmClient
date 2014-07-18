﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LastfmClient;
using NUnit.Framework;

namespace LastfmClientTests {
  [TestFixture]
  public class LastfmPageScraperTests {
    private LastfmPageScraper scraper;

    [SetUp]
    public void SetUp() {
      scraper = new LastfmPageScraper();
    }

    [Test]
    public void GetLastfmPlayingFromInfoFromFile_While_User_Is_Currently_Playing() {
      var listeningFrom = scraper.GetLastfmPlayingFromInfoFromFile("lastfmUserPageCurrentlyPlaying.html");
      Assert.That(listeningFrom.MusicServiceName, Is.EqualTo("Spotify"));
      Assert.That(listeningFrom.MusicServiceUrl, Is.EqualTo(@"http://www.spotify.com/"));
    }

    [Test]
    public void GetLastfmPlayingFromInfoFromFile_While_User_Is_Not_Playing_Returns_Emtpy_Result() {
      var listeningFrom = scraper.GetLastfmPlayingFromInfoFromFile("lastfmUserPageNotCurrentlyPlaying.html");
      Assert.That(String.IsNullOrWhiteSpace(listeningFrom.MusicServiceName), Is.True);
      Assert.That(String.IsNullOrWhiteSpace(listeningFrom.MusicServiceUrl), Is.True);
    }

    [Test, Category("Integration")]
    public void GetLastfmPlayingFromInfo_User_Page_Exists() {
      var pageUrl = @"http://www.last.fm/user/asciamanna";
      Assert.DoesNotThrow(() => scraper.GetLastfmPlayingFromInfo(pageUrl));
    }

    [Test, Category("Integration")]
    public void GetLastfmPlayingFromInfo_User_Page_Does_Not_Exist_Returns_Empty_Result() {
      var badPageUrl = @"http://www.last.fm/user/MadeUpNameDoesNotExistArgh";
      var result = scraper.GetLastfmPlayingFromInfo(badPageUrl);
      Assert.That(String.IsNullOrWhiteSpace(result.MusicServiceName), Is.True);
      Assert.That(String.IsNullOrWhiteSpace(result.MusicServiceUrl), Is.True);
    }
  }
}
