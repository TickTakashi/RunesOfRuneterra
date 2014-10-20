using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  public delegate bool EventMatcher(GameEvent e);

  public class GameEventListener {
    protected Effect callback;
    public EventMatcher ListeningFor;

    public GameEventListener(EventMatcher cond, Effect effect) {
      this.ListeningFor = cond;
      this.callback = effect;
    }

    public virtual bool Trigger(GameEvent e) {
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

  public class RepeatGameEventListener : GameEventListener {
    private IValue count;
    private int count_down = 0;
    public RepeatGameEventListener(IValue count, EventMatcher cond, Effect effect)
      : base(cond, effect) {
        this.count = count;
    }

    public override bool Trigger(GameEvent e) {
      if (ListeningFor(e)) {
        callback.Activate();
        count_down++;
        return (count.GetValue() - count_down) <= 0;
      }
      return false;
    }

    public override string ToString() {
      return "The next three times this condition is met " + ListeningFor + ", do this " + callback;
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
