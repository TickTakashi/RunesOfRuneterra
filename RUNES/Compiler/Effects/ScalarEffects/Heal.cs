using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Heal : ScalarEffect {

    public override bool Activate(Card source, IPlayer user, IGameController game_controller) {
      Log("Heal event. Healing " + target + " by " + value);
      IPlayer my_target = user;
      if (target == Target.ENEMY)
        my_target = game_controller.Opponent(my_target);
      my_target.Heal(value);
      return base.Activate(source, user, game_controller);
    }
    
    protected override string Noun() { return "health"; }
  }
}