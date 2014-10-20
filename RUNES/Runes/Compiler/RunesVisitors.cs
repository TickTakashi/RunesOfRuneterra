using RUNES.Runes.Model;
using RUNES.Runes.Model.Effects;
using RUNES.Runes.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* A Collection of Visitors for parsing RUNES 
 *
 * A Set of visitors that each handle a subset of all visitor methods necessary to parse
 * effect descriptions, to use these you should first create an effect listener, and then
 * call Accept on a EffectContext parsed via a RunesParser.
 * 
 * TODO(ticktakashi): At present this class does not implement all of the grammar.
 */
namespace RUNES.Runes.Compiler {
  class EffectVisitor : RunesParserBaseVisitor<Effect> {

    ScalarEffectVisitor scalar_effect_visitor;
    ConditionVisitor condition_visitor;
    PlayerVisitor player_visitor;
    IntValueVisitor value_visitor;
    Player user;
    Card source;

    public EffectVisitor(Player user, Player opponent, Card source) {
      this.source = source;
      this.player_visitor = new PlayerVisitor(user, opponent);
      this.user = user;
      this.value_visitor = new IntValueVisitor();
      this.scalar_effect_visitor = new ScalarEffectVisitor();
      this.condition_visitor = new ConditionVisitor(value_visitor);
    }

    public override Effect VisitEffect(RunesParser.EffectContext context) {
      if (context.preCond() != null) {
        // TODO(ticktakashi): Implement conditional activation.
        // context.preCond().Accept<Precondition>(precond_visitor); // or something.
      }
      Effect e = context.stat().Accept<Effect>(this);
      e.user = user;
      e.source = source;
      return e;
    }

    public override Effect VisitStatRepeat(RunesParser.StatRepeatContext context) {
      IntValue times_to_repeat = context.value().Accept<IntValue>(value_visitor);
      Effect effect = context.stat().Accept<Effect>(this);
      RepeatEffect repeated = new RepeatEffect(times_to_repeat, effect);
      repeated.user = user;
      repeated.source = source;
      return repeated;
    }

    public override Effect VisitStatScalar(RunesParser.StatScalarContext context) {
      IntValue value = context.value().Accept<IntValue>(new IntValueVisitor());
      Player target = context.player().Accept<Player>(player_visitor);
      ScalarEffect scalar = context.scalarEffect().Accept<ScalarEffect>(scalar_effect_visitor);
      scalar.target = target;
      scalar.ivalue = value;
      scalar.user = user;
      scalar.source = source;
      return scalar;
    }

    public override Effect VisitStatList(RunesParser.StatListContext context) {
      Effect first = context.stat(0).Accept<Effect>(this);
      Effect second = context.stat(1).Accept<Effect>(this);
      first.next = second;
      return first;
    }
    
    public override Effect VisitStatWhen(RunesParser.StatWhenContext context) {
      Effect triggered_effect = context.stat().Accept<Effect>(this);
      EventMatcher trigger_condition = context.condition().Accept<EventMatcher>(condition_visitor);
      Effect scheduled_effect = new ScheduleEffect(trigger_condition, triggered_effect);
      scheduled_effect.user = user;
      scheduled_effect.source = source;
      return scheduled_effect;
    }

    // TODD(ticktakashi): statCard (requires condCard)
  }

  class ConditionVisitor : RunesParserBaseVisitor<EventMatcher> {
    public IntValueVisitor value_visitor;

    public ConditionVisitor(IntValueVisitor value_visitor) {
      this.value_visitor = value_visitor;
    }

    // TODO(ticktakashi): condCard (for statCard)

    public override EventMatcher VisitCondScalar(RunesParser.CondScalarContext context) {
      IntValue value = context.value().Accept<IntValue>(value_visitor);
      int op = context.ineq().start.Type;
      EventMatcher condition = null;

      switch (op) {
        case RunesParser.GT:
          condition = delegate(GameEvent e) {
            return e.scalar_value > value.GetValue();
          };
          break;
        case RunesParser.LT:
          condition = delegate(GameEvent e) {
            return e.scalar_value < value.GetValue();
          };
          break;
        case RunesParser.EQ:
          condition = delegate(GameEvent e) {
            return e.scalar_value == value.GetValue();
          };
          break;
        default:
          Console.WriteLine("You have no yet implemented this binop: " + context.ineq().GetText());
          break;
      }
      
      EventMatcher checkType = CheckType(context.scalarEffect().start.Type);
      
      EventMatcher combined = delegate(GameEvent e) {
        return checkType(e) && condition(e);
      };

      return combined;
    }

    public override EventMatcher VisitCondExpr(RunesParser.CondExprContext context) {
      int op = context.binopBool().start.Type;
      EventMatcher condition = null;
      EventMatcher left = context.condition(0).Accept<EventMatcher>(this);
      EventMatcher right = context.condition(0).Accept<EventMatcher>(this);

      switch (op) {
        case RunesParser.OR:
          condition = delegate(GameEvent e) {
            return left(e) || right(e);
          };
          break;
        case RunesParser.AND:
          condition = delegate(GameEvent e) {
            return left(e) && right(e);
          };
          break;
        default:
          Console.WriteLine("You have no yet implemented this binop: " + context.binopBool().GetText());
          break;
      }

      return condition;
    }

    public override EventMatcher VisitCondNot(RunesParser.CondNotContext context) {
      EventMatcher cond = context.condition().Accept<EventMatcher>(this);
      return delegate(GameEvent e) { return !cond(e); };
    }

    public override EventMatcher VisitCondParen(RunesParser.CondParenContext context) {
      EventMatcher cond = context.condition().Accept<EventMatcher>(this);
      return cond;
    }

    // Creates a function which matches the event type.
    public EventMatcher CheckType(int token_index) {
      return delegate(GameEvent e) {
        return e.event_type == token_index;
      };
    }
  }

  class ScalarEffectVisitor : RunesParserBaseVisitor<ScalarEffect> {
    public override ScalarEffect VisitScalarEffect(RunesParser.ScalarEffectContext context) {
      ScalarEffect effect = null;

      int effectType = context.start.Type;
      switch (effectType) {
        case RunesParser.DRAWS:
          effect = new Draw();
          break;
        case RunesParser.HEALS:
          effect = new Heal();
          break;
        case RunesParser.TAKES:
          effect = new Damage();
          break;
        case RunesParser.SHIELDS:
          // TODO(ticktakashi): Implement Shields.
          // effect = new Shields();
          break;
        case RunesParser.PIERCES:
          // TODO(ticktakashi): Implement Pierces.
          // effect = new Pierces();
          break;
        default:
          Console.WriteLine("Could not match Scalar Effect. Index was: " + effectType + 
            "Defaulting to TAKES");
          goto case RunesParser.TAKES;
      }

      return effect;
    }
  }

  class PlayerVisitor : RunesParserBaseVisitor<Player> {
    Player user;
    Player opponent;

    public PlayerVisitor(Player user, Player opponent) {
      this.user = user;
      this.opponent = opponent;
    }
    
    public override Player VisitPlayer(RunesParser.PlayerContext context) {
      if (context.ENEMY() != null)
        return opponent;
      else
        return user;
    }
  }

  class IntValueVisitor : RunesParserBaseVisitor<IntValue> {
    public override IntValue VisitValue(RunesParser.ValueContext context) {
      // TODO(ticktakashi): Once other types of values have been added, add them here.
      int value = Int32.Parse(context.NUMBER().GetText());
      return new IntValue(value);
    }
  }
}