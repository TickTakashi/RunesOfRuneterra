using CARDScript.Model;
using CARDScript.Model.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  internal class KnockbackEffect : Effect {
    int distance;

    internal KnockbackEffect(int distance) {
      this.distance = distance;
    }

    internal override void Activate(Card card, Player user, Game game) {
      // TODO(ticktakashi): Implement Knockbacks.      
    }
  }
}
