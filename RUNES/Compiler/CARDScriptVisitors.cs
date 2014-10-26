using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Effects.ScalarEffects;
using CARDScript.Compiler.Events;
using CARDScript.Compiler.EventMatchers;
using CARDScript.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CARDScript.Model;
using Antlr4.Runtime;
using CARDScript.Model.Cards;

/* A Collection of Visitors for parsing CARDScript 
 *
 * A Set of visitors that each handle a subset of all visitor methods necessary
 * to parse effect descriptions, to use these you should first create an effect
 * listener, and then call Accept on a EffectContext parsed via a 
 * CARDScriptParser.
 * TODO(ticktakashi): This class does not implement all of the grammar.
 */
namespace CARDScript.Compiler {
  

  class CardVisitor : CARDScriptParserBaseVisitor<Card> {
    struct CardStats {
      public int damage;
      public int range;
      public int cost;
      public int limit;
    }

    EffectVisitor effect_visitor;

    public CardVisitor() {
      this.effect_visitor = new EffectVisitor();
    }

    public override Card VisitCardDash(CARDScriptParser.CardDashContext context) {
      int dash_strength = Int32.Parse(context.NUM().GetText());
      CardStats s = ParseCardAll(context.cardALL());
      string name = context.cardID().NAME().GetText();
      int id      = Int32.Parse(context.cardID().NUM().GetText());
      Effect hit  = context.cardE().Accept<Effect>(effect_visitor);
      Effect dod = context.effect().Accept<Effect>(effect_visitor);
      DashCard card = new DashCard(dash_strength, name, id, s.damage, s.range,
        s.cost, s.limit, hit, dod);
      return card;
    }

    public override Card VisitCardSpell(CARDScriptParser.CardSpellContext context) {
      CardStats s = ParseCardAll(context.cardALL());
      string name = context.cardID().NAME().GetText();
      int id = Int32.Parse(context.cardID().NUM().GetText());
      Effect effect = context.cardE().Accept<Effect>(effect_visitor);
      SpellCard card = new SpellCard(name, id, s.damage, s.range,
        s.cost, s.limit, effect);
      return card; 
    }

    public override Card VisitCardSkill(CARDScriptParser.CardSkillContext context) {
      CardStats s = ParseCardAll(context.cardALL());
      string name = context.cardID().NAME().GetText();
      int id = Int32.Parse(context.cardID().NUM().GetText());
      Effect hit = context.cardE().Accept<Effect>(effect_visitor);
      Effect dod = context.effect().Accept<Effect>(effect_visitor);
      SkillCard card = new SkillCard(name, id, s.damage, s.range, s.cost,
        s.limit, hit, dod);
      return card;
    }

    public override Card VisitCardMelee(CARDScriptParser.CardMeleeContext context) {
      CardStats s = ParseCardAll(context.cardALL());
      string name = context.cardID().NAME().GetText();
      int id = Int32.Parse(context.cardID().NUM().GetText());
      Effect effect = context.cardE().Accept<Effect>(effect_visitor);
      MeleeCard card = new MeleeCard(name, id, s.damage, s.range,
        s.cost, s.limit, effect);
      return card; 
    }

    private CardStats ParseCardAll(CARDScriptParser.CardALLContext context) {
      CardStats stats = ParseCardCL(context.cardCL());
      stats.damage = Int32.Parse(context.cardDR().NUM(0).GetText());
      stats.range = Int32.Parse(context.cardDR().NUM(1).GetText());
      return stats;
    }

    private CardStats ParseCardCL(CARDScriptParser.CardCLContext context) {
      CardStats stats = new CardStats();
      stats.cost = Int32.Parse(context.NUM(0).GetText());
      stats.limit = Int32.Parse(context.NUM(1).GetText());
      return stats;
    }

    public override Card VisitCardSelf(CARDScriptParser.CardSelfContext context) {
      return base.VisitCardSelf(context);
    }

    public override Card VisitCardPassive(CARDScriptParser.CardPassiveContext context) {
      return base.VisitCardPassive(context);
    }
  }

  public class EffectVisitor : CARDScriptParserBaseVisitor<Effect> {

    ScalarEffectVisitor scalar_effect_visitor;
    ConditionVisitor condition_visitor;
    IValueVisitor value_visitor;

    public EffectVisitor() {
      this.value_visitor = new IValueVisitor();
      this.scalar_effect_visitor = new ScalarEffectVisitor();
      this.condition_visitor = new ConditionVisitor(value_visitor,
                                                   scalar_effect_visitor);
    }

