using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  public delegate bool EventMatcher(GameEvent e);

  public class GameEventListener {
    Effect callback;
    public EventMatcher ListeningFor;

    public GameEventListener(EventMatcher cond, Effect effect) {
      this.ListeningFor = cond;
      this.callback = effect;
    }

    public bool Trigger(GameEvent e) {
      if (ListeningFor(e)) {
        return callback.Activate();
      } else {
        return false;
      }
    }

    public override string ToString() {
      return "Activates this effect: '" + callback + "' when " +
        "this condition is met: " + ListeningFor;
    }
  }

  // Game event struct which contains necessary information about 
  // events. This is kind of a chimera struct, so think about how you can
  // neatly divide this struct up into a base and derived structs.
  public class GameEvent {
    public int event_type = -1;
    public int scalar_value = -1;
  }

}
