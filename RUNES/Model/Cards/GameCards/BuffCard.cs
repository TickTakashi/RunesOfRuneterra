using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class BuffCard : GameCard {
    private int _time;
    public int Time(Player user, Game game) {
      return _time;
    }

    private Buff _buff;
    public Buff Buff { get { return _buff; } }


    public BuffCard(string name, int id, bool is_ult, int dash_distance, 
      int cost, int limit, int time, Buff buff) : base(name, id, is_ult, 
      dash_distance, cost, limit) {
        this._time = time;
        this._buff = buff;
    }

    internal override NormalEffect CreateEffect() {
      return new BuffEffect();
    }
  }

  internal class BuffCardBuilder : GameCardBuilder {
    int time;
    Buff buff;

    internal override GameCard Build() {
      BuffCard ret = new BuffCard(name, id, is_ult, dash_distance, 
        cost, limit, time, buff);
      ret.SetEffect(effect);
      return ret;
    }

    internal BuffCardBuilder WithTime(int time) {
      this.time = time;
      return this;
    }

    internal BuffCardBuilder WithBuff(Buff buff) {
      this.buff = buff;
      return this;
    }
  }
}
