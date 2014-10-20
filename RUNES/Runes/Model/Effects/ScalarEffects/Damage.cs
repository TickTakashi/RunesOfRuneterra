namespace RUNES.Runes.Model.Effects.ScalarEffects {
  public class Damage : ScalarEffect {

    public override bool Activate() {
      Log("Damage event. Dealing " + value + " damage to " + target);
      target.Damage(value);
      return base.Activate();
    }

    public override string ToString() {
      return TargetName() + " takes " + ivalue + " damage" + base.ToString();
    }
  }


}