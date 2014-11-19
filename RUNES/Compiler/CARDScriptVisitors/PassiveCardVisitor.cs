using CARDScript.Model.BuffEffects;
using CARDScript.Model.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.CARDScriptVisitors {
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
}
