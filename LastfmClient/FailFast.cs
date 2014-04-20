using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastfmClient {
  public static class FailFast {
    public static void IfNotPositive(int value, string name) {
      if (value < 1) {
        throw new ArgumentException(string.Format("{0} must be a positive integer.", name));
      }
    }
  }
}
