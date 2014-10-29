﻿using CARDScript.Compiler.Effects;
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
    EventConditionVisitor event_condition_visitor;
    IValueVisitor value_visitor;

    public EffectVisitor() {
      this.value_visitor = new IValueVisitor();
      this.scalar_effect_visitor = new ScalarEffectVisitor();
      this.event_condition_visitor = new EventConditionVisitor(value_visitor,
                                                   scalar_effect_visitor);
    }

    public override Effect VisitEffect(
        CARDScriptParser.EffectContext context) {
          if (context.stat() != null)
            return context.stat().Accept<Effect>(this);
          else
            return new NullEffect();
    }

    public override Effect VisitActionScalar(
        CARDScriptParser.ActionScalarContext context) {
      IValue value = context.value().Accept<IValue>(value_visitor);
      Target target = context.player().USER() != null ? Target.USER : Target.ENEMY;  
      TargetedScalarEffect scalar = context.scalarEffect().Accept<TargetedScalarEffect>(
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
      Matcher<GameEvent> trigger_condition =
          context.eventCond().Accept<Matcher<GameEvent>>(event_condition_visitor);
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
      //StateMatcher 
      return base.VisitIf(context);
    }

    public override Effect VisitActionRepeat(
        CARDScriptParser.ActionRepeatContext context) {
      Effect action = context.action().Accept<Effect>(this);
      IValue value = context.value().Accept<IValue>(value_visitor);
      RepeatEffect repeat = new RepeatEffect(value, action);
      return repeat;
    }
  }

  class EventConditionVisitor : CARDScriptParserBaseVisitor<Matcher<GameEvent>> {
    public IValueVisitor value_visitor;
    public ScalarEffectVisitor scalar_effect_visitor;

    public EventConditionVisitor(IValueVisitor value_visitor,
                            ScalarEffectVisitor scalar_effect_visitor) {
      this.value_visitor = value_visitor;
      this.scalar_effect_visitor = scalar_effect_visitor;
    }

    // TODO(ticktakashi): condCard (for statCard)

    public override Matcher<GameEvent> VisitEventCondScalar(
        CARDScriptParser.EventCondScalarContext context) {
      Target target = context.player().USER() != null ? Target.USER : Target.ENEMY; 
      IValue value = context.value().Accept<IValue>(value_visitor);
      TargetedScalarEffect effect = context.scalarEffect().Accept<TargetedScalarEffect>(
          scalar_effect_visitor);
      effect.ivalue = value;
      effect.target = target;

      int op = context.ineq().start.Type;
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
            context.ineq().GetText());
          break;
      }
     
      Matcher<GameEvent> combined = new ScalarMatcher(effect, condition);

      return combined;
    }

    public override Matcher<GameEvent> VisitEventCondExpr(
        CARDScriptParser.EventCondExprContext context) {
      int op = context.binopBool().start.Type;
      Matcher<GameEvent> condition = null;
      Matcher<GameEvent> left = context.eventCond(0).Accept<Matcher<GameEvent>>(this);
      Matcher<GameEvent> right = context.eventCond(0).Accept<Matcher<GameEvent>>(this);

      switch (op) {
        case CARDScriptParser.OR:
          condition = new OrMatcher<GameEvent>(left, right);
          break;
        case CARDScriptParser.AND:
          condition = new AndMatcher<GameEvent>(left, right);
          break;
        default:
          Console.WriteLine("You have no yet implemented this binop: " +
            context.binopBool().GetText());
          break;
      }
      
      return condition;
    }

    public override Matcher<GameEvent> VisitEventCondNot(
        CARDScriptParser.EventCondNotContext context) {
      Matcher<GameEvent> cond = context.eventCond().Accept<Matcher<GameEvent>>(this);
      return new NotMatcher<GameEvent>(cond);
    }

    public override Matcher<GameEvent> VisitEventCondParen(
        CARDScriptParser.EventCondParenContext context) {
      Matcher<GameEvent> cond = context.eventCond().Accept<Matcher<GameEvent>>(this);
      return cond;
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