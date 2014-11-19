using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.ScalarEffects {
  public interface IValue {
    int GetValue(Player user, Game game); 
  }

  public class LiteralIntValue : IValue {
    int value = 0;
    public LiteralIntValue(int value) {
      this.value = value;
    }

    public int GetValue(Player user, Game game) {
      return value;
    }

    public override string ToString() {
      return "" + value; 
    }
  }

  public class RandomValue : IValue {
    IValue l;
    IValue r;

    public RandomValue(IValue l, IValue r) {
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
}
