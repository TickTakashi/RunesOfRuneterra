using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class SpellCard : DamageCard {
    int range;

    public SpellCard(string name, int id, bool is_ult, bool is_dash,
      int dash_distance, int cost, int limit, int damage, int range)
      : base(name, id, is_ult, is_dash, dash_distance, cost, limit, damage) {
      this.range = range;
    }

    public override int Damage(Player user, Game game) {
      return base.Damage(user, game) + user.BonusSpellDamage();
    }

    public override int Range(Player user, Game game) {
      return range + user.BonusSpellRange();
    }

    public override NormalEffect CreateEffect() {
      return new DamageEffect();
    }
  }

  public class SpellCardBuilder : CardBuilder {
    int damage;
    int range;
    internal override Card Build() {
      return new SpellCard(name, id, is_ult, is_dash, dash_distance, cost,
        limit, damage, range);
    }

    internal SpellCardBuilder WithDamage(int damage) {
      this.damage = damage;
      return this;
    }

    internal SpellCardBuilder WithRange(int range) {
      this.range = range;
      return this;
    }
  }
}
