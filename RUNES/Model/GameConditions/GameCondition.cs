using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.GameConditions {
  public interface GameCondition {
    bool Condition(Player player, Game game);
  }
}
