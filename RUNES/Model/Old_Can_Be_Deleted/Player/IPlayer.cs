using CARDScript.Compiler.Effects;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
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
    void BuffPlayer(IPlayer caster, BuffType type, int duration);
    void BuffPlayer(IPlayer caster, BuffType type, int duration, int strength);

    // Check for CC
    bool HasBuff(BuffType type);

    // Get the melee range of this player, after modification.
    int GetMeleeRange();

    // Get the movement range of this player, after modification.
    int GetMovementRange();

    // Get the cost of each space moved for this player, after modification.
    int GetMovementCost();

    // Prompt this player to chose to move up to distance steps.
    void PromptMove(Card_OLD card, int distance, EffectCallback callback);

    // Prompt this player to negate this card effect.
    void PromptNegate(Card_OLD card, NormalEffect effect, EffectCallback callback);
    
    // Get the current health of this player
    int GetHealth();

    // Can this player melee attack?
    bool CanMelee();

    // How much damage does this player do with basic attacks?
    int GetMeleeDamage();

    // Basic attacks the opponent
    void MeleeAttack();
    
    // Add a card to the deck
    void AddToDeck(string s);

    // Add a card to the hand
    void AddToHand(string s);

    // Add a card to cooldown
    void AddToCooldown(string s);

    // How much additional skill damage does this player deal?
    int GetSkillDamage();

    // How much additional spell damage does this player deal?
    int GetSpellDamage();

    // Is this player dead?
    bool Dead(); 
  }
}
