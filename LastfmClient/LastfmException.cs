using System;

namespace LastfmClient {
  public class LastfmException : Exception {
    public LastfmException() { }

    public LastfmException(string message) : base(message) { }

    public LastfmException(string message, Exception inner) : base(message, inner) { }

    public int ErrorCode { get; set; }
  }
}
