﻿using CARDScript.Model.BuffEffects;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs.StatBonuses {
  internal class FlatStatBonus : Buff {
    IValue _value;
    protected int Value { get { return _value.GetValue();  } }

    internal FlatStatBonus(IValue value) {
      this._value = value;
    }
  }
}
