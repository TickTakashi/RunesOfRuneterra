using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class SkillCard : DamageCard {
    int range;

    public SkillCard(string name, int id, bool is_ult, bool is_dash,
      int dash_distance, int cost, int limit, int damage, int range)
      : base(name, id, is_ult, is_dash, dash_distance, cost, limit, damage) {
        this.range = range;
    }

    public override int Damage(Player user, Game game) {
      return base.Damage(user, game) + user.BonusSkillDamage();
    }

    public override int Range(Player user, Game game) {
      return range + user.BonusSkillRange();
    }

    public override NormalEffect CreateEffect() {
      return new SkillshotEffect();
    }
  }

  public class SkillCardBuilder : CardBuilder {
    int damage;
    int range;
    internal override GameCard Build() {
      SkillCard ret = new SkillCard(name, id, is_ult, is_dash, dash_distance,
        cost, limit, damage, range);
      ret.SetEffect(effect);
      return ret;
    }

    internal SkillCardBuilder WithDamage(int damage) {
      this.damage = damage;
      return this;
    }

    internal SkillCardBuilder WithRange(int range) {
      this.range = range;
      return this;
    }
  }
}
