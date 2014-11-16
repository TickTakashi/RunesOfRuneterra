using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using CARDScript.Model;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {

  // TODO(ticktakashi): Implement HealthMatcher. Which should check that player Ps health meets a certin requirement, likely using GMatcher and LMatcher.
  public class HealthMatcher : Matcher {
    InequalityMatcher matcher;
    Target target;

    public HealthMatcher(Target target, InequalityMatcher matcher) {
      this.matcher = matcher;
      this.target = target;
    }

    public override bool Match(GameEvent_OLD UNUSED, IGameController controller, IPlayer scheduler) {
      IPlayer player = TargetMethods.Resolve(target, scheduler, controller);
      return matcher.CompareTo(player.GetHealth());
    }

    public override string ToString() {
      return TargetMethods.Owner(target) + " health is " + matcher.StringWithInt();
    }
  }


  public class DistanceMatcher : Matcher {
    InequalityMatcher matcher;

    public DistanceMatcher(InequalityMatcher matcher) {
      this.matcher = matcher;
    }

    public override bool Match(GameEvent_OLD UNUSED, IGameController controller, IPlayer scheduler) {
      return matcher.CompareTo(controller.PlayerDistance());
    }

    public override string ToString() {
      return "the distance between you and your opponent is " + matcher.StringWithInt();
   }
  }
}
