using CARDScript.Model.GameCards;
using System;
namespace CARDScript.Model.Effects.ScalarEffects {
  internal class Heal : ScalarEffect {
    internal Heal(Target t, IValue v) : base(t, v) { }

    internal override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Heal(value.GetValue(user, game));
      base.Activate(source, user, game);
    }

    public override string ToString() {
      return String.Format("{0} gain{1} {2} health",
        TargetMethods.Name(Target), Target == Target.ENEMY ? "s" : "",
        value.ToString()) + base.ToString();
    }
  }
}