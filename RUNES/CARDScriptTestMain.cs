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

      string test_card = 
@"""Blood Price"" = 2 
COST = 1
LIMIT = 3
MELEE
DAMAGE = 2
EFFECT = {
  USER TAKES 2
  USER MAY ADDS 1 TITLE = ""Blood Thirst"" FROM USER DECK TO USER HAND
}";

      string test_passive =
@"""Dread"" = 0 
BUFF = {
  SPELL_D 1
}";

      List<string> cards = new List<string>();
      cards.Add(test_card);
      List<string> passives = new List<string>();
      passives.Add(test_passive);

      CardCompiler.BuildLibrary(passives, cards);
      PassiveCard pc = CardCompiler.CompilePassiveCard(test_passive);
      Console.WriteLine("Compiled Passive: ");
      Console.WriteLine(pc);

      GameCard gc = CardCompiler.CompileGameCard(test_card);
      Console.WriteLine("Compiled Game Card: ");
      Console.WriteLine(gc);
    }
  }
}
