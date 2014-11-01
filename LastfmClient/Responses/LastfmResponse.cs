﻿using System.Collections.Generic;

namespace LastfmClient.Responses {
  public interface ILastfmResponse<T> {
    string Status { get; set; }
    int Page { get; set; }
    int PerPage { get; set; }
    int TotalPages { get; set; }
    int TotalRecords { get; set; }
    IEnumerable<T> Items { get; set; }
  }

  public class LastfmResponse<T>  : ILastfmResponse<T> {
    public string Status { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Items { get; set; }
  }
}
