using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  class GameCardVisitor : CARDScriptParserBaseVisitor<GameCard> {
    EffectVisitor effect_visitor;
    BuffVisitor buff_visitor;

    public GameCardVisitor() {
      this.effect_visitor = new EffectVisitor();
      this.buff_visitor = new BuffVisitor();
    }

    private void ParseCardID(GameCardBuilder builder,
      CARDScriptParser.CardIDContext context) {
      builder = builder.WithName(context.NAME().GetText().Trim('"'))
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
}
