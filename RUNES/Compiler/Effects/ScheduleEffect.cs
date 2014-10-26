using System.Collections;
using System;
using CARDScript.Compiler.Events;
using CARDScript.Compiler.Effects;
using CARDScript;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Compiler.EventMatchers;
using CARDScript.Compiler;

/* Based on the WHEN statement, schedule a trigger condition
 * the effect_to_schedule in the correct list. 
  */
public class ScheduleEffect : Effect {
  private Effect callback;
  private EventMatcher listening_for;
  private IValue repeat;

  public ScheduleEffect(EventMatcher listening_for, Effect callback,
      IValue repeat = null) {
    this.callback = callback;
    this.listening_for = listening_for;
    this.repeat = repeat;
  }

  public override bool Activate(Card source, IPlayer user,
      IGameController game_controller) {
    // TODO(ticktakashi): Add the GameEventListener to the list of listeners
    game_controller.Schedule(new GameEventListener(listening_for, callback,
      user, game_controller, source, repeat));
    return base.Activate(source, user, game_controller);
  }

  public override string ToString() {
    return "the next time " + listening_for + ": " + callback;
  }
}