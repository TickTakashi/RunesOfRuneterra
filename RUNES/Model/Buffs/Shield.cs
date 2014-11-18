using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs {
  internal class Shield : Buff {
    int strength;

    public Shield(int strength) {
      this.strength = strength;
    }

    public override int ModifyDamage(int d, Player p, Game g) {
      if (d >= strength) {
        d -= strength;
        strength = 0;
      } else {
        strength -= d;
        d = 0;
      }
      return base.ModifyDamage(d, p, g);
    }
  }
}
