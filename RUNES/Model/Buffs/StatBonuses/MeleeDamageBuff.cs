using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class MeleeDamageBuff : FlatStatBonus {
    internal MeleeDamageBuff(IValue value) : base(value) { }

    public override int ModifyMeleeDamage(int d, Player p, Game g) {
      return base.ModifyMeleeDamage(d + value.GetValue(p, g), p, g);
    }
  }
}
