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
      TargetMethods.Resolve(target, user, controller).BuffPlayer(buff_type, value);
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

  public class VariableBuff : StandardBuff {
    private IValue strength;

    public VariableBuff(Target target, int effect_id, IValue duration,
      IValue strength) : base(target, effect_id, duration) {
        this.strength = strength;
    }
    // TODO(ticktakashi): For when you wake up: You need to implement the visitor component
    // of this. meaning actionBuff. I know you want to make things pretty but you don't have
    // time! focus on getting a decent amount of cards into the game.
    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      if (card is SelfCard) {
        TargetMethods.Resolve(target, user, controller).BuffPlayer(buff_type,
          ((SelfCard)card).time, strength.GetValue());
      } else if (ivalue != null) {
        TargetMethods.Resolve(target, user, controller).BuffPlayer(buff_type,
          value, strength.GetValue());
      } else {
        throw new NotImplementedException("You must specify a duration for non-self cards!"); 
      }

      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      string ret = TargetMethods.Owner(target);
      switch (buff_type) {
        case BuffType.MELEE_DAMAGE:
          ret += " melee attacks deal " + strength.GetValue() + " additional damage";
          break;
        case BuffType.MELEE_RANGE:
          ret += " melee attacks have " + strength.GetValue() + " additional range";
          break;
        case BuffType.SHIELD:
          return base.ToString();
        default:
          throw new NotImplementedException("Reached default case for buffs.");
      }

      if (ivalue != null) {
        ret += " for the next " + ivalue + " turns.";
      }
       
      string extend = "";
      if (next != null) {
        extend = ", and " + next.ToString(); 
      }
      
      return ret + extend;
    }
  }
}
