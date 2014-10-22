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
      DummyPlayer a = new DummyPlayer();
      DummyPlayer b = new DummyPlayer();
      DummyGameController g = new DummyGameController(a, b);
      Effect compiled = EffectCompiler.Compile(g, a, d);

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
    Player user;
    Player enemy;

    public DummyGameController(Player user, Player enemy) {
      this.user = user;
      this.enemy = enemy;
    }

    public void Schedule(Compiler.Events.GameEventListener listener) {
      throw new NotImplementedException();
    }

    public Player Opponent(Player p) {
      return p == user ? enemy : user;
    }
  }

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
