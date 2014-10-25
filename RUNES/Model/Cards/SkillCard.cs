using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement SkillCard.
  public class SkillCard : SpellCard {
    public Effect dodge_effect;

    public override void Activate() {
      throw new NotImplementedException();
    }
  }
}
