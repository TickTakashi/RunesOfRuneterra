using CARDScript.Compiler.Effects;
using CARDScript.Compiler.EventMatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Events {
  public class GameEventListener {
    protected Effect callback;
    public EventMatcher listening_for;

    public GameEventListener(EventMatcher cond, Effect effect) {
      this.listening_for = cond;
      this.callback = effect;
    }

    public virtual bool Trigger(GameEvent e) {
      if (listening_for.Match(e)) {
        return callback.Activate();
      } else {
        return false;
      }
    }

    public override string ToString() {
      return "the next time " + listening_for + ": " + callback;
    }
  }
}
