using CARDScript.Compiler.Events;
using CARDScript.Model;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public abstract class Matcher {
    public abstract bool Match(GameEvent_OLD e, IGameController controller, IPlayer scheduler);
  }
}
