using System;

namespace LastfmClient {
  public static class FailFast {
    public static void IfNotPositive(int value, string name) {
      if (value < 1) {
        throw new ArgumentException(string.Format("{0} must be a positive integer.", name));
      }
    }
  }
}
