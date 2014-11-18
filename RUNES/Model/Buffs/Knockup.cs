using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs {
  internal class Knockup : Buff {
    int strength;

    public Knockup(int strength) {
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
