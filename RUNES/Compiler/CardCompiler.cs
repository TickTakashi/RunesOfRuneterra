﻿using Antlr4.Runtime;
using CARDScript.Compiler;
using CARDScript.Compiler.CARDScriptVisitors;
using CARDScript.Compiler.Effects;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  /* The Card Compiler Class
   * 
   * Contains the static method Compile, which creates a Card from a 
   * CARDScript description.
   */
  public static class CardCompiler {
    static GameCardVisitor card_visitor;
    static PassiveCardVisitor passive_visitor;
    public static Dictionary<int, string> Passives;
    public static Dictionary<int, string> GameCards;

    internal static GameCard CompileGameCard(string card_description) {
      if (card_visitor == null)
        card_visitor = new GameCardVisitor();

      if (card_description == null || card_description.Length == 0)
        return null;

      AntlrInputStream input_stream = new AntlrInputStream(card_description);
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.CardDescContext description = parser.cardDesc();
     
      return description.Accept<GameCard>(card_visitor);
    }

    internal static PassiveCard CompilePassiveCard(string card_description) {
      if (passive_visitor == null)
        passive_visitor = new PassiveCardVisitor();

      if (card_description == null || card_description.Length == 0)
        return null;

      AntlrInputStream input_stream = new AntlrInputStream(card_description);
      CARDScriptLexer lexer = new CARDScriptLexer(input_stream);
      CommonTokenStream tokens = new CommonTokenStream(lexer);
      CARDScriptParser parser = new CARDScriptParser(tokens);
      CARDScriptParser.PassiveDescContext description = parser.passiveDesc();

      return description.Accept<PassiveCard>(passive_visitor);
    }

    public static void BuildLibrary(List<string> passives, List<string> cards) {
      Passives = new Dictionary<int, string>();
      GameCards = new Dictionary<int, string>();

      Console.WriteLine("Compiling passives: ");
      foreach(string s in passives) {
        Console.WriteLine("\nCompiling this: ");
        Console.WriteLine(s);
        
        PassiveCard p = CompilePassiveCard(s);
        Passives[p.ID] = s;
      }

      Console.WriteLine("Compiling game cards: ");
      foreach (string s in cards) {
        Console.WriteLine("\nCompiling this: ");
        Console.WriteLine(s);
        
        GameCard gc = CompileGameCard(s);
        GameCards[gc.ID] = s;
      }
    }

    public static List<GameCard> BulidDeck(List<int> ids) {
      List<GameCard> gcs = new List<GameCard>();
      foreach (int i in ids)
        gcs.Add(CompileGameCard(GameCards[i]));
      return gcs;
    }

    public static List<PassiveCard> BuildPassiveDeck(List<int> ids) {
      List<PassiveCard> pcs = new List<PassiveCard>();
      foreach (int i in ids)
        pcs.Add(CompilePassiveCard(Passives[i]));
      return pcs;
    }
  }
}
