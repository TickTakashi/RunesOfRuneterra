using System.Collections;
using System;
using CARDScript.Compiler.Events;
using CARDScript.Compiler.Effects;
using CARDScript;

/* Based on the WHEN statement, schedule a trigger condition
 * the effect_to_schedule in the correct list. 
  */
public class ScheduleEffect : Effect {
  protected GameEventListener listener;

  public ScheduleEffect(GameEventListener listener) {
    this.listener = listener;
  }

  public override bool Activate() {
    // TODO(ticktakashi): Add the GameEventListener to the list of listeners
    // GameController.schedule(listener);
    Console.WriteLine("Triggering the listener 3 times with event type 50" + 
                      " and scalar value 100");
    GameEvent incorrect = new GameEvent();
    incorrect.event_type = 50;
    incorrect.scalar_value = 100;
    Console.WriteLine(listener.Trigger(incorrect));
    Console.WriteLine(listener.Trigger(incorrect));
    Console.WriteLine(listener.Trigger(incorrect));

    Console.WriteLine("Triggering the listener 3 times with event type " + 
                      "SHIELDS and scalar value 2");
    GameEvent still_incorrect = new GameEvent();
    still_incorrect.event_type = CARDScriptParser.SHIELDS;
    still_incorrect.scalar_value = 2;
    Console.WriteLine("Checking Trigger result: " +
                      listener.listening_for.Match(still_incorrect));
    Console.WriteLine(listener.Trigger(still_incorrect));
    Console.WriteLine(listener.Trigger(still_incorrect));
    Console.WriteLine(listener.Trigger(still_incorrect));

    Console.WriteLine("Triggering the listener 3 times with event type" +
                      " SHIELDS and scalar value 5");
    GameEvent correct = new GameEvent();
    correct.event_type = CARDScriptParser.SHIELDS;
    correct.scalar_value = 5;
    Console.WriteLine("Checking Trigger result: " +
                      listener.listening_for.Match(correct));
    Console.WriteLine(listener.Trigger(correct));
    Console.WriteLine(listener.Trigger(correct));
    Console.WriteLine(listener.Trigger(correct));
    
    return base.Activate();
  }

  public override string ToString() {
    return listener.ToString();
  }
}