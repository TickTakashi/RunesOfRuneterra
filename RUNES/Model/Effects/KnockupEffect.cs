using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class KnockupEffect : Effect {
    int strength;

    public KnockupEffect(int strength) {
      this.strength = strength;
    }

    public override void Activate(Card card, Player user, Game game) {
      Player target = game.Opponent(user);
      Knockup knockup = new Knockup(target, strength);
      game.Attach(knockup);
      base.Activate(card, user, game);
    }

    private class Knockup : IRoRObserver<GameEvent> {
      Player target;
      int strength;
      
      public Knockup(Player target, int strength) {
        this.target = target;
      }

      public void Update(GameEvent change_event) {
        if (change_event.type == GameEvent.Type.TURN_START &&
          change_event.player == target) {
            target.ModifyAP(-strength);
            change_event.game.Detach(this);
        }
      }
    }
  }

}
