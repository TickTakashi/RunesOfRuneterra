using Antlr4.Runtime;
using RUNES.Runes.Compiler;
using RUNES.Runes.Model;
using RUNES.Runes.Model.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RUNES.Runes {
  class RunesLanguageBuilder {
    public static void Main() {
      string input =  "WHEN USER SHIELDS > 1 {\n" +
                      "  ENEMY TAKES 1 3 TIMES \n" +
                      "} 3 CHARGES";

      Console.WriteLine("Compiling this:");
      Console.WriteLine(input);
      Console.WriteLine();
      Effect compiled = Compile(input);
      Console.WriteLine("Compiled object: " + compiled);

      Console.WriteLine();
      Console.WriteLine("Testing ScheduleEffect");
      compiled.Activate();
    }

    public static Effect Compile(string input) {
      AntlrInputStream input_stream = new AntlrInputStream(input);
      RunesLexer lexer = new RunesLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      RunesParser parser = new RunesParser(tokens);
      EffectVisitor visitor = new EffectVisitor(new Player(), new Player(), new Card());
      RunesParser.EffectContext effect = parser.effect();
      return effect.Accept<Effect>(visitor);
    }
  }
}
