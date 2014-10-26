using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Damage : ScalarEffect {
    public override bool Activate(Card source, IPlayer user, IGameController game_controller) {
      Log("Damage event. Dealing " + value + " damage to " + target);
      IPlayer my_target = user;
      if (target == Target.ENEMY)
        my_target = game_controller.Opponent(my_target);
      my_target.Damage(value);
      return base.Activate(source, user, game_controller);
    }

    protected override string Noun() { return "damage"; }
  }
}