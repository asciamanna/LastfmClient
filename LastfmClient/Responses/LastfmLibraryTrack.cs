using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmLibraryTrack {
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public int PlayCount { get; set; }
  }
}
