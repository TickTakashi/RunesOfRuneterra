using CARDScript.Compiler;
using CARDScript.Compiler.Effects.ScalarEffects;
using CARDScript.Model;
using CARDScript.Model.Players;
using System.Collections;

namespace CARDScript.Compiler.Effects {

  
  public abstract class TargetedScalarEffect : ScalarEffect {
    private Target _target;

    public Target target {
      get { return _target; }
      set { _target = value; }
    }

    // TODO(ticktakashi): Reconsider the structure of this ToString() method to
    // change this in future.
    protected abstract string Noun();

    public override string ToString()  {
      string verb = CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id).ToLower();
      verb = verb.Substring(1, verb.Length - 2);
      return string.Format("{0} {1} {2} {3}{4}",
        target == Target.USER ? "you" : "your opponent",
        (target == Target.USER ? verb.Substring(0, verb.Length - 1) : verb),
        ivalue, Noun(), base.ToString());
    }
  }
}