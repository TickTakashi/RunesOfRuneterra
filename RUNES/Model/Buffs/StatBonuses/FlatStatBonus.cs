using CARDScript.Model.BuffEffects;
using CARDScript.Model.Cards;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal abstract class FlatStatBonus : Buff {
    protected IValue value;
    private bool value_fixed = false;
    private int _final_value;
    public int FinalValue {
      get {
        if (!value_fixed)
          throw new RoRException("Buff not initialized!");
        return _final_value;
      }
    }

    internal FlatStatBonus(IValue value) {
      this.value = value;
    }

    public override void InitBuff(Card source, Player player, Game game) {
      _final_value = value.GetValue(player, game);
      value_fixed = true;
      base.InitBuff(source, player, game);
    }

    public abstract string Print(string val);
    
    public override string ToString() {
      return Print(value.ToString()) + base.ToString();
    }

    public override string Description() {
      return Print(FinalValue.ToString()) + base.ToString();
    }
  }
}
