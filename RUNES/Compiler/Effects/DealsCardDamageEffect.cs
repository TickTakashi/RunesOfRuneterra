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
  public class DealsCardDamageEffect : Effect {
    protected DamageCard damage_card;

    public DealsCardDamageEffect(Effect next, DamageCard damage_card) {
      this.damage_card = damage_card;
      this.next = next;
    }

    // We need to check if we are in range here. We may have moved.
    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      bool negated = controller.PromptNegate(user, card, this);
      if (negated) {
        return true;
      }

      DealDamage(user, controller);

      return base.Activate(card, user, controller);
    }

    public virtual void DealDamage(IPlayer user, IGameController controller) {
      if (damage_card.InRange(user, controller)) {
        user.Damage(damage_card.damage);
      }
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

  public class SkillshotEffect : DealsCardDamageEffect {
    public SkillshotEffect(Effect a, DamageCard b) : base(a, b) { }

    public override void DealDamage(IPlayer user, IGameController controller) {
      if (damage_card.InRange(user, controller)) {
        user.Damage(damage_card.damage);
        // Fire skillshot landed event here.
      }
    }
  }
}
