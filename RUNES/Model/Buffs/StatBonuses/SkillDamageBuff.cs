using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class SkillDamageBuff : FlatStatBonus {
    internal SkillDamageBuff(IValue value) : base(value) { }

    public override int ModifySkillDamage(int d, Player p, Game g) {
      return base.ModifySkillDamage(d + FinalValue, p, g);
    }

    public override string Print(string val) {
      return "Your skillshots deal " + val + " additional damage.";
    }
  }
}
