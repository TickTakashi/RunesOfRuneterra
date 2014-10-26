using CARDScript.Compiler;
using CARDScript.Compiler.Events;
using CARDScript.Model.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  /* The IGameController Interface
   * 
   * An interface which describes the game control actions that must occur in
   * RoR. Provides methods to manipulate Players and check properties about the
   * games state at any one time.
   */
  public interface IGameController {

    // Fires a game event.
    void FireEvent(GameEvent game_event);

    // Schedules a GameEventListener to listen for fired events.
    void Schedule(GameEventListener listener);

    // Returns the opponent of p
    IPlayer Opponent(IPlayer p);

    // Returns true if this player is within range distance of opponent
    bool InRange(IPlayer user, int range);

    // Starts the current players turn by drawing a card etc.
    void StartTurn();

    // Ends the current players turn and starts the next players turn.
    void EndTurn();

    // Whos turn is it?
    IPlayer Current();

    // Prompt player to negate this card. Also fires SkillshotHit and Miss events
    // This should be similar to FireEvent
    bool PromptNegate(IPlayer user, Card card);

    // Prompt the player to chose a place within distance range and move there
    void PromptMove(IPlayer user, int distance);
  }
}
