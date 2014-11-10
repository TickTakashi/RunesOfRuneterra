using CARDScript.Compiler.Effects;
using CARDScript.Compiler.Matchers;
using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Events {
  public class GameEventListener {
    private IPlayer scheduling_user;
    private IGameController scheduling_game_controller;
    private Card scheduling_card;
    private Effect callback;
    private Matcher listening_for;
    private IValue count;
    private int count_down = 0;
    
    public GameEventListener(Matcher cond, Effect effect,
        IPlayer scheduling_user, IGameController scheduling_game_controller,
        Card scheduling_card, IValue count = null) {
      this.scheduling_card = scheduling_card;
      this.scheduling_user = scheduling_user;
      this.scheduling_game_controller = scheduling_game_controller;
      this.listening_for = cond;
      this.callback = effect;

      if (count == null) {
        this.count = new LiteralIntValue(1);
      } else {
        this.count = count;
      }
    }

    public bool Trigger(GameEvent e) {
      if (listening_for.Match(e, scheduling_game_controller, scheduling_user)) {
        // TODO(ticktakashi): Do I need to take into account the bool return of
        //                    this activation?
        callback.Activate(scheduling_card, scheduling_user,
          scheduling_game_controller);
        count_down++;
        return (count.GetValue() - count_down) <= 0;
      }
      return false;
    }
  }
}
