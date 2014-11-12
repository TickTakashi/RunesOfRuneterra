using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  class CardAdder : Effect {
    Target player;
    IValue value;
    string name;
    int destination;

    public CardAdder(Target player, IValue value, string name, int destination) {
      this.player = player;
      this.value = value;
      this.name = name;
      this.destination = destination;
    }

    public override bool Activate(Card card, IPlayer user, IGameController controller) {
      IPlayer target = TargetMethods.Resolve(player, user, controller);
      
      for (int i = 0; i < value.GetValue(); i++) {
        switch(destination) {
          case CARDScriptParser.HAND:
            target.AddToHand(name);
            break;
          case CARDScriptParser.DECK:
            target.AddToDeck(name);
            break;
          case CARDScriptParser.COOL:
            target.AddToCooldown(name);
            break;
          default:
            throw new Exception("No place specified!");
        }
      }

      return base.Activate(card, user, controller);
    }

    public override string ToString() {
      string ret = "add " + value.GetValue() + 
        " " + name + (value.GetValue() > 1 ? "s to " : " to ");
      switch (destination) {
        case CARDScriptParser.HAND:
          ret += TargetMethods.Owner(player) + " hand";
          break;
        case CARDScriptParser.DECK:
          ret += "the top of " + TargetMethods.Owner(player) + " deck";
          break;
        case CARDScriptParser.COOL:
          ret += TargetMethods.Owner(player) + " cooldown";
          break;
        default:
          throw new Exception("No place specified!");
      } 
      return ret + base.ToString();
    }
  }
}
