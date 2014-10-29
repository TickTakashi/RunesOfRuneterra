using CARDScript.Compiler.Effects;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public abstract class DamageCard : Card {
    private int _damage;
    public int damage { get { return _damage; } }
  
    public DamageCard(string name, int id, int damage, int cost, int limit,
      Effect effect) : base(name, id, cost, limit, effect) {
      this._damage = damage;
      if (!effect.DealsCardDamage()) {
        Effect cardDamage = new BasicCardEffect(effect, this);
        this.effect = cardDamage;
      }
    }

    // Dont check range here, because abilities may move us towards the enemy.
    // We only check range in DealsDamageEffect.
    public override bool CanActivate(IPlayer user,
      IGameController controller) {
      return base.CanActivate(user, controller);
    }

    public abstract bool InRange(IPlayer user, IGameController controller);
  }
}
