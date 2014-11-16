using CARDScript.Compiler.Effects;
using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class BuffCard : Card {
    private int _time;
    public int Time(Player user, Game game) {
      return _time;
    }

    private BuffEffect _buff;
    public BuffEffect Buff { get { return _buff; } }


    public BuffCard(string name, int id, bool is_ult, bool is_dash,
       int dash_distance, int cost, int limit, int time, BuffEffect buff) : 
      base(name, id, is_ult, is_dash, dash_distance, cost, limit) {
        this._time = time;
        this._buff = buff;
    }

    public override NormalEffect CreateEffect() {
      return new BuffActivationEffect();
    }
  }

  public class BuffCardBuilder : CardBuilder {
    int time;
    BuffEffect buff;

    internal override Card Build() {
      return new BuffCard(name, id, is_ult, is_dash, dash_distance, cost, 
        limit, time, buff);
    }

    internal BuffCardBuilder WithTime(int time) {
      this.time = time;
      return this;
    }

    internal BuffCardBuilder WithBuff(BuffEffect buff) {
      this.buff = buff;
      return this;
    }
  }
}
