using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  // TODO(ticktakashi): Investigate whether or not this can be used as an entry
  // point for dodging and negation. All things that happen before this effect
  // should be considered things that activate anyway regardless of dodging and
  // things that activate afterwards are part of the cards effect that can be 
  // dodged. This would become sort of a master effect.
  public class BasicCardEffect : Effect {
    protected DamageCard damage_card;

    public BasicCardEffect(Effect next, DamageCard damage_card) {
      this.damage_card = damage_card;
      this.next = next;
    }

    // We need to check if we are in range here. We may have moved.
    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      controller.PromptNegate(user, card, this, DealDamage);
      // This should never happen more than once. Even with fox fire. We are definitely done here.
      return true;
    }


    public virtual void DealDamage(Card card, IPlayer user, IGameController controller) {
      if (damage_card.InRange(user, controller))
        user.Damage(damage_card.damage);

      base.Activate(card, user, controller);
    }

    // We check if we are in range here, because this will not be reached if 
    // there is confirmation beforehand (e.g. a dash)
    public override bool CanActivate(Card card, IPlayer user, IGameController controller) {
      return damage_card.InRange(user, controller) && 
        base.CanActivate(card, user, controller);
    }

    public override bool DealsCardDamage() {
      return true;
    }
  }

  public class SkillshotEffect : BasicCardEffect {
    public SkillshotEffect(Effect a, DamageCard b) : base(a, b) { }

    public override void DealDamage(Card card, IPlayer user, IGameController controller) {
      if (damage_card.InRange(user, controller)) {
        user.Damage(damage_card.damage);
        // Fire skillshot landed event here.
      }
      base.Activate(card, user, controller);
    }
  }
}
