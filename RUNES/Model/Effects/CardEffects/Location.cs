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
    internal static CardCollection Resolve(Player player, Location location) {
        switch (location) {
          case (Location.HAND):
            return player.Hand;
          case (Location.COOL):
            return player.Cooldown;
          case (Location.DECK):
            return player.Deck;
          default:
            throw new RoRException("Cannot resolve location.");
        }
    }
  }
}
