using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class SpellDamageBuff : FlatStatBonus {
    internal SpellDamageBuff(IValue value) : base(value) { }

    public override int ModifySpellDamage(int d, Player p, Game g) {
      return base.ModifySpellDamage(d + value.GetValue(p, g), p, g);
    }
  }
}
