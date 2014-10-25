using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  // TODO(ticktakashi): Implement SelfCard.
  public class SelfCard : Card {
    protected int time;

    public override void Activate() {
      throw new NotImplementedException();
    }
  }
}
