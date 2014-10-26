using CARDScript.Compiler;
using CARDScript.Model;
using System.Collections;

namespace CARDScript.Compiler.Effects {

  public enum Target {
    ENEMY, USER
  }

  public abstract class ScalarEffect : Effect {
    private Target _target;

    public Target target {
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
      string verb = CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id).ToLower();
      verb = verb.Substring(1, verb.Length - 2);
      return string.Format("{0} {1} {2} {3} {4}",
        target == Target.USER ? "you" : "your opponent",
        (target == Target.USER ? verb.Substring(0, verb.Length - 1) : verb),
        ivalue, Noun(), base.ToString());
    }
  }
}