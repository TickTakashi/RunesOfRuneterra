﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards.CardConditions {
  internal interface CardCondition {
    internal bool Condition(GameCard c);
  }
}