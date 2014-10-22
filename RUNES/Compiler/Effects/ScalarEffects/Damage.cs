using CARDScript.Compiler.Effects;
namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Damage : ScalarEffect {

    public override bool Activate() {
      Log("Damage event. Dealing " + value + " damage to " + target);
      target.Damage(value);
      return base.Activate();
    }

    protected override string Noun() { return "damage"; }
  }


}