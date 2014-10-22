using System.Collections;
using System;
using CARDScript.Compiler.Events;
using CARDScript.Compiler.Effects;
using CARDScript;
using CARDScript.Model;

/* Based on the WHEN statement, schedule a trigger condition
 * the effect_to_schedule in the correct list. 
  */
public class ScheduleEffect : Effect {
  protected GameEventListener listener;
  protected IGameController controller;

  public ScheduleEffect(GameEventListener listener) {
    this.listener = listener;
  }

  public override bool Activate() {
    // TODO(ticktakashi): Add the GameEventListener to the list of listeners
    controller.Schedule(listener);
    return base.Activate();
  }

  public override string ToString() {
    return listener.ToString();
  }
}