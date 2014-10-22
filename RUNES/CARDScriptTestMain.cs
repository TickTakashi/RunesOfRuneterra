using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CARDScript {
  class CARDScriptTestMain {
    public static void Main() {
      string input =  "WHEN ENEMY HEALS >= 1 {\n" +
                      "  ENEMY TAKES 1 3 TIMES \n" +
                      "  USER HEALS 3\n" +
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
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      EffectVisitor visitor = new EffectVisitor(new DummyPlayer(), 
                                                new DummyPlayer(),
                                                new DummyCard());
      CARDScriptParser.EffectContext effect = parser.effect();
      return effect.Accept<Effect>(visitor);
    }
  }

  class DummyCard : ICard { }
  class DummyPlayer : Player {

    public override void Damage(int value) {
      throw new NotImplementedException();
    }

    public override void Heal(int value) {
      throw new NotImplementedException();
    }

    public override void Draw() {
      throw new NotImplementedException();
    }

    public override void Discard(int index) {
      throw new NotImplementedException();
    }
  }
}
