using CARDScript.Compiler;
using System.Collections;

namespace CARDScript.Compiler.Effects {
  public abstract class Effect {
    private ICard _source;

    public ICard source {
      get { return _source; }
      set { _source = value; }
    }

    private IPlayer _user;

    public IPlayer user {
      get { return _user; }
      set { _user = value; }
    }

    private Effect _next;

    public Effect next {
      get { return _next; }
      set { _next = value; }
    }

    // Activate this effect, returns true if the effect can be deleted.
    public virtual bool  Activate() {
      if (next != null) {
        return next.Activate();
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