using System.Collections;
using System;
using RUNES.Runes.Model.Effects;

/* Based on the WHEN statement, schedule a trigger condition
 * the effect_to_schedule in the correct list. 
  */
public class ScheduleEffect : Effect {
  GameEventListener listener;

  public ScheduleEffect(EventMatcher cond, Effect effect) {
    this.listener = new GameEventListener(cond, effect);
  }

  public override bool Activate() {
    // TODO(ticktakashi): Add the GameEventListener to the list of event registered
    // event listeners
    // GameController.schedule(listener);
    Console.WriteLine("Triggering the listener 3 times with event type 50 and scalar value 100");
    GameEvent incorrect = new GameEvent();
    incorrect.event_type = 50;
    incorrect.scalar_value = 100;
    Console.WriteLine(listener.Trigger(incorrect));
    Console.WriteLine(listener.Trigger(incorrect));
    Console.WriteLine(listener.Trigger(incorrect));

    Console.WriteLine("Triggering the listener 3 times with event type SHIELDS and scalar value 2");
    GameEvent still_incorrect = new GameEvent();
    still_incorrect.event_type = RUNES.Runes.RunesParser.SHIELDS;
    still_incorrect.scalar_value = 2;
    Console.WriteLine(listener.Trigger(still_incorrect));
    Console.WriteLine(listener.Trigger(still_incorrect));
    Console.WriteLine(listener.Trigger(still_incorrect));

    return base.Activate();
  }

  // TODO(ticktakashi): Remove Debug ToString Method.
  public override string ToString() {
    return "ScheduleEffect, contains this: " + listener;
  }
}