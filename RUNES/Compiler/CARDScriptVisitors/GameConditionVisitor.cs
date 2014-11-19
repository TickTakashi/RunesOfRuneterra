using CARDScript.Model;
using CARDScript.Model.Effects.ScalarEffects;
using CARDScript.Model.GameConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  internal class GameConditionVisitor : CARDScriptParserBaseVisitor<GameCondition> {
    IValueVisitor value_visitor;

    internal GameConditionVisitor() {
      this.value_visitor = new IValueVisitor();
    }

    internal static Inequality ParseInequality(
      CARDScriptParser.IneqContext context) {
        if (context.EQ() != null)
          return Inequality.EQ;
        else if (context.NEQ() != null)
          return Inequality.NEQ;
        else if (context.LT() != null)
          return Inequality.LT;
        else if (context.LTE() != null)
          return Inequality.LTE;
        else if (context.GT() != null)
          return Inequality.GT;
        else if (context.GTE() != null)
          return Inequality.GTE;
        else
          throw new RoRException("Equality doesn't exist!");     
    }

    public override GameCondition VisitStateCondIneq(
      CARDScriptParser.StateCondIneqContext context) {
        Inequality ineq = ParseInequality(context.ineq());
        IValue l = context.value(0).Accept<IValue>(value_visitor);
        IValue r = context.value(1).Accept<IValue>(value_visitor);
        return new IneqCondition(ineq, l, r);
    }
  }
}
