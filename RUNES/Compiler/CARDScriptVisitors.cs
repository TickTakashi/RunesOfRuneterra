using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Effects.TargetedScalarEffects;
using CARDScript.Compiler.Events;
using CARDScript.Compiler.Matchers;
using CARDScript.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CARDScript.Model;
using Antlr4.Runtime;
using CARDScript.Model.Cards;
using CARDScript.Compiler.Effects.ScalarEffects;

/* A Collection of Visitors for parsing CARDScript 
 *
 * A Set of visitors that each handle a subset of all visitor methods necessary
 * to parse effect descriptions, to use these you should first create an effect
 * listener, and then call Accept on a EffectContext parsed via a 
 * CARDScriptParser.
 * TODO(ticktakashi): This class does not implement all of the grammar.
 */
namespace CARDScript.Compiler {

  class PassiveVisitor : CARDScriptParserBaseVisitor<Passive> {

  }

  class CardVisitor : CARDScriptParserBaseVisitor<Card> {
    struct CardInfo {
      public string name;
      public int id;
      public int cost;
      public int limit;
    }

    EffectVisitor effect_visitor;

    public CardVisitor() {
      this.effect_visitor = new EffectVisitor();
    }

    public override Card VisitCardSpell(CARDScriptParser.CardSpellContext context) {
      CardInfo s = ParseCardID(context.cardID());
      int damage = Int32.Parse(context.NUM(0).GetText());
      int range = Int32.Parse(context.NUM(1).GetText());
      Effect effect = context.cardE().Accept<Effect>(effect_visitor); 
      SpellCard card = new SpellCard(s.name, s.id, damage, range,
        s.cost, s.limit, effect);
      return card; 
    }

    public override Card VisitCardSkill(CARDScriptParser.CardSkillContext context) {
      CardInfo s = ParseCardID(context.cardID());
      int damage = Int32.Parse(context.NUM(0).GetText());
      int range  = Int32.Parse(context.NUM(1).GetText());
      Effect effect = context.cardE().Accept<Effect>(effect_visitor);
      SkillCard card = new SkillCard(s.name, s.id, damage, range, s.cost,
        s.limit, effect);
      return card;
    }

    public override Card VisitCardMelee(CARDScriptParser.CardMeleeContext context) {
      CardInfo s = ParseCardID(context.cardID());
      int damage = Int32.Parse(context.NUM().GetText());
      Effect effect = context.cardE().effect().Accept<Effect>(effect_visitor);
      MeleeCard card = new MeleeCard(s.name, s.id, damage,
        s.cost, s.limit, effect);
      return card; 
    }
    
    public override Card VisitCardSelf(CARDScriptParser.CardSelfContext context) {
      CardInfo s = ParseCardID(context.cardID());
      int time = Int32.Parse(context.NUM().GetText());
      Effect effect = context.cardE().Accept<Effect>(effect_visitor);
      SelfCard card = new SelfCard(s.name, s.id, s.cost, s.limit, time, effect);
      return card;
    }

    private CardInfo ParseCardID(CARDScriptParser.CardIDContext context) {
      CardInfo stats = new CardInfo();
      stats.name = context.NAME().GetText();
      stats.id = Int32.Parse(context.NUM(0).GetText());
      stats.cost = Int32.Parse(context.NUM(1).GetText());
      stats.limit = Int32.Parse(context.NUM(2).GetText());
      return stats;
    }
  }

  public class EffectVisitor : CARDScriptParserBaseVisitor<Effect> {

    ScalarEffectVisitor scalar_effect_visitor;
    MatcherVisitor matcher_visitor;
    IValueVisitor value_visitor;

    public EffectVisitor() {
      this.value_visitor = new IValueVisitor();
      this.scalar_effect_visitor = new ScalarEffectVisitor();
      this.matcher_visitor = new MatcherVisitor(value_visitor,
                                                   scalar_effect_visitor);
    
    }

    public override Effect VisitEffect(
        CARDScriptParser.EffectContext context) {
          if (context.stat() != null)
            return context.stat().Accept<Effect>(this);
          else
            return new NullEffect();
    }

    public override Effect VisitStatDamage(CARDScriptParser.StatDamageContext context) {
      return new CardEffect(); // I am relying on the right card being passed to this on activation.
    }

    public override Effect VisitActionDash(CARDScriptParser.ActionDashContext context) {
      int distance = Int32.Parse(context.NUM().GetText());
      return new Dash(distance);
    }

    public override Effect VisitActionCC(CARDScriptParser.ActionCCContext context) {

      return new StandardBuff(Target.ENEMY, context.ccEffect().Start.Type, 
        Int32.Parse(context.NUM().GetText()));
    }

    public override Effect VisitActionBuff(CARDScriptParser.ActionBuffContext context) {
      IValue strength = context.value(0).Accept<IValue>(value_visitor);
      IValue duration = null;
      
      if (context.value(1) != null) {
        duration = context.value(1).Accept<IValue>(value_visitor);
      }

      return new VariableBuff(MatcherVisitor.ParseTarget(context.player()),
        context.buff().Start.Type, duration, strength);
    }

    public override Effect VisitActionScalar(
        CARDScriptParser.ActionScalarContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      Target target = MatcherVisitor.ParseTarget(context.player());  
      TargetedScalarEffect scalar = context.scalarEffect().Accept<TargetedScalarEffect>(
          scalar_effect_visitor);
      scalar.target = target;
      scalar.ivalue = value;
      return scalar;
    }

    public override Effect VisitActionSearch(CARDScriptParser.ActionSearchContext context) {
      Target player = MatcherVisitor.ParseTarget(context.player());
      IValue value = context.value().Accept<IValue>(value_visitor);
      int destination = context.location().Start.Type;
      return new CardAdder(player, value, context.NAME().GetText(), destination);
    }

    public override Effect VisitStatList(
        CARDScriptParser.StatListContext context) {
      Effect first = context.stat(0).Accept<Effect>(this);
      Effect second = context.stat(1).Accept<Effect>(this);
      first.GetLastEffect().next = second;
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

    public static Target ParseTarget(CARDScriptParser.PlayerContext context) {
      return context.USER() != null ? Target.USER : Target.ENEMY; 
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
  }

  class IValueVisitor : CARDScriptParserBaseVisitor<IValue> {
    public override IValue VisitValueInt(CARDScriptParser.ValueIntContext context) {
      // TODO(ticktakashi): Add new value types here.
      int value = Int32.Parse(context.NUM().GetText());
      return new LiteralIntValue(value);
    }

    public override IValue VisitValueRandom(CARDScriptParser.ValueRandomContext context) {
      IValue l = context.value(0).Accept<IValue>(this);
      IValue r = context.value(1).Accept<IValue>(this);
      return new RandomValue(l, r);
    }
  }
}