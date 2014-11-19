using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Effects.ScalarEffects;
using CARDScript.Model;
using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.Effects;
using CARDScript.Model.Effects.CardEffects;
using CARDScript.Model.Effects.ScalarEffects;
using CARDScript.Model.GameConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  public class EffectVisitor : CARDScriptParserBaseVisitor<Effect> {
    IValueVisitor value_visitor;
    GameCardConditionVisitor card_condition_visitor;
    GameConditionVisitor game_condition_visitor;

    public EffectVisitor() {
      this.value_visitor = new IValueVisitor();
      this.card_condition_visitor = new GameCardConditionVisitor();
      this.game_condition_visitor = new GameConditionVisitor();
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
        case (CARDScriptParser.SKILL):
          return new SkillshotEffect();
        case (CARDScriptParser.SPELL):
          return new DamageEffect();
        case (CARDScriptParser.DAMAGE):
          return new DamageEffect();
        case (CARDScriptParser.MELEE):
          return new MeleeEffect();
        case (CARDScriptParser.SELF):
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
      GameCardCondition condition =
        context.cardCond().Accept<GameCardCondition>(card_condition_visitor);
      bool is_optional = context.MAY() != null;
      return new CardMoveEffect(choice_maker, value, debit_player,
        debit_location, credit_player, credit_location, condition,
        is_optional);
    }

    public override Effect VisitStatEIf(
      CARDScriptParser.StatEIfContext context) {
        GameCondition condition = context.stateCond().Accept<GameCondition>(
          game_condition_visitor);
        Effect if_body = context.effect(0).Accept<Effect>(this);
        Effect else_body = context.effect(1) == null ? null :
          context.effect(1).Accept<Effect>(this);
        return new IfEffect(condition, if_body, else_body);
    }
  }
}
