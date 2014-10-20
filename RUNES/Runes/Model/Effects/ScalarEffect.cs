using System.Collections;

namespace RUNES.Runes.Model.Effects {

  public abstract class ScalarEffect : Effect {
    private Player _target;

    public Player target {
      get { return _target; }
      set { _target = value; }
    }

    private IValue _ivalue;

    public IValue ivalue {
      get { return _ivalue; }
      set { _ivalue = value; }
    }

    public int value {
      get { return _ivalue.GetValue(); }
    }

    public string TargetName() {
      return target == user ? "the user" : "your opponent";
    }
  }
}