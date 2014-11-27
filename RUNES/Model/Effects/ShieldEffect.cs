using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class ShieldEffect : Effect {
    int strength;

    internal ShieldEffect(int strength) {
      this.strength = strength;
    }

    internal override void Activate(GameCard card, Player user, Game game) {
      Shield shield = new Shield(strength);
      shield.Apply(card, user, game);
      base.Activate(card, user, game);
    }
  }
}
