using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript {
  public class EffectCompiler {
    public static Effect Compile(IGameController controller, Player user,
      ICard source) {
      AntlrInputStream input_stream = new AntlrInputStream(
        source.GetCARDScript());
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.EffectContext effect = parser.effect();
      EffectVisitor effect_visitor = new EffectVisitor(user, 
        controller.Opponent(user), source);
      return effect.Accept<Effect>(effect_visitor);
    }
  }
}
