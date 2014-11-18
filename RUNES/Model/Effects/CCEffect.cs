using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class CCEffect : Effect  {
    CCType cc;
    int duration;

    internal CCEffect(CCType type, int duration) {
      this.cc = type;
      this.duration = duration;
    }

    public override void Activate(GameCard card, Player user, Game game) {
      Player target = game.Opponent(user);
      target.ApplyCC(cc, duration);
      base.Activate(card, user, game);
    }

  }
}
