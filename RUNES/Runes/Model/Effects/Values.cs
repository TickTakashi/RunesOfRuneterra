using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model.Effects {
  public interface IValue {
    int GetValue(); 
  }

  public class IntValue : IValue {
    int value = 0;
    public IntValue(int value) {
      this.value = value;
    }

    public int GetValue() {
      return value;
    }

    public override bool Equals(object obj) {
      if (obj == null)
        return false;

      IntValue iv = obj as IntValue;
      if ((Object)iv == null)
        return false;

      return iv.value == value;
    }
  }

  // TODO(ticktakashi): Implement a CardValue type that
  // can extract values from the stats of a set card. E.g.
  // another cards damage. Also implement modifiers for this
  // such as "double" or "triple"
  public class CardValue : IValue {
    public int GetValue() {
      throw new NotImplementedException();
    }
  }
}
