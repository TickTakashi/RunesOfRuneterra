using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The SkillCard Class
   * 
   * Represents a Skillshot in league of legends. These skills are similar to
   * SpellCards except for the fact that they can be dodged. They also have an
   * additional activation hook, as something may happen only when they are
   * dodged.
   */
  // TODO(ticktakashi): Implement SkillCard.
  public class SkillCard : SpellCard {
    // TODO(ticktakashi): Think about adding pre and post effects.
    public Effect dodge_effect;

    public SkillCard(string name, int id, int damage, int range, int cost,
        int limit, Effect effect, Effect dodge_effect) : base(name, id, damage,
        range, cost, limit, effect) {
      this.dodge_effect = dodge_effect;
    }

    public override bool CanActivate(IPlayer user, IGameController game_controller) {
      return base.CanActivate(user, game_controller);
    }

    public override void Activate(IPlayer user, IGameController game_controller) {
      // TODO(ticktakashi): Implement Skillshot & Dodging semantics.
      // TODO(ticktakashi): Reconsider how you implement skillshots and dodging,
      //                    You are going to need to implement similar semantics
      //                    for the special case of Spellshields.
      //                    This will probably relate to the semantics for conditional
      //                    activation. Add a listener for skillshots when the dodge
      //                    is added to the hand, and then check through them when
      //                    activating a skillshot, prompting to dodge and returning
      //                    some negation notice if they do so. Derailing the Activate()
      //                    call. "If your opponent plays a Skillshot" <- this is the same as
      //                    a dash and the listener will go wherever this effect would go.

      // 1 - Consume Action Points
      user.Spend(cost);

      // 2 - Prompt Dodge.
      bool dodged = game_controller.PromptNegate(user, this);

      // 3 - If they dodged, dodge_effect, else damage & effect.
      if (dodged) {
        dodge_effect.Activate(this, user, game_controller);
      } else {
        effect.Activate(this, user, game_controller);
        game_controller.Opponent(user).Damage(damage);
      }
    }
  }
}
