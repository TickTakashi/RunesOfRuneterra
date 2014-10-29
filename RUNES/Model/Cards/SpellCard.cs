﻿using CARDScript.Model;
using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CARDScript.Model.Players;

namespace CARDScript.Model.Cards {
  /* The SpellCard Class
   * 
   * A Spell card almost corresponds to a targeted skill in league of legends.
   * They cannot be dodged. It has a range, which means that
   * the card can only be used if you are within range distance of your 
   * opponent.
   */
  public class SpellCard : DamageCard {
    private int _range;
    public int range { get { return _range; } }

    public SpellCard(string name, int id, int damage, int range, int cost,
      int limit, Effect effect) : base(name, id, damage, cost, limit, effect) {
      this._range = range;
    }

    public override bool InRange(IPlayer user, IGameController controller) {
      return controller.InRange(user, range);
    }

    public override string PrettyPrint() {
      return base.PrettyPrint() + string.Format("\nDamage: {0}\nRange: {1}", 
        damage, range);
    }
  }
}