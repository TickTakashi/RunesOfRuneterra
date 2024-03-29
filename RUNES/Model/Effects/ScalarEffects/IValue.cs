﻿using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.Effects.CardEffects;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.ScalarEffects {
  internal interface IValue {
    int GetValue(Player user, Game game); 
  }

  internal class LiteralIntValue : IValue {
    int value = 0;
    internal LiteralIntValue(int value) {
      this.value = value;
    }

    public int GetValue(Player user, Game game) {
      return value;
    }

    public override string ToString() {
      return "" + value; 
    }
  }

  internal class RandomValue : IValue {
    IValue l;
    IValue r;

    internal RandomValue(IValue l, IValue r) {
      this.l = l;
      this.r = r;
    }

    public int GetValue(Player user, Game game) {
      Random rand = new Random();
      return rand.Next(r.GetValue(user, game), 
        l.GetValue(user, game));
    }

    public override string ToString() {
      return "between " + l + " and " + r;
    }
  }

  internal class DoubleValue : IValue {
    IValue r;

    internal DoubleValue(IValue r) {
      this.r = r;
    }

    public int GetValue(Player user, Game game) {
      return 2 * r.GetValue(user, game); 
    }
  }

  internal class HalfValue : IValue {
    IValue r;

    internal HalfValue(IValue r) {
      this.r = r;
    }

    public int GetValue(Player user, Game game) {
      return r.GetValue(user, game) / 2;
    }
  }

  internal class DistanceValue : IValue {
    public int GetValue(Player user, Game game) {
      return game.Distance(user, game.Opponent(user));
    }
  }

  internal class HealthValue : IValue {
    Target t;

    internal HealthValue(Target t) {
      this.t = t;
    }

    public int GetValue(Player user, Game game) {
      Player target = TargetMethods.Resolve(t, user, game);
      return target.Health;
    }
  }

  internal class CardCountValue : IValue {
    GameCardCondition condition;
    Target t;
    Location l;

    internal CardCountValue(GameCardCondition condition, Target t, Location l) {
      this.condition = condition;
      this.t = t;
      this.l = l;
    }

    public int GetValue(Player user, Game game) {
      Player target = TargetMethods.Resolve(t, user, game);
      CardCollection collection = LocationMethods.Resolve(target, l);
      List<GameCard> matching = collection.CardsWhichSatisfy(condition);
      return matching.Count;
    }
  }
}
