using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public class NullMatcher<T> : Matcher<T> {
    public override bool Match(T e) {
      return true;
    }
  }

  public abstract class MatcherBinop<T> : Matcher<T> {
    protected Matcher<T> l;
    protected Matcher<T> r;
    public MatcherBinop(Matcher<T> l, Matcher<T> r) {
      this.l = l;
      this.r = r;
    }
  }

  public class OrMatcher<T> : MatcherBinop<T> {
    public OrMatcher(Matcher<T> l, Matcher<T> r) : base(l, r) { }
    public override bool Match(T e) {
      return l.Match(e) || r.Match(e);
    }
  }

  public class AndMatcher<T> : MatcherBinop<T> {
    public AndMatcher(Matcher<T> l, Matcher<T> r) : base(l, r) { }
    public override bool Match(T e) {
      return l.Match(e) && r.Match(e);
    }
  }

  public class NotMatcher<T> : Matcher<T> {
    Matcher<T> l;
    public NotMatcher(Matcher<T> l) {
      this.l = l;
    }
    public override bool Match(T e) {
      return !l.Match(e);
    }

    public override string ToString() {
      return "not " + l;
    }
  }

  public abstract class InequalityMatcher : Matcher<GameEvent> {
    protected IValue l;
    public InequalityMatcher(IValue l) { 
      this.l = l;
    }
  }

  public class GTMatcher : InequalityMatcher {
    public GTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() <= e.scalar_value;
    }

    public override string ToString() {
      return "or more";
    }
  }

  public class LTMatcher : InequalityMatcher {
    public LTMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() >= e.scalar_value;
    }

    public override string ToString() {
      return "or less";
    }
  }

  public class EQMatcher : InequalityMatcher {
    public EQMatcher(IValue v) : base(v) { }
    public override bool Match(GameEvent e) {
      return l.GetValue() == e.scalar_value;
    }

    public override string ToString() {
      return "exactly";
    }
  }

  public class ScalarMatcher : Matcher<GameEvent> {
    private ScalarEffect effect;
    private InequalityMatcher condition;
    public ScalarMatcher(ScalarEffect effect, InequalityMatcher condition) {
      this.effect = effect;
      this.condition = condition;
    }

    // TODO(ticktakashi): Need to make sure this matches the right player
    public override bool Match(GameEvent e) {
      return effect.effect_id == e.event_type && condition.Match(e);
    }

    public override string ToString() {
      return effect.ToString() + condition.ToString();
    }
  }
}
