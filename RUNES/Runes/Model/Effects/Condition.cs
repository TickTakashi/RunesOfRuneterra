using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  public abstract class EventMatcher {
    public abstract bool Match(GameEvent e);
  }

  public abstract class BinaryMatcher<T> : EventMatcher {
    protected T l;
    protected T r;
    public BinaryMatcher(T l, T r) {
      this.l = l;
      this.r = r;
    }
  }

  public class OrMatcher : BinaryMatcher<EventMatcher> {
    public OrMatcher(EventMatcher l, EventMatcher r) : base(l,r) { }
    public override bool Match(GameEvent e) {
      return l.Match(e) || r.Match(e); 
    }
  }

  public class AndMatcher : BinaryMatcher<EventMatcher> {
    public AndMatcher(EventMatcher l, EventMatcher r) : base(l, r) { }
    public override bool Match(GameEvent e) {
      return l.Match(e) && r.Match(e);
    }
  }

  public abstract class UnaryMatcher<T> : EventMatcher {
    protected T l;
    public UnaryMatcher(T l) {
      this.l = l;
    }
  }

  public class NotMatcher : UnaryMatcher<EventMatcher> {
    public NotMatcher(EventMatcher e) : base(e) { }
    public override bool Match(GameEvent e) {
      return !l.Match(e);
    }
  }

  public class GTMatcher : UnaryMatcher<IValue> {
    public GTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() < e.scalar_value;
    }
  }

  public class LTMatcher : UnaryMatcher<IValue> {
    public LTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() > e.scalar_value;
    }
  }

  public class EQMatcher : UnaryMatcher<IValue> {
    public EQMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() == e.scalar_value;
    }
  }

  public class EventTypeMatcher : UnaryMatcher<int> {
    public EventTypeMatcher(int i) : base(i) { }
    public override bool Match(GameEvent e) {
      return e.event_type == l;
    }
  }

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
      return "Activates this effect: '" + callback + "' when " +
        "this condition is met: " + listening_for;
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
      if (listening_for.Match(e)) {
        callback.Activate();
        count_down++;
        return (count.GetValue() - count_down) <= 0;
      }
      return false;
    }

    public override string ToString() {
      return "The next three times this condition is met " + listening_for + ", do this " + callback;
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
