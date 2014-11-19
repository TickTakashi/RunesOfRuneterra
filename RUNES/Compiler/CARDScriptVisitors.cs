using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Effects.ScalarEffects;
using CARDScript.Model;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs.StatBonuses;
using CARDScript.Model.Cards;
using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.Effects;
using CARDScript.Model.Effects.CardEffects;
using CARDScript.Model.Effects.ScalarEffects;
using System;
namespace CARDScript.Compiler {
  class PassiveCardVisitor : CARDScriptParserBaseVisitor<PassiveCard> {
    BuffVisitor buff_visitor;

    public PassiveCardVisitor() {
      this.buff_visitor = new BuffVisitor();
    }

    public override PassiveCard VisitPassiveDesc(
      CARDScriptParser.PassiveDescContext context) {
      Buff buff = context.cardB().Accept<Buff>(buff_visitor);
      int id = Int32.Parse(context.NUM().GetText());
      string name = context.NAME().GetText();
      return new PassiveCard(name, id, buff);
    }
  }

  class GameCardVisitor : CARDScriptParserBaseVisitor<GameCard> {
    EffectVisitor effect_visitor;
    BuffVisitor buff_visitor;

    public GameCardVisitor() {
      this.effect_visitor = new EffectVisitor();
      this.buff_visitor = new BuffVisitor();
    }

    private void ParseCardID(CardBuilder builder, 
      CARDScriptParser.CardIDContext context) {
        builder = builder.WithName(context.NAME().GetText())
          .WithID(Int32.Parse(context.NUM(0).GetText()))
          .WithCost(Int32.Parse(context.NUM(1).GetText()))
          .WithLimit(Int32.Parse(context.NUM(2).GetText()));

        if (context.DASH() != null) {
          builder.WithDash(Int32.Parse(context.NUM(3).GetText()));
        }

        if (context.ULTIMATE() != null) {
          builder.WithUlt();
        }
    }

    public override GameCard VisitCardSpell(
      CARDScriptParser.CardSpellContext context) {
      SpellCardBuilder builder = new SpellCardBuilder();
      builder.WithDamage(Int32.Parse(context.NUM(0).GetText()))
        .WithRange(Int32.Parse(context.NUM(1).GetText()))
        .WithEffect(context.cardE().Accept<Effect>(effect_visitor));
      ParseCardID(builder, context.cardID());
      return builder.Build();
    }

    public override GameCard VisitCardSkill(
      CARDScriptParser.CardSkillContext context) {
      SkillCardBuilder builder = new SkillCardBuilder();
      builder.WithDamage(Int32.Parse(context.NUM(0).GetText()))
        .WithRange(Int32.Parse(context.NUM(1).GetText()))
        .WithEffect(context.cardE().Accept<Effect>(effect_visitor));
      ParseCardID(builder, context.cardID());
      return builder.Build(); ;
    }

    public override GameCard VisitCardMelee(
      CARDScriptParser.CardMeleeContext context) {
      MeleeCardBuilder builder = new MeleeCardBuilder();
      builder.WithDamage(Int32.Parse(context.NUM().GetText()))
        .WithEffect(context.cardE().Accept<Effect>(effect_visitor));
      ParseCardID(builder, context.cardID());
      return builder.Build();
    }
    
    public override GameCard VisitCardSelf(
      CARDScriptParser.CardSelfContext context) {
      BuffCardBuilder builder = new BuffCardBuilder();
      builder.WithTime(Int32.Parse(context.NUM().GetText()))
        .WithBuff(context.cardB().Accept<Buff>(buff_visitor))
        .WithEffect(context.cardE().Accept<Effect>(effect_visitor));
      ParseCardID(builder, context.cardID());
      return builder.Build();
    }
  }

  public class BuffVisitor : CARDScriptParserBaseVisitor<Buff> {
    IValueVisitor value_visitor;

    public BuffVisitor() {
      this.value_visitor = new IValueVisitor();
    }

