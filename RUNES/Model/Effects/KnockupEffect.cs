using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
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
      Knockup knockup = new Knockup(card, strength);
      game.Opponent(user).ApplyBuff(knockup);
      base.Activate(card, user, game);
    }

    private class Knockup : Buff {
      int strength;
      
      public Knockup(Card card, int strength) : base(card) {
        this.strength = strength;
      }

      public override int ModifyActionPoints(int d, Player p, Game g) {
        int ap = d - strength;
        if (ap < 0)
          ap = 0;
        return base.ModifyActionPoints(ap, p, g);
      }
    }
  }

}
