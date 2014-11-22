using CARDScript.Compiler.Effects;
using CARDScript.Model.GameCards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class StunEffect : Effect {
    int strength;

    internal StunEffect(int strength) {
      this.strength = strength;
    }

    internal override void Activate(GameCard card, Player user, Game game) {
      GameState old = game.GetState();
      int count = strength;
      ChannelCallback callback = delegate(Channel c) {
        if (game.Opponent(user).OwnsChannel(c)) {
          c.RemoveFrom(game.Opponent(user), game);
          count--;
          if (count == 0) {
            game.SetState(old);
            base.Activate(card, user, game);
            return true;
          }
        }

        return false;
      };

      game.SetState(new ChooseChannelState(user, game, callback));
    }
  }
}
