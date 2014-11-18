﻿using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects {
  internal class ShieldEffect : Effect {
    int strength;

    public ShieldEffect(int strength) {
      this.strength = strength;
    }

    public override void Activate(GameCard card, Player user, Game game) {
      Shield shield = new Shield(strength);
      user.ApplyBuff(card, shield);
      base.Activate(card, user, game);
    }
  }
}
