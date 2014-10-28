using CARDScript.Compiler;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  class RepeatEffect : Effect {
    IValue count;
    Effect repeated_effect;

    public RepeatEffect(IValue count, Effect to_repeat) {
      repeated_effect = to_repeat;
      this.count = count;
    }

    public override bool Activate(Card source, IPlayer user, IGameController game_controller) {
      for (int i = 0; count.GetValue() - i > 0; i++)
        repeated_effect.Activate(source, user, game_controller);

      return next.Activate(source, user, game_controller);
    }

    public override string ToString() {
      return repeated_effect.ToString() + count.GetValue() +
        " times in a row" + base.ToString();
    }
  }
}
