using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Matchers {
  public abstract class Matcher<T> {
    public abstract bool Match(T e);
  }
}
