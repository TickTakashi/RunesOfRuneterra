using CARDScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public abstract class StateMatcher : Matcher<IGameController> {
  }

  public class HealthMatcher : StateMatcher {

    public override bool Match(IGameController e) {
      throw new NotImplementedException();
    }
  }
}
