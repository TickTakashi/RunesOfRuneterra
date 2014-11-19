using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards.CardConditions {
  internal class DashCondition : GameCardCondition {
    public bool Condition(GameCard card) {
      return card.IsDash;
    }
  }
}
