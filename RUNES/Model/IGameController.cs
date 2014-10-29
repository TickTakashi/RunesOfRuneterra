using CARDScript.Compiler;
using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Events;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
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

    // Prompt player to negate this card. Also fires SkillshotHit and Miss events
    // This should be similar to FireEvent
    void PromptNegate(IPlayer user, Card card, Effect effect, EffectCallback callback);

    // Prompt the player to chose a place within distance range and move there
    void PromptMove(IPlayer user, int distance, EffectCallback callback);


  }

  public delegate void EffectCallback(Card card, IPlayer user, IGameController controller);
}
