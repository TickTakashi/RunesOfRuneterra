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
  public class CardCompiler {
    public static Card Compile(IGameController controller, IPlayer user,
      string card_description) {
      if (card_description == null || card_description.Length == 0)
        return null;

      AntlrInputStream input_stream = new AntlrInputStream(card_description);
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.EffectContext effect = parser.effect();
      CardVisitor card_visitor = new CardVisitor(user,
                                                 controller.Opponent(user),
                                                 card_description);

      return effect.Accept<Card>(card_visitor);
    }
  }
}
