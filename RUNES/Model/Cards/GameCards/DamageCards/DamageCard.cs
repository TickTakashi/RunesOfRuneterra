using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public abstract class DamageCard : GameCard {
    protected int _damage;
    public virtual int Damage(Player user, Game game) {
      return _damage;
    }

    public abstract int Range(Player user, Game game);
    
    public DamageCard(string name, int id, bool is_ult, int dash_distance, 
      int cost, int limit, int damage) : base(name, id,  is_ult, dash_distance,
      cost, limit)  {
        this._damage = damage;
    }
  }
}
