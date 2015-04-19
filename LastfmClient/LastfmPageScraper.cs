using System.IO;
using System.Linq;
using HtmlAgilityPack;
using LastfmClient.Responses;

namespace LastfmClient {
  public interface ILastfmPageScraper {
    LastfmMusicSource GetLastfmMusicSource(string url);
  }

  public class LastfmPageScraper : ILastfmPageScraper {
    private const string lastfmBaseUrl = "http://www.last.fm";

    public LastfmMusicSource GetLastfmMusicSource(string url) {
      var doc = new HtmlWeb().Load(url);
      return ScrapePageForMusicSource(doc);
    }

    public LastfmMusicSource GetLastfmMusicSourceFromFile(string path) {
      var doc = new HtmlDocument();
      doc.LoadHtml(File.ReadAllText(path));
      return ScrapePageForMusicSource(doc);
    }
    
    private static LastfmMusicSource ScrapePageForMusicSource(HtmlDocument doc) {
      var span = doc.DocumentNode.SelectSingleNode("//span[@class='source']");
      return CreateMusicSource(span);
    }

    private static LastfmMusicSource CreateMusicSource(HtmlNode span) {
      var musicSource = new LastfmMusicSource();
      if (span != null) {
        musicSource.MusicServiceUrl = FormatUrl(span.FirstChild.Attributes.Single(a => a.Name == "href").Value);
        musicSource.MusicServiceName = span.FirstChild.InnerText;
      }
      return musicSource;
    }

    private static string FormatUrl(string url) {
      return url.StartsWith("/") ? lastfmBaseUrl + url : url;
    }
  }
}
