using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.CardEffects {
  internal enum Location {
    HAND,
    DECK,
    COOL,
  }

  internal static class LocationMethods {
    internal static CardCollection Resolve(Player player, Game game, 
      Location location) {
        throw new NotImplementedException();
    }
  }
}