    public override Effect VisitEffect(
        CARDScriptParser.EffectContext context) {
      return context.stat().Accept<Effect>(this);
    }

    public override Effect VisitActionScalar(
        CARDScriptParser.ActionScalarContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      Target target = context.player().USER() != null ? Target.USER : Target.ENEMY;  
      ScalarEffect scalar = context.scalarEffect().Accept<ScalarEffect>(
          scalar_effect_visitor);
      scalar.target = target;
      scalar.ivalue = value;
      return scalar;
    }

    public override Effect VisitStatList(
        CARDScriptParser.StatListContext context) {
      Effect first = context.stat(0).Accept<Effect>(this);
      Effect second = context.stat(1).Accept<Effect>(this);
      first.next = second;
      return first;
    }
    
    public override Effect VisitWhen(CARDScriptParser.WhenContext context) {
      Effect triggered_effect = context.stat().Accept<Effect>(this);
      EventMatcher trigger_condition =
          context.condition().Accept<EventMatcher>(condition_visitor);
      Effect scheduled_effect;

      if (context.CHARGES() != null) {
        IValue value = context.value().Accept<IValue>(value_visitor);
        scheduled_effect = new ScheduleEffect(trigger_condition, triggered_effect, value);
      } else {
        scheduled_effect = new ScheduleEffect(trigger_condition, triggered_effect);
      }

      return scheduled_effect;
    }

    public override Effect VisitActionRepeat(
        CARDScriptParser.ActionRepeatContext context) {
      Effect action = context.action().Accept<Effect>(this);
      IValue value = context.value().Accept<IValue>(value_visitor);
      RepeatEffect repeat = new RepeatEffect(value, action);
      return repeat;
    }
  }

  class ConditionVisitor : CARDScriptParserBaseVisitor<EventMatcher> {
    public IValueVisitor value_visitor;
    public ScalarEffectVisitor scalar_effect_visitor;

    public ConditionVisitor(IValueVisitor value_visitor,
                            ScalarEffectVisitor scalar_effect_visitor) {
      this.value_visitor = value_visitor;
      this.scalar_effect_visitor = scalar_effect_visitor;
    }

    // TODO(ticktakashi): condCard (for statCard)

    public override EventMatcher VisitCondScalar(
        CARDScriptParser.CondScalarContext context) {
      Target target = context.player().USER() != null ? Target.USER : Target.ENEMY; 
      IValue value = context.value().Accept<IValue>(value_visitor);
      ScalarEffect effect = context.scalarEffect().Accept<ScalarEffect>(
          scalar_effect_visitor);
      effect.ivalue = value;
      effect.target = target;

      int op = context.ineq().start.Type;
      UnaryMatcher<IValue> condition = null;

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
          Console.WriteLine("You have no yet implemented this binop: " +
            context.ineq().GetText());
          break;
      }
     
      EventMatcher combined = new ScalarMatcher(effect, condition);

      return combined;
    }

    public override EventMatcher VisitCondExpr(
        CARDScriptParser.CondExprContext context) {
      int op = context.binopBool().start.Type;
      EventMatcher condition = null;
      EventMatcher left = context.condition(0).Accept<EventMatcher>(this);
      EventMatcher right = context.condition(0).Accept<EventMatcher>(this);

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

    public override EventMatcher VisitCondNot(
        CARDScriptParser.CondNotContext context) {
      EventMatcher cond = context.condition().Accept<EventMatcher>(this);
      return new NotMatcher(cond);
    }

    public override EventMatcher VisitCondParen(
        CARDScriptParser.CondParenContext context) {
      EventMatcher cond = context.condition().Accept<EventMatcher>(this);
      return cond;
    }
  }

  class ScalarEffectVisitor : CARDScriptParserBaseVisitor<ScalarEffect> {
    public override ScalarEffect VisitScalarEffect(
        CARDScriptParser.ScalarEffectContext context) {
      ScalarEffect effect = null;

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
        case CARDScriptParser.SHIELDS:
          // TODO(ticktakashi): Implement Shields.
          // effect = new Shields();
          goto case CARDScriptParser.TAKES;
        case CARDScriptParser.PIERCES:
          // TODO(ticktakashi): Implement Pierces.
          // effect = new Pierces();
          goto case CARDScriptParser.TAKES;
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