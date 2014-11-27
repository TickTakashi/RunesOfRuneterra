using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class KnockupEffect : Effect {
    int strength;

    internal KnockupEffect(int strength) {
      this.strength = strength;
    }

    internal override void Activate(GameCard card, Player user, Game game) {
      Knockup knockup = new Knockup(strength);
      knockup.Apply(card, game.Opponent(user), game);
      base.Activate(card, user, game);
    }
  }
}
