using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.ScalarEffects {
  internal abstract class ScalarEffect : Effect {
    protected IValue value;
    internal Target _target;
    internal Target Target { get { return _target; } }

    internal ScalarEffect(Target target, IValue value) {
      this.value = value;
      this._target = target;
    }
  }
}