    public override Buff VisitBuffEffect(
      CARDScriptParser.BuffEffectContext context) {
      if (context.statB() != null)
        return context.statB().Accept<Buff>(this);
      else
        return null;
    }

    public override Buff VisitStatBFlat(
      CARDScriptParser.StatBFlatContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      switch (context.bonusB().Start.TokenIndex) {
        case(CARDScriptParser.MELEE_D):
          return new MeleeDamageBuff(value);
        case(CARDScriptParser.MELEE_R):
          return new MeleeRangeBuff(value);
        case(CARDScriptParser.SKILL_D):
          return new SkillDamageBuff(value);
        default:
          throw new RoRException(
            "COMPILER: This bonusB is not yet implemented!");
      }
    }
  }

  public class EffectVisitor : CARDScriptParserBaseVisitor<Effect> {
    IValueVisitor value_visitor;
    CardConditionVisitor card_condition_visitor;

    public EffectVisitor() {
      this.value_visitor = new IValueVisitor();
      this.card_condition_visitor = new CardConditionVisitor();
    }

    public static Target ParseTarget(
      CARDScriptParser.PlayerContext context) {
      return context.USER() != null ? Target.USER : Target.ENEMY; 
    }

    public static Location ParseLocation(
      CARDScriptParser.LocationContext context) {
      if (context.COOL() != null)
        return Location.COOL;
      if (context.DECK() != null)
        return Location.DECK;
      if (context.HAND() != null)
        return Location.HAND;
      else
        throw new RoRException("COMPILER: Location doesn't exist.");
    }

    public override Effect VisitEffect(
        CARDScriptParser.EffectContext context) {
          if (context.statE() != null)
            return context.statE().Accept<Effect>(this);
          else
            return new NullEffect();
    }

    public override Effect VisitStatENormal(
      CARDScriptParser.StatENormalContext context) {
      switch (context.cardType().Start.TokenIndex) {
        case(CARDScriptParser.SKILL):
          return new SkillshotEffect();
        case(CARDScriptParser.SPELL):
          return new DamageEffect();
        case(CARDScriptParser.DAMAGE):
          return new DamageEffect();
        case(CARDScriptParser.MELEE):
          return new MeleeEffect();
        case(CARDScriptParser.SELF):
          return new BuffEffect();
        default:
          throw new RoRException("COMPILATION: Card Type Expected.");
      }
    }

    public override Effect VisitActionCC(
      CARDScriptParser.ActionCCContext context) {
      string cc_name = context.ccEffect().GetText();
      CCType cc = (CCType)Enum.Parse(typeof(CCType), cc_name, true);
      return new CCEffect(cc, Int32.Parse(context.NUM().GetText()));
    }

    public override Effect VisitActionShield(
      CARDScriptParser.ActionShieldContext context) {
      return new ShieldEffect(Int32.Parse(context.NUM().GetText()));
    }

    public override Effect VisitActionKnockup(
      CARDScriptParser.ActionKnockupContext context) {
      return new KnockupEffect(Int32.Parse(context.NUM().GetText()));
    }

    public override Effect VisitActionKnockback(
      CARDScriptParser.ActionKnockbackContext context) {
      return new KnockbackEffect(Int32.Parse(context.NUM().GetText()));
    }

    public override Effect VisitActionScalar(
      CARDScriptParser.ActionScalarContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      Target target = ParseTarget(context.player());
      
      switch (context.scalarE().Start.TokenIndex) {
        case (CARDScriptParser.DRAWS):
          return new Draw(target, value);
        case (CARDScriptParser.TAKES):
          return new Damage(target, value);
        case (CARDScriptParser.HEALS):
          return new Heal(target, value);
        default:
          throw new RoRException(
            "COMPILER: This scalarE is not yet implemented!");
      }
    }

