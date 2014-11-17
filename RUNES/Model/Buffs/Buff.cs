using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.BuffEffects {
  internal abstract class Buff {
    private Buff _next;

    public Buff Next {
      get { return _next; }
      set { _next = value; }
    }

    public virtual int ModifyDamage(int d, Player p, Game g) {
      return Next != null ? Next.ModifyDamage(d, p, g) : d;
    }

    public virtual int ModifyHeal(int d, Player p, Game g) {
      return Next != null ? Next.ModifyHeal(d, p, g) : d;
    }
  }
}
