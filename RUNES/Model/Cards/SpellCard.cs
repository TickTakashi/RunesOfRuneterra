using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement DamageCard.
  public class SpellCard : Card {
    protected int damage;
    protected int range;

    public override void Activate() {
      throw new NotImplementedException();
    }
  }
}
