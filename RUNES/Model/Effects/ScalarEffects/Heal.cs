using CARDScript.Model.GameCards;
namespace CARDScript.Model.Effects.ScalarEffects {
  internal class Heal : ScalarEffect {
    internal Heal(Target t, IValue v) : base(t, v) { }

    internal override void Activate(GameCard source, Player user, Game game) {
      Player my_target = TargetMethods.Resolve(Target, user, game);
      my_target.Heal(value.GetValue(user, game));
      base.Activate(source, user, game);
    }
  }
}