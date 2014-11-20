using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  internal class KnockbackEffect : Effect {
    int distance;

    internal KnockbackEffect(int distance) {
      this.distance = distance;
    }

    internal override void Activate(GameCard card, Player user, Game game) {
      bool direction_choice = game.Distance(user, game.Opponent(user)) == 0;
      if (direction_choice) {
        GameState old_state = game.GetState();
        DirectionCallback callback = delegate(bool direction) {
          Knockback(direction, game, game.Opponent(user));
          game.SetState(old_state);
          base.Activate(card, user, game);
        };
        game.SetState(new ChooseDirectionState(user, callback, game));
      } else {
        int p_x = game.GetPosition(user);
        int o_x = game.GetPosition(game.Opponent(user));
        int vec = -p_x + o_x;
        Knockback(vec > 0, game, game.Opponent(user));
        base.Activate(card, user, game);
      }
    }

    internal void Knockback(bool direction, Game game, Player target) {
      int s_distance = (direction ? 1 : -1) * distance;
      int new_position = game.GetPosition(target) + s_distance;
      
      if (new_position < 0)
        new_position = 0;

      if (new_position >= Game.FIELD_SIZE)
        new_position = Game.FIELD_SIZE - 1;

      game.SetPosition(target, new_position);
    }
  }
}
