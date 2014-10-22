using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.EventMatchers {
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
    public OrMatcher(EventMatcher l, EventMatcher r) : base(l, r) { }
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
      return l.GetValue() <= e.scalar_value;
    }

    public override string ToString() {
      return "or more";
    }
  }

  public class LTMatcher : UnaryMatcher<IValue> {
    public LTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() >= e.scalar_value;
    }

    public override string ToString() {
      return "or less";
    }
  }

  public class EQMatcher : UnaryMatcher<IValue> {
    public EQMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() == e.scalar_value;
    }

    public override string ToString() {
      return "exactly";
    }
  }

  public class ScalarMatcher : EventMatcher {
    private ScalarEffect effect;
    private UnaryMatcher<IValue> condition;
    public ScalarMatcher(ScalarEffect effect, UnaryMatcher<IValue> condition) {
      this.effect = effect;
      this.condition = condition;
    }

    public override bool Match(GameEvent e) {
      return effect.effect_id == e.event_type && condition.Match(e);
    }

    public override string ToString() {
      return effect.ToString() + condition.ToString();
    }
  }
}
