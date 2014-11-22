using CARDScript.Compiler;
using CARDScript.Model;
using CARDScript.Model.GameCards;
using System.Collections;

namespace CARDScript.Compiler.Effects {
  internal class Effect {
    private Effect _next;

    internal Effect Next {
      get { return _next; }
      set { _next = value; }
    }

    // After this effect has completed, call base to activate the next effect.
    internal virtual void Activate(GameCard card, Player user, Game game) {
      if (Next != null)
        Next.Activate(card, user, game);
    }

    // This effect can only be activated if ALL effects in its effect chain
    // are activatable.
    internal virtual bool CanActivate(GameCard card, Player user, Game game) {
      return !user.IsCCd(CCType.SILENCE) && (Next == null ||
        Next.CanActivate(card, user, game));
    }

    internal virtual bool CanNegate(GameCard card, Player user, Game game,
      NormalEffect effect) {
      return CanActivate(card, user, game) && Next != null && 
        Next.CanNegate(card, user, game, effect);
    }

    // Does this effect include the cards normal damage? If not, we might need
    // to add it.
    internal virtual bool ContainsNormalEffect() {
      return Next != null && Next.ContainsNormalEffect();
    }

    internal Effect GetLastEffect() {
      if (Next == null)
        return this;
      else
        return Next.GetLastEffect();
    }

    public override string ToString() {
      string extend = "";
      if (Next != null) {
        extend = ", then " + Next.ToString(); 
      }
      return extend;
    }
  }
}