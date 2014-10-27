using CARDScript.Model;
using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The SpellCard Class
   * 
   * A Spell card almost corresponds to a targeted skill in league of legends.
   * They cannot be dodged. This class also provides the basis for all damaging
   * cards, including MeleeCard and SkillCard. It has a range, which means that
   * the card can only be used if you are within range distance of your 
   * opponent.
   */
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
      base.Activate(user, game_controller);
      game_controller.Opponent(user).Damage(damage);
    }

    public override string PrettyPrint() {
      return base.PrettyPrint() + string.Format("\nDamage: {0}\nRange: {1}", 
        damage, range);
    }
  }
}
