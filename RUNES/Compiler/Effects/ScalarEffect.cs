using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects.ScalarEffects {
  public abstract class ScalarEffect : Effect {
    private IValue _ivalue;

    public IValue ivalue {
      get { return _ivalue; }
      set { _ivalue = value; }
    }

    public int value {
      get { return _ivalue.GetValue(); }
    }

    // TODO: Surely all effects need this?
    private int _effect_id;

    public int effect_id {
      get { return _effect_id; }
      set { _effect_id = value; }
    }
  }
}
