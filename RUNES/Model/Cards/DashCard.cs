using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement DashCard
  public class DashCard : SpellCard {
    int distance;
    Effect dodge;
    
    public DashCard(int distance, string name, int id, int damage, int range,
      int cost, int limit, Effect effect, Effect dodge) :
      base(name, id, damage, range, cost, limit, effect) {
      this.distance = distance;
    }

    public override bool CanActivate(IPlayer user, IGameController game_controller) {
      return !(user.IsSnared() || user.IsStunned() || user.IsSilenced()) &&
        user.HasActionPoints(cost);
    } 

    public override void Activate(IPlayer user, IGameController game_controller) {
      user.Spend(cost);
      game_controller.PromptMove(user, distance);

      // TODO(ticktakashi): Fire an ActivationBegin event here.
      effect.Activate(this, user, game_controller);

      if (game_controller.InRange(user, range)) {
        game_controller.Opponent(user).Damage(damage);
      }
    }

    // Once again, reconsider this. We don't want to have to cast everywhere.
    // So instead, try to fire a Skillshot event and listen for it, then
    // do the appropriate things. At present, in PromptNegate you will need to
    // cast this to DashCard and then call this method.
    public void Dodge(Card dodged, IPlayer user, IGameController game_controller) {
      dodge.Activate(this, user, game_controller);
    }
  }
}
