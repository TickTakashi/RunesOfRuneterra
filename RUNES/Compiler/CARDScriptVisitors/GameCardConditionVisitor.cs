using CARDScript.Model.Cards.CardConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
  class GameCardConditionVisitor :
    CARDScriptParserBaseVisitor<GameCardCondition> {
    public override GameCardCondition VisitCardCondName(
      CARDScriptParser.CardCondNameContext context) {
      return new NameCondition(context.NAME().GetText().Trim('"'));
    }

    public override GameCardCondition VisitCardCondType(
      CARDScriptParser.CardCondTypeContext context) {
      string type_name = context.cardType().GetText();
      CardType ct = (CardType)Enum.Parse(typeof(CardType), type_name, true);
      return new TypeCondition(ct);
    }

    public override GameCardCondition VisitCardCondDash(
      CARDScriptParser.CardCondDashContext context) {
      return new DashCondition();
    }

    public override GameCardCondition VisitCardCondUlt(
      CARDScriptParser.CardCondUltContext context) {
      return new UltCondition();
    }
  }
}
