using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  class RepeatEffect : Effect {
    int count;
    Effect repeated_effect;

    public RepeatEffect(IValue count, Effect to_repeat) {
      repeated_effect = to_repeat;
      this.count = count.GetValue();
    }

    // TODO(ticktakashi): Fix. This doesn't make sense in most cases.
    public override bool Activate() {
      base.Activate();
      return --count <= 0;
    }

    // TODO(ticktakashi): Remove Debug ToString method.
    public override string ToString() {
      return repeated_effect.ToString() + ". This effect activates " + count + " times.";
    }
  }
}
