using CARDScript.Compiler.Effects;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class MeleeCard : DamageCard {
    public MeleeCard(string name, int id, bool is_ult, bool is_dash, 
      int dash_distance, int cost, int limit, int damage) : base(name, id, 
      is_ult, is_dash, dash_distance, cost, limit, damage) {}

    public override int Damage(Player user, Game game) {
      return user.MeleeDamage() + base.Damage(user, game);
    }

    public override int Range(Player user, Game game) {
      return user.MeleeRange();
    }

    internal override NormalEffect CreateEffect() {
      return new MeleeEffect();
    }
  }

  internal class MeleeCardBuilder : CardBuilder {
    int damage;

     internal override GameCard Build() {
      MeleeCard ret = new MeleeCard(name, id, is_ult, is_dash, dash_distance, 
        cost, limit, damage);
      ret.SetEffect(effect);
      return ret;
    }

    internal MeleeCardBuilder WithDamage(int damage) {
      this.damage = damage;
      return this;
    }
  }
}
