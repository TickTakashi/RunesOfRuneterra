using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CARDScript {
  class CARDScriptTestMain {
    public static void Main() {
      
      DummyGameController g = new DummyGameController();
      Card compiled = CardCompiler.Compile(
        "\"Test\" = 0 \n" +
        "SPELL \n" +
        "DAMAGE = 1 \n" +
        "RANGE = 2 \n" + 
        "COST = 3 \n" +
        "LIMIT = 4 \n" +
        "EFFECT = { \n" +
        "  USER TAKES 3 \n" +
        "}"
        );

      if (compiled == null) {
        Console.WriteLine("NULL!");
      }
      Console.WriteLine("Compiled object:\n" + compiled);
    }
  }


  class DummyGameController : IGameController {

    public void FireEvent(GameEvent game_event) {
      throw new NotImplementedException();
    }

    public void Schedule(GameEventListener listener) {
      throw new NotImplementedException();
    }

    public IPlayer Opponent(IPlayer p) {
      throw new NotImplementedException();
    }

    public bool InRange(IPlayer user, int range) {
      throw new NotImplementedException();
    }

    public void StartTurn() {
      throw new NotImplementedException();
    }

    public void EndTurn() {
      throw new NotImplementedException();
    }

    public IPlayer Current() {
      throw new NotImplementedException();
    }

    public void PromptNegate(IPlayer user, Card card, Effect effect, EffectCallback callback) {
      throw new NotImplementedException();
    }

    public void PromptMove(IPlayer user, Card card, int distance, EffectCallback callback) {
      throw new NotImplementedException();
    }


    public void MovePlayer(IPlayer player, int distance) {
      throw new NotImplementedException();
    }
  }
}
