using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LastfmClient {
  public interface ILastfmResponseParser {
    LastfmLibraryTrackResponse Parse(XElement xmlResponse);
  }

  public class LastfmResponseParser : ILastfmResponseParser {
    public LastfmLibraryTrackResponse Parse(XElement xmlResponse) {
      var tracks = xmlResponse.DescendantsAndSelf("tracks");
      var tracksElement = tracks.First();

      return new LastfmLibraryTrackResponse {
        Status = xmlResponse.Attribute("status").Value,
        Page = Int32.Parse(tracksElement.Attribute("page").Value),
        PerPage = Int32.Parse(tracksElement.Attribute("perPage").Value),
        TotalPages = Int32.Parse(tracksElement.Attribute("totalPages").Value),
        TotalRecords = Int32.Parse(tracksElement.Attribute("total").Value),
        Tracks = BuildTracks(tracks.Descendants("track")).ToList(),
      };
    }
    
    IEnumerable<LastfmLibraryTrack> BuildTracks(IEnumerable<XElement> tracks) {
      var libraryTracks = new List<LastfmLibraryTrack>();
      foreach (var track in tracks.Where(t => t.Element("album") != null)) {
        libraryTracks.Add(new LastfmLibraryTrack {
          Name = track.Element("name").Value,
          Album = track.Element("album").Element("name").Value,
          Artist = track.Element("artist").Element("name").Value,
          PlayCount = Int32.Parse(track.Element("playcount").Value),
        });
      }
      return libraryTracks;
    }
  }
}
