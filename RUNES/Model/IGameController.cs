using CARDScript.Compiler;
using CARDScript.Compiler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public interface IGameController {
    void FireEvent(GameEvent game_event);
    void Schedule(GameEventListener listener);
    IPlayer Opponent(IPlayer p);
  }
}
