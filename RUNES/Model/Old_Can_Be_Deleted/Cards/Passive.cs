using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement Passives.
  public class Passive {
    string name;
    int id;
    Effect passive_effect;

    public Passive(string name, int id, Effect passive_effect) {
      this.passive_effect = passive_effect;
      this.name = name;
      this.id = id;
    }
  }
}
