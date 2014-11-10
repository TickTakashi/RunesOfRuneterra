using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using CARDScript.Model;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public abstract class StateMatcher : Matcher {
  }

  // TODO(ticktakashi): Implement HealthMatcher. Which should check that player Ps health meets a certin requirement, likely using GMatcher and LMatcher.
  public class HealthMatcher : StateMatcher {
    InequalityMatcher matcher;
    Target target;

    public HealthMatcher(Target target, InequalityMatcher matcher) {
      this.matcher = matcher;
      this.target = target;
    }

    public override bool Match(GameEvent UNUSED, IGameController controller, IPlayer scheduler) {
      IPlayer player = TargetMethods.Resolve(target, scheduler, controller);
      return matcher.CompareTo(player.GetHealth());
    }
  }
}
