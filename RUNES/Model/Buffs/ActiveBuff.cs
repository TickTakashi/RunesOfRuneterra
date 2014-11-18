using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Buffs {
  public class ActiveBuff : Buff {
    public Card source;

    public ActiveBuff(Card source, Buff buff) {
      this.source = source;
      this.Next = buff;
    }
  }
}
