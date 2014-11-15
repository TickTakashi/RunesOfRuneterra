using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class StandardBuff : ScalarEffect {
    protected BuffType buff_type;
    protected Target target;

    public StandardBuff(Target target, int effect_id, int duration) :
      this(target, effect_id, new LiteralIntValue(duration)) { }

    public StandardBuff(Target target, int effect_id, IValue duration) {
      this.target = target;
      this.effect_id = effect_id;
      string buff_name = CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id);
      buff_name = buff_name.Substring(1, buff_name.Length - 2);
      this.buff_type = (BuffType) Enum.Parse(typeof(BuffType), buff_name, true);
      this.ivalue = duration;
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      int turn_offset = 0;
      if (target == Target.ENEMY)
        turn_offset++;
      TargetMethods.Resolve(target, user, controller).BuffPlayer(user, buff_type, value + turn_offset);
      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      string extend = "";
      if (next != null) {
        extend = next.ToString();
      } 
      return "\n<i>" + buff_type + " " + value + "</i>\n" + extend;
    }
  }

  public class VariableBuff : ScalarEffect {
    protected BuffType buff_type;
    protected Target target;
    private IValue strength;

    public VariableBuff(Target target, int effect_id, IValue duration,
      IValue strength) {
      this.target = target;
      this.effect_id = effect_id;
      string buff_name = CARDScriptParser.DefaultVocabulary.GetLiteralName(effect_id);
      buff_name = buff_name.Substring(1, buff_name.Length - 2);
      this.buff_type = (BuffType)Enum.Parse(typeof(BuffType), buff_name, true);
      this.ivalue = duration;
      this.strength = strength;
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      if (card is SelfCard) {
        TargetMethods.Resolve(target, user, controller).BuffPlayer(user, buff_type,
          ((SelfCard)card).time, strength.GetValue());
      } else {
        TargetMethods.Resolve(target, user, controller).BuffPlayer(user, buff_type,
          1, strength.GetValue());
      }

      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      string extend = "";
      if (next != null) {
        extend = next.ToString();
      }
     
      string ret = TargetMethods.Owner(target);
      switch (buff_type) {
        case BuffType.MELEE_DAMAGE:
          ret += " melee attacks deal " + strength.GetValue() + " additional damage";
          break;
        case BuffType.MELEE_RANGE:
          ret += " melee attacks have " + strength.GetValue() + " additional range";
          break;
        case BuffType.SKILL_DAMAGE:
          ret += " skillshots deal " + strength.GetValue() + " additional damage";
          break;
        case BuffType.SHIELD:
          return "\n<i>SHIELD " + strength.GetValue() + "</i>\n" + extend;
        case BuffType.KNOCKUP:
          return "\n<i>KNOCKUP " + strength.GetValue() + "</i>\n" + extend;
        default:
          throw new NotImplementedException("Reached default case for buffs.");
      }

      if (ivalue != null) {
        ret += " for the next " + ivalue + " turns.";
      }

      if (next != null) {
        extend = " and " + extend;
      }
      return ret + extend;
    }
  }
}
