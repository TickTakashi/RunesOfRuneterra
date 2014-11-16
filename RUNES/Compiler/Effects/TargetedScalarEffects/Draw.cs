using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
namespace CARDScript.Compiler.Effects.TargetedScalarEffects {
  public class Draw : TargetedScalarEffect {

    public override bool Activate(Card_OLD source, IPlayer user, IGameController game_controller) {
      Log("Draw event. " + target + " draws " + value);
      IPlayer my_target = user;
      if (target == Target.ENEMY)
        my_target = game_controller.Opponent(my_target);
      my_target.Draw(value);
      return base.Activate(source, user, game_controller);
    }

    protected override string Noun() { return value > 1 ? "cards" : "card" ; }
  }
}
