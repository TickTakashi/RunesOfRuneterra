using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public abstract class Card {
    protected int cost;
    public abstract void Activate();
  }
}
