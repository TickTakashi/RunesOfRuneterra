﻿namespace CARDScript.Model.Effects.ScalarEffects {
  public class Heal : ScalarEffect {
    public Heal(Target t, IValue v) : base(t, v) { }

    public override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Heal(value.GetValue(user, game));
      base.Activate(source, user, game);
    }
  }
}