using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CARDScript {
  class CARDScriptTestMain {
    public static void Main() {
      Console.WriteLine("Creating Empty Decks."); 
      List<GameCard> player_1_deck = new List<GameCard>();
      List<PassiveCard> player_1_passive = new List<PassiveCard>();
      List<GameCard> player_2_deck = new List<GameCard>();
      List<PassiveCard> player_2_passive = new List<PassiveCard>();

      Console.WriteLine("Creating New Game With Empty Decks.");
      Game game = new Game(player_1_deck, player_1_passive, player_2_deck,
        player_2_passive);
    }
  }
}
