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

    public override bool Activate(Card_OLD card, IPlayer user, IGameController controller) {
      user.PromptMove(card, value, base.Activate);
      return true;
    }

    // We can always dash if we know that we are not CCd.
    public override bool CanActivate(Card_OLD card, IPlayer user, IGameController controller) {
      return !user.HasBuff(BuffType.SNARE) && !user.HasBuff(BuffType.STUN) && 
        !user.HasBuff(BuffType.SILENCE);
    }

    public override bool CanNegate(Card_OLD card, IPlayer user, 
      IGameController controller, NormalEffect effect) {
      return effect.Dashable() && CanActivate(card, user, controller) ||
        base.CanNegate(card, user, controller, effect);     
    }

    public override string ToString() {
      string extend = "";
      if (Next != null) {
        extend = Next.ToString();
      }
      return "\n<i>DASH " + value + "</i>\n" + extend;
    }
  }
}
