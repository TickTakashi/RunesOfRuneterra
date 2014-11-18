using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs {
  class TimedBuff : Buff, IRoRObserver<GameEvent> {
    Player target;
    int time_remaining;

    internal TimedBuff(Player target, Game game, Buff b, 
      int total_duration) {
      this.time_remaining = total_duration;
      this.Next = b;
      this.target = target;
      game.Attach(this);
    }

    public override bool IsFinished() {
      return time_remaining <= 0;
    }

    public void Update(GameEvent change_event) {
      if (change_event.type == GameEvent.Type.TURN_END &&
        change_event.player == target) {
          time_remaining--;
          if (time_remaining <= 0)
            change_event.game.Detach(this);
      }
    }
  }
}
