using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  // This class is an important entry point for dodging. If you are confused,
  // read the commit logs. Also, try and remember that this effect deals the
  // "normal" damage of the card. You don't need to deal damage again anywhere
  // else, and all damage now happens through effects.
  public class CardEffect : Effect {
    DamageCard damage_card;

    public CardEffect(Effect next, DamageCard damage_card) {
      this.damage_card = damage_card;
      this.next = next;
    }

    public CardEffect(DamageCard damage_card) : this(null, damage_card) {}

    public CardEffect() : this(null, null) {}

    // We need to check if we are in range here. We may have moved.
    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      controller.PromptNegate(user, card, this, DealDamage);
      // This should never happen more than once. Even with fox fire. We are definitely done here.
      return true;
    }

    public virtual bool DealDamage(Card card, IPlayer user, IGameController controller) {
      if (damage_card == null) {
        damage_card = (DamageCard)card;
      }

      if (damage_card != null) {
        if (damage_card.InRange(user, controller))
          user.Damage(damage_card.damage);

        if (damage_card is SkillCard)
          ; // Fire Skillshot hit event
        else if (damage_card is MeleeCard)
          ; // Fire On hit event.
      }
      return base.Activate(card, user, controller);
    }

    // We check if we are in range here, because this will not be reached if 
    // there is confirmation beforehand (e.g. a dash)
    public override bool CanActivate(Card card, IPlayer user, 
      IGameController controller) {
      return damage_card.InRange(user, controller) &&
        base.CanActivate(card, user, controller);
    }

    public override bool DealsCardDamage() {
      return true;
    }

    public bool Dashable() {
      return damage_card != null && damage_card is SkillCard;
    }
  }
}