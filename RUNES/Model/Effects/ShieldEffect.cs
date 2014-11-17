using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
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
      Shield shield = new Shield(card, strength);
      user.ApplyBuff(shield);
      base.Activate(card, user, game);
    }

    private class Shield : Buff {
      int strength;

      public Shield(Card card, int strength)
        : base(card) {
        this.strength = strength;
      }

      /*public override int ModifyActionPoints(int d, Player p, Game g) {
        int ap = d - strength;
        if (ap < 0)
          ap = 0;
        return base.ModifyActionPoints(ap, p, g);
      }*/
    }
  }
}
