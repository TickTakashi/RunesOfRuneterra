using CARDScript.Compiler.Effects;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The DashCard Class
   * 
   * Represents dashes and mobility modifying moves in league of legends. This
   * is a special type of spell, which will move the player to a target 
   * location before deciding whether or not to deal damage.
   * Dash cards can also be used to Dodge skillshots (by discarding them) and 
   * so they have a special effect hook, dodge, that can be activated in this
   * case.
   */
  public class DashCard : SpellCard {
    private int _distance;
    public int distance { get { return _distance; } }

    Effect dodge;
    
    public DashCard(int distance, string name, int id, int damage, int range,
      int cost, int limit, Effect effect, Effect dodge) :
      base(name, id, damage, range, cost, limit, effect) {
      this._distance = distance;
      this.dodge = dodge;
    }

    public override bool CanActivate(IPlayer user, IGameController game_controller) {
      return !(user.HasBuff(BuffType.SNARE) || user.HasBuff(BuffType.STUN) ||
        user.HasBuff(BuffType.SILENCE)) && user.HasActionPoints(cost);
    } 

    public override void Activate(IPlayer user, IGameController game_controller) {
      user.Spend(cost);
      game_controller.PromptMove(user, _distance);

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
