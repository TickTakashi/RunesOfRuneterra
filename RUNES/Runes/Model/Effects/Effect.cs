using System.Collections;

namespace RUNES.Runes.Model.Effects {
  public abstract class Effect {
    private Card _source;

    public Card source {
      get { return _source; }
      set { _source = value; }
    }

    private Player _user;

    public Player user {
      get { return _user; }
      set { _user = value; }
    }

    private Effect _next;

    public Effect next {
      get { return _next; }
      set { _next = value; }
    }

    // Activate this effect, returns true if the effect is complete and can be deleted.
    public virtual bool  Activate() {
      if (next != null) {
        return next.Activate();
      } else {
        return true;
      }
    }

    // Wrap this so that I don't have to import unity everywhere... :D 
    public void Log(string message) {
      //Debug.Log(message);
    }

    public override string ToString() {
      string extend = "";
      if (next != null) {
        extend = " and then " + next.ToString(); 
      }
      return extend;
    }
  }
}