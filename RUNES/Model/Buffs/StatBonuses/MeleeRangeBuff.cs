using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class MeleeRangeBuff : FlatStatBonus {
    internal MeleeRangeBuff(IValue value) : base(value) { }

    public override int ModifyMeleeRange(int d, Player p, Game g) {
      return base.ModifyMeleeRange(d + value.GetValue(p, g), p, g);
    }
  }
}
