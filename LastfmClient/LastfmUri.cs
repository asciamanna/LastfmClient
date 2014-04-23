using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient {
  public static class LastfmUri {
    public static string LibraryTracks {
      get {
        return @"http://ws.audioscrobbler.com/2.0/?method=library.gettracks&api_key={0}&user={1}&page={2}";
      }
    }

    public static string LibraryAlbums {
      get {
        return @"http://ws.audioscrobbler.com/2.0/?method=library.getalbums&api_key={0}&user={1}&page={2}";
      }
    }

    public static string UserRecentTracks {
      get {
        return @"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user={1}&api_key={0}&page={2}";
      }
    }

    public static string UserTopArtists {
      get {
        return @"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user={1}&api_key={0}&page={2}";
      }
    }
  }
}
