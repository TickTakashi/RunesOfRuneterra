using CARDScript.Compiler.Effects;
using CARDScript.Model.Effects.ScalarEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Effects.CardEffects {
  class CardMoveEffect : Effect {
    private Target choice_maker;
    private IValue value;
    private Target debit_player;
    private Location debit_location;
    private Target credit_player;
    private Location credit_location;
    private string card_name;
    private bool is_optional;

    public CardMoveEffect(Target choice_maker, IValue value, 
      Target debit_player, Location debit_location, Target credit_player,
      Location credit_location, string card_name, bool is_optional) {
      this.choice_maker = choice_maker;
      this.value = value;
      this.debit_player = debit_player;
      this.debit_location = debit_location;
      this.credit_player = credit_player;
      this.credit_location = credit_location;
      this.card_name = card_name;
      this.is_optional = is_optional;
    }

    public override void Activate(Card card, Player user, Game game) {
      throw new NotImplementedException();
      base.Activate(card, user, game);
    }
  }
}
