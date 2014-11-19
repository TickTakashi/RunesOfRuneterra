using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards.CardConditions {
  internal interface GameCardCondition {
    internal bool Condition(GameCard c);
  }
}
