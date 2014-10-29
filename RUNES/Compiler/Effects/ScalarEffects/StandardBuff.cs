using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class StandardBuff : ScalarEffect {
    private BuffType buff_type;

    public StandardBuff(int effect_id, int strength) {
      this.effect_id = effect_id;
      this.buff_type = (BuffType) Enum.Parse(typeof(BuffType),
         CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id), true);
      this.ivalue = new LiteralIntValue(strength);
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      user.BuffPlayer(buff_type, value);
      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      return "\n" + buff_type + " " + value + "\n" +
        base.ToString();
    }
  }
}