    public override Effect VisitActionSearch(
      CARDScriptParser.ActionSearchContext context) {
      Target choice_maker = ParseTarget(context.player(0));
      Target debit_player = ParseTarget(context.player(1));
      Target credit_player = ParseTarget(context.player(2));
      IValue value = context.value().Accept<IValue>(value_visitor);
      Location debit_location = ParseLocation(context.location(0));
      Location credit_location = ParseLocation(context.location(1));
      CardCondition condition = context.cardCond().Accept<CardCondition>(
        card_condition_visitor);
      bool is_optional = context.MAY() != null;
      return new CardMoveEffect(choice_maker, value, debit_player, 
        debit_location, credit_player, credit_location, condition, 
        is_optional);
    }
  }
    
  class IValueVisitor : CARDScriptParserBaseVisitor<IValue> {
    public override IValue VisitValueInt(
      CARDScriptParser.ValueIntContext context) {
      int value = Int32.Parse(context.NUM().GetText());
      return new LiteralIntValue(value);
    }

    public override IValue VisitValueRandom(
      CARDScriptParser.ValueRandomContext context) {
      IValue l = context.value(0).Accept<IValue>(this);
      IValue r = context.value(1).Accept<IValue>(this);
      return new RandomValue(l, r);
    }

    public override IValue VisitValueDouble(
      CARDScriptParser.ValueDoubleContext context) {
        IValue r = context.value().Accept<IValue>(this);
        return new DoubleValue(r);
    }

    public override IValue VisitValueHalf(
      CARDScriptParser.ValueHalfContext context) {
        IValue r = context.value().Accept<IValue>(this);
        return new HalfValue(r);
    }

    public override IValue VisitValueDistance(
      CARDScriptParser.ValueDistanceContext context) {
        return new DistanceValue(); 
    }
  }

  class CardConditionVisitor : CARDScriptParserBaseVisitor<CardCondition> {
    public override CardCondition VisitCardCondName(
      CARDScriptParser.CardCondNameContext context) {
        return new NameCondition(context.NAME().GetText());
    }

