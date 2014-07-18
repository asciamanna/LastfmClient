﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface ILastfmPageScraper {
    LastfmPlayingFrom GetLastfmPlayingFromInfo(string url);
  }

  public class LastfmPageScraper : ILastfmPageScraper {
    public LastfmPlayingFrom GetLastfmPlayingFromInfo(string url) {
      var doc = new HtmlWeb().Load(url);
      return ScrapePlayingFromInfo(doc);
    }

    public LastfmPlayingFrom GetLastfmPlayingFromInfoFromFile(string path) {
      var doc = new HtmlDocument();
      doc.LoadHtml(File.ReadAllText(path));
      return ScrapePlayingFromInfo(doc);
    }
    
    private static LastfmPlayingFrom ScrapePlayingFromInfo(HtmlDocument doc) {
      var musicServiceUrl = string.Empty;
      var musicServiceName = string.Empty;

      var span = doc.DocumentNode.SelectSingleNode("//span[@class='source']");

      if (span != null) {
        musicServiceUrl = span.FirstChild.Attributes.Where(a => a.Name == "href").Single().Value;
        musicServiceName = span.FirstChild.InnerText;
      }
      return new LastfmPlayingFrom {
        MusicServiceName = musicServiceName,
        MusicServiceUrl = musicServiceUrl
      };
    }
  }
}
