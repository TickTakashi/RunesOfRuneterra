using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Matchers;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Events {
  public class GameEvent_OLD {
    public int event_type = -1;
    public int scalar_value = -1;
    public IPlayer target_player;
  }
}
