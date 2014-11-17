using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class ShieldEffect : Effect {
    int strength;

    public ShieldEffect(int strength) {
      this.strength = strength;
    }

    public override void Activate(Card card, Player user, Game game) {
      // TODO(TickTakashi): Shields.
      base.Activate(card, user, game);
    }
  }
}
