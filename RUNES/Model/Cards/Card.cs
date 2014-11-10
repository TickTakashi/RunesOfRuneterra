using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Matchers;
using CARDScript.Compiler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CARDScript.Model.Players;

namespace CARDScript.Model.Cards {
  /* The Card Class
   * 
   * The base class for all non-passive cards. Contains information about 
   * this card ID number and name, as well as in game limits such as its cost
   * to activate and the max number allowed in a deck.
   */
  public abstract class Card {
    protected string _name;
    public string name { get { return _name; } }

    protected int _id;
    public int id { get { return _id; } }
    
    protected int _cost;
    public int cost { get { return _cost; } }

    protected int _limit;
    public int limit { get { return _limit;  } }

    protected Effect effect;

    public Card(string name, int id, int cost, int limit, Effect effect) {
      this._name = name.Substring(1, name.Length - 2);
      this._id = id;
      this._cost = cost;
      this._limit = limit;
      this.effect = effect;
    }

    // TODO(ticktakashi): Implement conditional activation.
    // public abstract bool CanActivateNow(GameEvent e, card, user, controller)

    public virtual bool CanActivate(IPlayer user, IGameController controller) {
      return effect.CanActivate(this, user, controller) &&
        user.HasActionPoints(cost);
    }

    public bool CanNegate(CardEffect possibly_negatable) {
      return effect.CanNegate(possibly_negatable);
    }

    public virtual void Activate(IPlayer user, IGameController game_controller) {
      // TODO(ticktakashi): Fire an ActivationBegin event here.
      user.Spend(cost);
      effect.Activate(this, user, game_controller);
    }

    public virtual string PrettyPrint() {
      return string.Format("Name: {0}\nID: {1}\nCost: {2}\nLimit: {3}", name, _id,
        cost, _limit);
    }

    public override string ToString() {
      return PrettyPrint() + "\nEffect: " + effect.ToString();
    }

    public string GetDescription() {
      return effect.ToString();
    }
  }
}
