using CARDScript.Compiler;
using CARDScript.Model;
using CARDScript.Model.Cards;
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