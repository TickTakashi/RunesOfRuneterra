using CARDScript.Compiler.Matchers;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  public class ConditionalEffect : Effect {
    Matcher condition;
    Effect if_body;
    Effect else_body;

    public ConditionalEffect(Matcher condition, Effect if_body,
        Effect else_body = null) {
      this.condition = condition;
      this.if_body = if_body;
      this.else_body = else_body;
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      // Think about the return value here.
      if (condition.Match(null, controller, user)) {
        if_body.Activate(card, user, controller);
      } else {
        if (else_body != null) {
          else_body.Activate(card, user, controller);
        }
      }
      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      string elseif = "";
      
      if (else_body != null) {
        elseif = " otherwise " + else_body;
      }

      return "if " + condition + ": " + if_body + else_body + base.ToString();
    }
  }
}
