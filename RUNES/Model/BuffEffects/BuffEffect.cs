using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.BuffEffects {
  internal abstract class BuffEffect {
    private BuffEffect _next;

    public BuffEffect Next {
      get { return _next; }
      set { _next = value; }
    }
 
    public virtual void Activate(int time, Card card, Player user, Game game) {
      if (Next != null)
        Next.Activate(time, card, user, game);
    }

    public virtual bool CanActivate(int time, Card card, Player user, 
      Game game) {
        return !user.IsCCd(CCType.STUN) && !user.IsCCd(CCType.SILENCE) &&
          (Next == null || Next.CanActivate(time, card, user, game));
    }
  }
}
