namespace CARDScript.Model.Effects.ScalarEffects {
  public class Draw : ScalarEffect {
    public Draw(Target t, IValue v) : base(t, v) { }

    public override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Draw(value.GetValue(user, game));
      base.Activate(source, user, game);
    }
  }
}
