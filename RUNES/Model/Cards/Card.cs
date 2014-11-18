using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public abstract class Card {
    protected string _name;
    public string Name { get { return _name; } }

    protected int _id;
    public int ID { get { return _id; } }

    protected bool _is_ult;
    public bool IsUltimate { get { return _is_ult; } }

    protected bool _is_dash;
    public bool IsDash { get { return _is_dash; } }

    protected int _dash_distance;
    public int DashDistance { get { return _dash_distance; } }

    protected int _cost;
    public int Cost(Player user, Game game) {
      return user.CardCost(this, _cost);
    }

    protected int _limit;
    public int Limit { get { return _limit; } }

    public Effect effect;

    public Card(string name, int id, bool is_ult, bool is_dash,
      int dash_distance, int cost, int limit) {
      this._name = name;
      this._id = id;
      this._is_ult = is_ult;
      this._is_dash = is_dash;
      this._dash_distance = dash_distance;
      this._cost = cost;
      this._limit = limit;
    }

    public void SetEffect(Effect effect) {
      if (!effect.ContainsNormalEffect()) {
        NormalEffect my_effect = CreateEffect();
        my_effect.Next = effect;
        this.effect = my_effect;
      } else {
        this.effect = effect;
      }
    }

    public bool CanActivate(Player user, Game game) {
      return (!user.IsCCd(CCType.SNARE) && !user.IsCCd(CCType.SILENCE) &&
        !user.IsCCd(CCType.STUN) && IsDash) || 
        effect.CanActivate(this, user, game);
    }

    public void Activate(Player user, Game game) {
      if (IsDash) {
        GameState old = game.GetState();
        game.SetState(new ChooseLocationState(user, DashDistance,
           delegate() {
             game.SetState(old);
             effect.Activate(this, user, game);
           },
           game));
      } else {
        effect.Activate(this, user, game);
      }
    }

    public bool CanNegate(Player user, Game game, NormalEffect e) {
      return effect.CanNegate(this, user, game, e);
    }

    public abstract NormalEffect CreateEffect();
  }

  internal abstract class CardBuilder {
    protected string name = "";
    protected int id = -1;
    protected bool is_ult = false;
    protected bool is_dash = false;
    protected int dash_distance = 0;
    protected int cost = -1;
    protected int limit = -1;
    protected Effect effect;

    internal abstract Card Build();

    internal virtual bool CanBuild() {
      return name == "" || id == -1 || cost == -1 || limit == -1;
    }

    internal CardBuilder WithName(string name) {
      this.name = name;
      return this;
    }

    internal CardBuilder WithID(int id) {
      this.id = id;
      return this;
    }

    internal CardBuilder WithUlt() {
      this.is_ult = true;
      return this;
    }

    internal CardBuilder WithDash(int distance) {
      this.dash_distance = distance;
      this.is_dash = true;
      return this;
    }

    internal CardBuilder WithLimit(int limit) {
      this.limit = limit;
      return this;
    }

    internal CardBuilder WithCost(int cost) {
      this.cost = cost;
      return this;
    }

    internal CardBuilder WithEffect(Effect effect) {
      this.effect = effect;
    }
  }
}
