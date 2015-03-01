using System.IO;
using System.Linq;
using HtmlAgilityPack;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface ILastfmPageScraper {
    LastfmPlayingFrom GetLastfmPlayingFromInfo(string url);
  }

  public class LastfmPageScraper : ILastfmPageScraper {
    private const string lastfmBaseUrl = "http://www.last.fm";

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
      var span = doc.DocumentNode.SelectSingleNode("//span[@class='source']");
      return CreatePlayingFrom(span);
    }

    private static LastfmPlayingFrom CreatePlayingFrom(HtmlNode span) {
      var playingFrom = new LastfmPlayingFrom();
      if (span != null) {
        playingFrom.MusicServiceUrl = FormatUrl(span.FirstChild.Attributes.Where(a => a.Name == "href").Single().Value);
        playingFrom.MusicServiceName = span.FirstChild.InnerText;
      }
      return playingFrom;
    }

    private static string FormatUrl(string url) {
      return url.StartsWith("/") ? lastfmBaseUrl + url : url;
    }
  }
}
