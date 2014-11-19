using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.Effects.CardEffects;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  class IValueVisitor : CARDScriptParserBaseVisitor<IValue> {
    GameCardConditionVisitor card_condition_visitor;

    public IValueVisitor() {
      this.card_condition_visitor = new GameCardConditionVisitor();
    }

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

    public override IValue VisitValueHealth(
      CARDScriptParser.ValueHealthContext context) {
      Target target = EffectVisitor.ParseTarget(context.player());
      return new HealthValue(target);
    }

    public override IValue VisitValueCardCount(
      CARDScriptParser.ValueCardCountContext context) {
      GameCardCondition condition =
        context.cardCond().Accept<GameCardCondition>(card_condition_visitor);
      Target t = EffectVisitor.ParseTarget(context.player());
      Location l = EffectVisitor.ParseLocation(context.location());
      return new CardCountValue(condition, t, l);
    }
  }
}
