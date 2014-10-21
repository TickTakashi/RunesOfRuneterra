﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  class RepeatEffect : Effect {
    IValue count;
    Effect repeated_effect;

    public RepeatEffect(IValue count, Effect to_repeat) {
      repeated_effect = to_repeat;
      this.count = count;
    }

    public override bool Activate() {
      for (int i = 0; count.GetValue() - i > 0; i++)
        repeated_effect.Activate();

      return true;
    }

    public override string ToString() {
      return repeated_effect.ToString() + count.GetValue() + " times in a row" + base.ToString();
    }
  }
}
