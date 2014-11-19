using CARDScript.Model.BuffEffects;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class FlatStatBonus : Buff {
    protected IValue value;

    internal FlatStatBonus(IValue value) {
      this.value = value;
    }
  }
}
