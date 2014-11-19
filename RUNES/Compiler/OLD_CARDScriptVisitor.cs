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