using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using CARDScript.Model;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public class NullMatcher : Matcher {
    public override bool Match(GameEvent e, IGameController controller, IPlayer player) {
      return true;
    }
  }

  public abstract class MatcherBinop : Matcher {
    protected Matcher l;
    protected Matcher r;
    public MatcherBinop(Matcher l, Matcher r) {
      this.l = l;
      this.r = r;
    }
  }

  public class OrMatcher : MatcherBinop {
    public OrMatcher(Matcher l, Matcher r) : base(l, r) { }
    public override bool Match(GameEvent e, IGameController controller, IPlayer player) {
      return l.Match(e, controller, player) || r.Match(e, controller, player);
    }
  }

  public class AndMatcher : MatcherBinop {
    public AndMatcher(Matcher l, Matcher r) : base(l, r) { }
    public override bool Match(GameEvent e, IGameController controller, IPlayer player) {
      return l.Match(e, controller, player) && r.Match(e, controller, player);
    }
  }

  public class NotMatcher : Matcher {
    Matcher l;
    public NotMatcher(Matcher l) {
      this.l = l;
    }
    public override bool Match(GameEvent e, IGameController controller, IPlayer player) {
      return !l.Match(e, controller, player);
    }

    public override string ToString() {
      return "not " + l;
    }
  }

  public abstract class InequalityMatcher {
    public IValue l;
    public InequalityMatcher(IValue l) { 
      this.l = l;
    }

    public abstract bool CompareTo(int rval);

    public string StringWithInt() {
      return l.ToString() + " " + ToString();
    }
  }

  public class GTMatcher : InequalityMatcher {
    public GTMatcher(IValue v) : base(v) { }
    public override bool CompareTo(int value) {
      return l.GetValue() <= value;
    }

    public override string ToString() {
      return "or more";
    }
  }

  public class LTMatcher : InequalityMatcher {
    public LTMatcher(IValue v) : base(v) { }
    public override bool CompareTo(int value) {
      return l.GetValue() >= value;
    }

    public override string ToString() {
      return "or less";
    }
  }

  public class EQMatcher : InequalityMatcher {
    public EQMatcher(IValue v) : base(v) { }
    public override bool CompareTo(int value) {
      return l.GetValue() == value;
    }

    public override string ToString() {
      return "exactly";
    }
  }

  public class ScalarMatcher : Matcher {
    private TargetedScalarEffect effect;
    private InequalityMatcher condition;
    public ScalarMatcher(TargetedScalarEffect effect, InequalityMatcher condition) {
      this.effect = effect;
      this.condition = condition;
    }

    // TODO(ticktakashi): Need to make sure this matches the right player
    public override bool Match(GameEvent e, IGameController controller, IPlayer player) {
      return effect.effect_id == e.event_type && condition.CompareTo(e.scalar_value);
    }

    public override string ToString() {
      return effect.ToString() + condition.ToString();
    }
  }
}
