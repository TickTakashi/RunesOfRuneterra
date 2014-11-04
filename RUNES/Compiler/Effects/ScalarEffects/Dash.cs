using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Dash : ScalarEffect {

    public Dash(int distance) {
      this.ivalue = new LiteralIntValue(distance);
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      controller.PromptMove(user, card, value, base.Activate);
      return true;
    }

    // We can always dash if we know that we are not CCd.
    public override bool CanActivate(Card card, IPlayer user, IGameController controller) {
      return !user.HasBuff(BuffType.SNARE) && !user.HasBuff(BuffType.STUN) && 
        !user.HasBuff(BuffType.SILENCE);
    }

    public override bool CanNegate(Effect effect) {
      return (effect is CardEffect && ((CardEffect)effect).Dashable()) ||
        base.CanNegate(effect);     
    }

    public override string ToString() {
      return "\n<i>DASH " + value + "</i>\n" + base.ToString();
    }
  }
}
