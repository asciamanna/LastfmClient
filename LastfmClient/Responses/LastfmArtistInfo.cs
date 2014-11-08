using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public class LastfmArtistInfo {
    public string Name { get; set; }
    public string Mbid {  get; set;}
    public string PlaceFormed { get; set; }
    public int? YearFormed { get; set; }
    public string BioSummary { get; set; }
  }
}
