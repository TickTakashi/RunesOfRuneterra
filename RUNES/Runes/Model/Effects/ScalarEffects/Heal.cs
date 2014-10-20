﻿namespace RUNES.Runes.Model.Effects.ScalarEffects {
  public class Heal : ScalarEffect {

    public override bool Activate() {
      Log("Heal event. Healing " + target + " by " + value);
      target.Heal(value);
      return base.Activate();
    }

    public override string ToString() {
      return  target + " heals " + value + " health" + base.ToString();
    }
  }
}