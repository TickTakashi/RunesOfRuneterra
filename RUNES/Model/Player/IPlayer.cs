﻿using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Players {
  /* The IPlayer Interface
   * 
   * A interface that describes the behaviour that a Player must provide.
   */
  public interface IPlayer {
    // Decrease your health by value - Fires a Damage Taken Event
    void Damage(int value);

    // Increase your health by value
    void Heal(int value);
    
    // Draw a card from your deck
    void Draw(int value);

    // Discard a card from your had at index
    void Discard(int index);

    // Has at least n Action Points
    bool HasActionPoints(int n);

    // Returns the number of action points this player has
    int ActionPoints();

    // Spends n Action Points
    void Spend(int n);

    // Refreshes Action Points, setting to the max value
    void RefreshActionPoints();

    // Add a buff to the player
    void BuffPlayer(BuffType type, int strength);

    // Check for CC
    bool HasBuff(BuffType type);

    // Get the melee range of this player, after modification.
    int GetMeleeRange();
  }
}