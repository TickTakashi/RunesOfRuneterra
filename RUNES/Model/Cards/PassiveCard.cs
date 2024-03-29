﻿using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class PassiveCard : Card {
    private Buff _buff;
    public Buff Buff { get { return _buff; } }

    public PassiveCard(string name, int id, Buff buff)
      : base(name, id) {
        this._buff = buff;
    }

    public override string Description() {
      return Buff.ToString();
    } 
  }
}
