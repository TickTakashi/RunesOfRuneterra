using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript {
  /* The Card Compiler Class
   * 
   * Contains the static method Compile, which creates a Card from a 
   * CARDScript description.
   */
  public class CardCompiler {
    static CardVisitor card_visitor;
    static PassiveVisitor passive_visitor;

    public static Card_OLD Compile(string card_description) {
      if (card_visitor == null)
        card_visitor = new CardVisitor();

      if (card_description == null || card_description.Length == 0)
        return null;

      AntlrInputStream input_stream = new AntlrInputStream(card_description);
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.CardDescContext description = parser.cardDesc();
     
      return description.Accept<Card_OLD>(card_visitor);
    }

    public static Passive CompilePassive(string card_description) {
      if (passive_visitor == null)
        passive_visitor = new PassiveVisitor();

      if (card_description == null || card_description.Length == 0)
        return null;

      AntlrInputStream input_stream = new AntlrInputStream(card_description);
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.CardDescContext description = parser.cardDesc();

      return description.Accept<Passive>(passive_visitor);
    }
  }
}
