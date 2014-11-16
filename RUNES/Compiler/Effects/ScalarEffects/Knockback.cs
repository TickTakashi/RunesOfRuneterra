using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Knockback : ScalarEffect {

    public Knockback(int distance) {
      this.ivalue = new LiteralIntValue(distance);
    }

    public override bool Activate(Card_OLD card, IPlayer user, IGameController controller) {
      controller.KnockbackPlayer(controller.Opponent(user), value, card, user, controller, base.Activate);
      return true;
    }

    public override string ToString() {
      string extend = "";
      if (Next != null) {
        extend = Next.ToString();
      }
      return "\n<i>KNOCKBACK " + value + "</i>\n" + extend;
    }
  }
}
