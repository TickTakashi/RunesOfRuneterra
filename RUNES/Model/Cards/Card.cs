using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Matchers;
using CARDScript.Compiler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The Card Class
   * 
   * The base class for all non-passive cards. Contains information about 
   * this card ID number and name, as well as in game limits such as its cost
   * to activate and the max number allowed in a deck.
   */
  public abstract class Card {
    protected string name;
    protected int id;
    protected int cost;
    protected int limit;
    protected Effect effect;

    public Card(string name, int id, int cost, int limit, Effect effect) {
      this.name = name;
      this.id = id;
      this.cost = cost;
      this.limit = limit;
      this.effect = effect;
    }

    // TODO(ticktakashi): Implement conditional activation.
    public virtual bool CanActivate(IPlayer user, IGameController UNUSED) {
      return !(user.IsStunned() || user.IsSilenced()) &&
        user.HasActionPoints(cost);
    }

    public virtual void Activate(IPlayer user, IGameController game_controller) {
      // TODO(ticktakashi): Fire an ActivationBegin event here.
      user.Spend(cost);
      effect.Activate(this, user, game_controller);
    }

    public virtual string PrettyPrint() {
      return string.Format("Name: {0}\nID: {1}\nCost: {2}\nLimit: {3}", name, id,
        cost, limit);
    }

    public override string ToString() {
      return PrettyPrint() + "\nEffect: " + effect.ToString();
    }
  }
}
