using CARDScript.Compiler.Effects;
using CARDScript.Model.Cards.CardConditions;
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
    private CardCondition condition;
    private bool is_optional;

    public CardMoveEffect(Target choice_maker, IValue value, 
      Target debit_player, Location debit_location, Target credit_player,
      Location credit_location, CardCondition condition, bool is_optional) {
      this.choice_maker = choice_maker;
      this.value = value;
      this.debit_player = debit_player;
      this.debit_location = debit_location;
      this.credit_player = credit_player;
      this.credit_location = credit_location;
      this.condition = condition;
      this.is_optional = is_optional;
    }

    public override void Activate(GameCard card, Player user, Game game) {
      Player chooser = TargetMethods.Resolve(choice_maker, user, game);
      Player debiter = TargetMethods.Resolve(debit_player, user, game);
      Player crediter = TargetMethods.Resolve(credit_player, user, game);
      CardCollection debit = LocationMethods.Resolve(debiter, debit_location);
      CardCollection cred = LocationMethods.Resolve(crediter, credit_location);
      int num_cards = value.GetValue();
      List<GameCard> potentials = debit.CardsWhichSatisfy(condition.Condition);
      GameState prev = game.GetState();
      List<GameCard> selected = new List<GameCard>();
      
      DialogueCallback continue_effect = delegate() {
        foreach (GameCard crd in selected) {
          debit.Remove(crd);
          cred.Add(crd);
        }
        game.SetState(prev);
        base.Activate(card, user, game);
      };

      CardChoiceCallback callback = delegate(GameCard c) {
        if (c == null) {
          if (is_optional) {
            continue_effect();
          } else {
            throw new RoRException(
              "Pass shouldn't be called for non-optional choices!");
          }
        } else if (!selected.Contains(c)) {
          selected.Add(c);
          if (selected.Count == num_cards) {
            continue_effect();
          }
        }
      };

      ChooseCardState next = new ChooseCardState(chooser, potentials, callback,
        game, is_optional);

      if (potentials.Count >= num_cards) {
        game.SetState(next);
      } else {
        base.Activate(card, user, game);
      }
    }
  }
}
