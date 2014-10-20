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

    public override string ToString() {
      return "not " + l;
    }
  }

  public class GTMatcher : UnaryMatcher<IValue> {
    public GTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() < e.scalar_value;
    }

    public override string ToString() {
      return "more than " + l;
    }
  }

  public class LTMatcher : UnaryMatcher<IValue> {
    public LTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() > e.scalar_value;
    }

    public override string ToString() {
      return "less than " + l;
    }
  }

  public class EQMatcher : UnaryMatcher<IValue> {
    public EQMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() == e.scalar_value;
    }

    public override string ToString() {
      return "exactly " + l;
    }
  }

  public class EventTypeMatcher : UnaryMatcher<int> {
    public EventTypeMatcher(int i) : base(i) { }
    public override bool Match(GameEvent e) {
      return e.event_type == l;
    }

    public string TypeName() {
      String action = RunesParser.tokenNames[l].ToLower();
      return action.Substring(1, action.Length - 2);
    }

    // TODO(ticktakashi): Make this more sophisticated.
    public string Suffix() {
      return "damage";
    }
  }

  // TODO(ticktakashi): Reconsider this usage of user, passing it around like this doesn't seem smart.
  public class ScalarMatcher : EventMatcher {
    private Player user;
    private Player target;
    private EventTypeMatcher typecheck;
    private UnaryMatcher<IValue> condition;
    public ScalarMatcher(Player user, Player target, EventTypeMatcher typecheck, UnaryMatcher<IValue> condition) {
      this.user = user;
      this.target = target;
      this.typecheck = typecheck;
      this.condition = condition;
    }

    public override bool Match(GameEvent e) {
      return typecheck.Match(e) && condition.Match(e);
    }

    public override string ToString() {
      return ScalarEffect.TargetName(user, target) + " " + typecheck.TypeName() + " " + condition +
        " " + typecheck.Suffix();
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
      return "the next time " + listening_for + ", " + callback;
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
      return "the next " + count + " times " + listening_for + ", " + callback;
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
