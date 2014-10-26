using CARDScript.Model;
using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement SpellCard.
  public class SpellCard : Card {
    public int damage;
    public int range;

    public SpellCard(string name, int id, int damage, int range, int cost,
      int limit, Effect effect) : base(name, id, cost, limit, effect) {
      this.damage = damage;
      this.range = range;
    }

    public override bool CanActivate(IPlayer user, IGameController game_controller) {
      return game_controller.InRange(user, range) && base.CanActivate(user, game_controller);
    }

    public override void Activate(IPlayer user, IGameController game_controller) {
      // Activate this cards effect.
      base.Activate(user, game_controller);

      // Once it has resolved, deal Damage.
      game_controller.Opponent(user).Damage(damage);
    }

    public override string PrettyPrint() {
      return base.PrettyPrint() + string.Format("\nDamage: {0}\nRange: {1}", 
        damage, range);
    }
  }
}
