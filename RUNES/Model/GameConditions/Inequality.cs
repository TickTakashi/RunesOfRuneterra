using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.GameConditions {
  enum Inequality {
    GT,
    GTE,
    LT,
    LTE,
    EQ,
    NEQ,
  }

  public static class InequalityMethods {
    public static bool Resolve(Inequality ineq, int l, int r) {
      switch (ineq) {
        case(Inequality.GT):
          return l > r;
        case(Inequality.GTE):
          return l >= r;
        case(Inequality.LT):
          return l < r;
        case(Inequality.LTE):
          return l <= r;
        case(Inequality.EQ):
          return l == r;
        case(Inequality.NEQ):
          return l != r;
        default:
          throw new RoRException("Inequality invalid!");
      }
    }
  }
}
