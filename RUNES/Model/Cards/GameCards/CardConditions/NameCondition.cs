using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards.CardConditions {
  internal class NameCondition : GameCardCondition {
    private string card_name;
    internal NameCondition(string card_name) {
      this.card_name = card_name;
    }

    public bool Condition(GameCard card) {
      return card.Name == card_name;
    }
  }
}
