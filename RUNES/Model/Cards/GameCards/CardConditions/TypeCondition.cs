using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards.CardConditions {
  internal class TypeCondition : GameCardCondition {
    CardType type;

    internal TypeCondition(CardType type) {
      this.type = type;
    }

    public bool Condition(GameCard c) {
      switch (type) {
        case (CardType.DAMAGE):
          return c is DamageCard;
        case (CardType.MELEE):
          return c is MeleeCard;
        case (CardType.SELF):
          return c is BuffCard;
        case (CardType.SKILL):
          return c is SkillCard;
        case (CardType.SPELL):
          return c is SpellCard;
        default:
          throw new RoRException("CardType does not exist!");
      }
    }
  }
}
