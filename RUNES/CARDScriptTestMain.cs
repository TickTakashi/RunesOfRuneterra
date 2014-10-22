using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CARDScript {
  class CARDScriptTestMain {
    public static void Main() {
      DummyCard d = new DummyCard("WHEN ENEMY HEALS >= 1 {\n" +
                                  "  ENEMY TAKES 1 3 TIMES \n" +
                                  "  USER HEALS 3\n" +
                                  "} 3 CHARGES");
      DummyCard e = new DummyCard("");
      DummyPlayer a = new DummyPlayer();
      DummyPlayer b = new DummyPlayer();
      DummyGameController g = new DummyGameController(a, b);
      Effect compiled = EffectCompiler.Compile(g, a, e);

      Console.WriteLine("Compiled object: " + compiled);
    }
  }

  class DummyCard : ICard {
    string description;
    
    public DummyCard(string s) {
      description = s;
    }

    public string GetCARDScript() {
      return description;
    }
  }

  class DummyGameController : IGameController {
    IPlayer user;
    IPlayer enemy;

    public DummyGameController(IPlayer user, IPlayer enemy) {
      this.user = user;
      this.enemy = enemy;
    }

    public void Schedule(Compiler.Events.GameEventListener listener) {
      throw new NotImplementedException();
    }

    public IPlayer Opponent(IPlayer p) {
      return p == user ? enemy : user;
    }
  }

  class DummyPlayer : IPlayer {

    public void Damage(int value) {
      throw new NotImplementedException();
    }

    public void Heal(int value) {
      throw new NotImplementedException();
    }

    public void Discard(int index) {
      throw new NotImplementedException();
    }

    public void Draw(int value) {
      throw new NotImplementedException();
    }
  }
}
