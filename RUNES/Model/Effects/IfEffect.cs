using CARDScript.Compiler.Effects;
using CARDScript.Model.GameCards;
using CARDScript.Model.GameConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class IfEffect : Effect {
    GameCondition condition;
    Effect if_body;
    Effect else_body;

    internal IfEffect(GameCondition condition, Effect if_body,
      Effect else_body = null) {
        this.condition = condition;
        this.if_body = if_body;
        this.else_body = else_body;
    }

    internal override void Activate(GameCard card, Player user, Game game) {
      if (condition.Condition(user, game)) {
        if_body.Activate(card, user, game);
      } else if (else_body != null) {
        else_body.Activate(card, user, game);
      }

      base.Activate(card, user, game);
    }

    internal override bool ContainsNormalEffect() {
      return if_body.ContainsNormalEffect() || 
        (else_body != null && else_body.ContainsNormalEffect()) || 
        base.ContainsNormalEffect();
    }
  }
}
