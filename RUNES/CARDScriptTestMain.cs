﻿using Antlr4.Runtime;
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
      string script =
@"""Solar Flare"" = 52
COST = 2
LIMIT = 1
SKILL
DAMAGE = 2
RANGE = 4
EFFECT = {
  STUN 1
}";      
      DummyGameController g = new DummyGameController();
      Card_OLD compiled = CardCompiler.Compile(script);

      if (compiled == null) {
        Console.WriteLine("NULL!");
        return;
      }


      Console.WriteLine(script);
      System.IO.StreamWriter file = new System.IO.StreamWriter(
        "D:\\GameDev\\Unity Projects\\LoLCardGame\\Assets\\CARDSSources\\" +
        compiled.id.ToString("000") + "_" + 
        compiled.name.Replace(" ", "_").ToUpper() + ".txt");

      file.WriteLine(script);

      file.Close();
      Console.WriteLine("Compiled object:\n" + compiled);
    }
  }


  class DummyGameController : IGameController {

    public void FireEvent(GameEvent_OLD game_event) {
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

    public void PromptNegate(IPlayer user, Card_OLD card, Effect effect, EffectCallback callback) {
      throw new NotImplementedException();
    }

    public void PromptMove(IPlayer user, Card_OLD card, int distance, EffectCallback callback) {
      throw new NotImplementedException();
    }


    public void MovePlayer(IPlayer player, int distance) {
      throw new NotImplementedException();
    }


    public int PlayerDistance() {
      throw new NotImplementedException();
    }


    public Card_OLD BuildCard(string card_name) {
      throw new NotImplementedException();
    }


    public void KnockbackPlayer(IPlayer target_player, int value, EffectCallback callback) {
      throw new NotImplementedException();
    }


    public void KnockbackPlayer(IPlayer target_player, int value, Card_OLD card, IPlayer user, IGameController controller, EffectCallback callback) {
      throw new NotImplementedException();
    }
  }
}
