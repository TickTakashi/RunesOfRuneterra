using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The MeleeCard Class
   * 
   * Represents skills with on-hit effects in league of legends. Basically the
   * same as SpellCards except that they are prevented by Blinds and also fire
   * a on-hit event when activated.
   */
  public class MeleeCard : SpellCard {
    public MeleeCard(string name, int id, int damage, int range, int cost,
      int limit, Effect effect) : base(name, id, damage, range, cost, limit,
      effect) { }


    public override bool CanActivate(IPlayer user, IGameController game_controller) {
      return !user.IsBlind() && base.CanActivate(user, game_controller);
    }

    public override void Activate(IPlayer user, IGameController game_controller) {
      base.Activate(user, game_controller);
      // TODO(ticktakashi): Fire a OnHit event here.
    }
  }
}
