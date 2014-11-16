using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  public interface IValue {
    int GetValue(); 
  }

  public class LiteralIntValue : IValue {
    int value = 0;
    public LiteralIntValue(int value) {
      this.value = value;
    }

    public int GetValue() {
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

    public int GetValue() {
      Random rand = new Random();
      return rand.Next(r.GetValue(), l.GetValue());
    }

    public override string ToString() {
      return "between " + l + " and " + r;
    }
  }
}
