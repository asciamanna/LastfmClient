using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient.Responses {
  public interface ILastfmResponse {
    string Status { get; set;}
    int Page { get; set; }
    int PerPage { get; set; }
    int TotalPages { get; set; }
    int TotalRecords { get; set; }
  }

  public abstract class LastfmResponse  {
    public string Status { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
  }
}
