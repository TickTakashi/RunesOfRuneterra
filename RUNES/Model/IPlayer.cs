using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
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

    // Stuns the player for n turns
    void Stun(int n);
    bool IsStunned();

    // Snares the player for n turns
    void Snare(int n);
    bool IsSnared();

    // Blinds the player for n turns
    void Blind(int n);
    bool IsBlind();

    // Slows the player for n turns 
    void Slow(int n);
    bool IsSlowed();

    // Silence the player for n turns
    void Silence(int n);
    bool IsSilenced();
  }
}
