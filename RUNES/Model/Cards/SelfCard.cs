using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The SelfCard Class
   * 
   * Corresponds to a Self cast buff in league of legends. There is a limited
   * duration.
   */
  // TODO(ticktakashi): Implement SelfCard.
  public class SelfCard : Card {
    protected int time;

    public SelfCard(string name, int id, int cost, int limit, int time,
        Effect effect) : base(name, id, cost, limit, effect) {
      this.time = time;
    }

    public override void Activate(IPlayer user, IGameController game_controller) {
      // TODO(ticktakashi): Implement SelfCard timer semantics
      throw new NotImplementedException();
    }
  }
}
