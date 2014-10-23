using CARDScript.Compiler;
using System.Collections;

namespace CARDScript.Compiler.Effects {

  public abstract class ScalarEffect : Effect {
    private IPlayer _target;
   
    public IPlayer target {
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

    private int _effect_id;

    public int effect_id {
      get { return _effect_id; }
      set { _effect_id = value; }
    }

    // TODO(ticktakashi): Reconsider the structure of this ToString() method to
    // change this in future.
    protected abstract string Noun();

    public override string ToString()  {
      string verb = CARDScriptParser.tokenNames[effect_id].ToLower();
      verb = verb.Substring(1, verb.Length - 2);
      return string.Format("{0} {1} {2} {3} {4}",
        target == user ? "you" : "your opponent",
        (user == target ? verb.Substring(0, verb.Length - 1) : verb),
        ivalue, Noun(), base.ToString());
    }
  }
}