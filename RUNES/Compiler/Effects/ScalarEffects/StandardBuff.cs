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
      string buff_name = CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id);
      buff_name = buff_name.Substring(1, buff_name.Length - 2);
      this.buff_type = (BuffType) Enum.Parse(typeof(BuffType), buff_name, true);
      this.ivalue = new LiteralIntValue(strength);
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      user.BuffPlayer(buff_type, value);
      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      return "\n<i>" + buff_type + "</i> " + value + "\n" +
        base.ToString();
    }
  }
}
