using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
namespace CARDScript.Compiler.Effects.TargetedScalarEffects {
  public class Damage : TargetedScalarEffect {
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