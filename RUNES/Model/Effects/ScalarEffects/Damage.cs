namespace CARDScript.Model.Effects.ScalarEffects {
  public class Damage : ScalarEffect {
    public Damage(Target t, IValue v) : base(t, v) { }

    public override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Damage(Value);
      base.Activate(source, user, game);
    }
  }
}