    public override CardCondition VisitCardCondType(
      CARDScriptParser.CardCondTypeContext context) {
        string type_name = context.cardType().GetText();
        CardType ct = (CardType)Enum.Parse(typeof(CardType), type_name, true);
        return new TypeCondition(ct);
    }
  }
    /*
    public override Effect VisitStatList(
        CARDScriptParser.StatListContext context) {
      Effect first = context.stat(0).Accept<Effect>(this);
      Effect second = context.stat(1).Accept<Effect>(this);
      first.GetLastEffect().Next = second;
      return first;
    }
    
    public override Effect VisitWhen(CARDScriptParser.WhenContext context) {
      Effect triggered_effect = context.stat().Accept<Effect>(this);
      Matcher trigger_condition =
          context.eventCond().Accept<Matcher>(matcher_visitor);
      Effect scheduled_effect;

      if (context.CHARGES() != null) {
        IValue value = context.value().Accept<IValue>(value_visitor);
        scheduled_effect = new ScheduleEffect(trigger_condition, triggered_effect, value);
      } else {
        scheduled_effect = new ScheduleEffect(trigger_condition, triggered_effect);
      }

      return scheduled_effect;
    }

    public override Effect VisitIf(CARDScriptParser.IfContext context) {
      Effect ifthen = context.stat(0).Accept<Effect>(this);
      Effect ifelse = context.ELSE() != null ? context.stat(1).Accept<Effect>(this) : null;
      Matcher matcher = context.stateCond().Accept<Matcher>(matcher_visitor);
      ConditionalEffect cond = new ConditionalEffect(matcher, ifthen, ifelse);
      return cond;
    }

    public override Effect VisitActionRepeat(
        CARDScriptParser.ActionRepeatContext context) {
      Effect action = context.action().Accept<Effect>(this);
      IValue value = context.value().Accept<IValue>(value_visitor);
      RepeatEffect repeat = new RepeatEffect(value, action);
      return repeat;
    }
  }

  class MatcherVisitor : CARDScriptParserBaseVisitor<Matcher> {
    public IValueVisitor value_visitor;
    public ScalarEffectVisitor scalar_effect_visitor;

    public MatcherVisitor(IValueVisitor value_visitor,
                            ScalarEffectVisitor scalar_effect_visitor) {
      this.value_visitor = value_visitor;
      this.scalar_effect_visitor = scalar_effect_visitor;
    }

    public override Matcher VisitStateCondHealth(
        CARDScriptParser.StateCondHealthContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      InequalityMatcher im = ParseInequality(context.ineq(), value); 
      Target t = ParseTarget(context.player());
      return new HealthMatcher(t, im);    
    }

    public override Matcher VisitStateCondDistance(
        CARDScriptParser.StateCondDistanceContext context) {
          IValue value = context.value().Accept<IValue>(value_visitor);
          InequalityMatcher im = ParseInequality(context.ineq(), value);
      return new DistanceMatcher(im);
    }

    public override Matcher VisitEventCondScalar(
        CARDScriptParser.EventCondScalarContext context) {
      Target target = ParseTarget(context.player()); 
      IValue value = context.value().Accept<IValue>(value_visitor);
      TargetedScalarEffect effect = context.scalarEffect().Accept<TargetedScalarEffect>(
          scalar_effect_visitor);
      effect.ivalue = value;
      effect.target = target;

      InequalityMatcher condition = ParseInequality(context.ineq(), value);
    
      Matcher combined = new ScalarMatcher(effect, condition);

      return combined;
    }

    public override Matcher VisitEventCondExpr(
        CARDScriptParser.EventCondExprContext context) {
      int op = context.binopBool().start.Type;
      Matcher condition = null;
      Matcher left = context.eventCond(0).Accept<Matcher>(this);
      Matcher right = context.eventCond(0).Accept<Matcher>(this);

      switch (op) {
        case CARDScriptParser.OR:
          condition = new OrMatcher(left, right);
          break;
        case CARDScriptParser.AND:
          condition = new AndMatcher(left, right);
          break;
        default:
          Console.WriteLine("You have no yet implemented this binop: " +
            context.binopBool().GetText());
          break;
      }
      
      return condition;
    }

    public override Matcher VisitEventCondNot(
        CARDScriptParser.EventCondNotContext context) {
      Matcher cond = context.eventCond().Accept<Matcher>(this);
      return new NotMatcher(cond);
    }

    public override Matcher VisitEventCondParen(
        CARDScriptParser.EventCondParenContext context) {
      Matcher cond = context.eventCond().Accept<Matcher>(this);
      return cond;
    }

    public static InequalityMatcher ParseInequality(
        CARDScriptParser.IneqContext context, IValue value) {
      int op = context.start.Type;
      InequalityMatcher condition = null;

      switch (op) {
        case CARDScriptParser.GT:
          condition = new GTMatcher(value);
          break;
        case CARDScriptParser.LT:
          condition = new LTMatcher(value);
          break;
        case CARDScriptParser.EQ:
          condition = new EQMatcher(value);
          break;
        default:
          Console.WriteLine("You have not yet implemented this binop: " +
            context.GetText());
          break;
      }

      return condition;
    }

  }


  class ScalarEffectVisitor : CARDScriptParserBaseVisitor<TargetedScalarEffect> {
    public override TargetedScalarEffect VisitScalarEffect(
        CARDScriptParser.ScalarEffectContext context) {
      TargetedScalarEffect effect = null;

      int effectType = context.start.Type;
      switch (effectType) {
        case CARDScriptParser.DRAWS:
          effect = new Draw();
          break;
        case CARDScriptParser.HEALS:
          effect = new Heal();
          break;
        case CARDScriptParser.TAKES:
          effect = new Damage();
          break;
        default:
          Console.WriteLine("Could not match Scalar Effect. Index was: " +
                            effectType + "Defaulting to TAKES");
          goto case CARDScriptParser.TAKES;
      }

      effect.effect_id = effectType;
      return effect;
    }
  }*/


}