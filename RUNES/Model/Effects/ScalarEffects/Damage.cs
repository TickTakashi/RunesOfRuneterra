using CARDScript.Model.GameCards;
namespace CARDScript.Model.Effects.ScalarEffects {
  internal class Damage : ScalarEffect {
    internal Damage(Target t, IValue v) : base(t, v) { }

    internal override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Damage(value.GetValue(user, game));
      base.Activate(source, user, game);
    }
  }
}