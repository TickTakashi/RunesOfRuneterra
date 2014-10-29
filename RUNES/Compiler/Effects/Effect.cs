using CARDScript.Compiler;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System.Collections;

namespace CARDScript.Compiler.Effects {
  public abstract class Effect {
    private Effect _next;

    public Effect next {
      get { return _next; }
      set { _next = value; }
    }

    // Activate this effect, returns true if the effect can be deleted.
    public virtual bool  Activate(Card card, IPlayer user, IGameController controller) {
      if (next != null) {
        return next.Activate(card, user, controller);
      } else {
        return true;
      }
    }

    public virtual bool CanActivate(Card card, IPlayer user, IGameController controller) {
      return !user.HasBuff(BuffType.STUN) && !user.HasBuff(BuffType.SILENCE) &&
        (next == null || next.CanActivate(card, user, controller));
    }

    // Does this effect deal this cards damage? It may do so next turn or 
    // it may do so right now. Either way, we need to know if this happens so 
    // that we can decide whether or not to add a DealsCardDamageEffect when a
    // DamageCard is created.
    public virtual bool DealsCardDamage() {
      if (next != null) {
        return next.DealsCardDamage();
      } else {
        return false;
      }
    }

    public virtual bool CanNegate(Effect effect) {
      if (next != null) {
        return next.CanNegate(effect);
      } else {
        return false;
      }
    }

    // TODO(ticktakashi): Investigate the sanity of this.  
    public void Log(string message) {
      //Debug.Log(message);
    }

    public override string ToString() {
      string extend = "";
      if (next != null) {
        extend = ", then " + next.ToString(); 
      }
      return extend;
    }
  }
}