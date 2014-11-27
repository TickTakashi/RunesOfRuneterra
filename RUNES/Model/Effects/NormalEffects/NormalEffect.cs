using CARDScript.Model;
using CARDScript.Model.Buffs;
using CARDScript.Model.Cards;
using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  internal abstract class NormalEffect : Effect {
    internal override bool ContainsNormalEffect() {
      return true;
    }
  }

  internal class DamageEffect : NormalEffect {
    internal override void Activate(GameCard card, Player user, Game game) {
      if (card is DamageCard) {
        DamageCard source = (DamageCard)card;

        if (InRange(card, user, game))
          game.Opponent(user).Damage(source.Damage(user, game));

        // TODO(ticktakashi): Implement arbitrary negation for spellshields.
      }
      base.Activate(card, user, game);
    }

    protected bool InRange(GameCard card, Player user, Game game) {
      return card is DamageCard && game.Distance(user, game.Opponent(user)) < 
        ((DamageCard) card).Range(user, game);
    }

    internal override bool CanActivate(GameCard card, Player user, Game game) {
      return InRange(card, user, game) && !user.IsCCd(CCType.SILENCE) &&
        base.CanActivate(card, user, game);
    }

    public override string ToString() {
      if (Next != null)
        return Next.ToString();
      else
        return "";
    }
  }

  internal class SkillshotEffect : DamageEffect {
    internal override void Activate(GameCard card, Player user, Game game) {
      Player opponent = game.Opponent(user);
      GameCardCondition isdash = new DashCondition();
      List<GameCard> dodge_cards = opponent.Hand.CardsWhichSatisfy(isdash);
      if (InRange(card, user, game) && dodge_cards.Count > 0) {
        CardChoiceCallback callback = delegate(GameCard c) {
          if (c != null) {
            opponent.Discard(c);
          } else {
            base.Activate(card, user, game);
            user.NotifyAll(new PlayerEvent(PlayerEvent.Type.SKILLSHOT_HIT, 
              user));
          }
        };
        game.SetState(new ChooseCardState(opponent, dodge_cards, callback,
          game, true));
      } else {
        base.Activate(card, user, game);
      }
    }
  }

  internal class MeleeEffect : DamageEffect {
    internal override void Activate(GameCard card, Player user, Game game) {
      base.Activate(card, user, game);
      user.NotifyAll(new PlayerEvent(PlayerEvent.Type.MELEE_HIT,
        user));
    }
    
    internal override bool CanActivate(GameCard card, Player user, Game game) {
      return !user.CanMelee() && base.CanActivate(card, user, game);
    }
  }

  internal class BuffEffect : NormalEffect {
    internal override void Activate(GameCard card, Player user, Game game) {
      if (card is BuffCard) {
        BuffCard source = (BuffCard)card;
        TimedBuff timed = new TimedBuff(user, game, source.Buff, 
          source.Time(user, game));
        timed.Apply(card, user, game); 
      }
      base.Activate(card, user, game);
    }
  }
}