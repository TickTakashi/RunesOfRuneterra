using CARDScript.Model;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs.StatBonuses;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  internal class BuffVisitor : CARDScriptParserBaseVisitor<Buff> {
    IValueVisitor value_visitor;

    internal BuffVisitor() {
      this.value_visitor = new IValueVisitor();
    }

    public override Buff VisitBuffEffect(
      CARDScriptParser.BuffEffectContext context) {
      if (context.statB() != null)
        return context.statB().Accept<Buff>(this);
      else
        return null;
    }

    public override Buff VisitStatBFlat(
      CARDScriptParser.StatBFlatContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      switch (context.bonusB().Start.TokenIndex) {
        case (CARDScriptParser.MELEE_D):
          return new MeleeDamageBuff(value);
        case (CARDScriptParser.MELEE_R):
          return new MeleeRangeBuff(value);
        case (CARDScriptParser.SKILL_D):
          return new SkillDamageBuff(value);
        case(CARDScriptParser.SPELL_D):
          return new SpellDamageBuff(value);
        default:
          throw new RoRException(
            "COMPILER: This bonusB is not yet implemented!");
      }
    }
  }
}
