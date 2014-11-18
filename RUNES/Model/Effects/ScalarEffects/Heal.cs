namespace CARDScript.Model.Effects.ScalarEffects {
  public class Heal : ScalarEffect {
    public Heal(Target t, IValue v) : base(t, v) { }

    public override void Activate(Card source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Heal(Value);
      base.Activate(source, user, game);
    }
  }
}