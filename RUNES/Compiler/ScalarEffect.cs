using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  public class ScalarEffect : Effect {
    private IValue _ivalue;

    public IValue ivalue {
      get { return _ivalue; }
      set { _ivalue = value; }
    }

    public int value {
      get { return _ivalue.GetValue(); }
    }
  }
}
