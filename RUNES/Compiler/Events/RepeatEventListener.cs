using CARDScript.Compiler.Effects;
using CARDScript.Compiler.EventMatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Events {
  public class RepeatEventListener : GameEventListener {
    private IValue count;
    private int count_down = 0;
    public RepeatEventListener(IValue count, EventMatcher cond,
                                   Effect effect)
      : base(cond, effect) {
      this.count = count;
    }

    public override bool Trigger(GameEvent e) {
      if (listening_for.Match(e)) {
        callback.Activate();
        count_down++;
        return (count.GetValue() - count_down) <= 0;
      }
      return false;
    }

    public override string ToString() {
      return "the next " + count + " times " + listening_for + ": " + callback;
    }
  }
}
