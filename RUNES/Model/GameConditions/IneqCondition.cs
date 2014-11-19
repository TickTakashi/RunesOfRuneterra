using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.GameConditions {
  class IneqCondition : GameCondition {
    Inequality ineq;
    IValue l;
    IValue r;

    public IneqCondition(Inequality ineq, IValue l, IValue r) {
      this.l = l;
      this.r = r;
      this.ineq = ineq;
    }
    
    public bool Condition(Player player, Game game) {
      int left = l.GetValue(player, game);
      int right = r.GetValue(player, game);
      return InequalityMethods.Resolve(ineq, left, right);
    }
  }
}
