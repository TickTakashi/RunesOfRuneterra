using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.CARDScriptCompiler {
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

  // TODO(ticktakashi): Implement a CardValue type
  // that can extract values from the stats of a card
  // E.g. another cards damage. Also implement modifiers for this such as 
  // "double" or "triple"
  public class CardValue : IValue {
    public int GetValue() {
      throw new NotImplementedException();
    }
  }
}
