using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.ScalarEffects {
  public enum Target {
    ENEMY, USER
  }

  public static class TargetMethods {
    public static Player Resolve(Target t, Player activator, Game game) {
      if (t == Target.USER) {
        return activator;
      } else {
        return game.Opponent(activator);
      }
    }

    public static string Name(Target target) {
      return target == Target.USER ? "you" : "your opponent";
    }

    public static string Owner(Target target) {
      return target == Target.USER ? "your" : "your opponents";
    }
  }

}
