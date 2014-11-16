using CARDScript.Model;
using CARDScript.Model.Cards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler.Effects {
  public abstract class NormalEffect : Effect {
    public override bool ContainsNormalEffect() {
      return true;
    }
  }

  public class DamageEffect : NormalEffect {
    protected DamageCard source;

    public DamageEffect(DamageCard source) {
      this.source = source;
    }

    public override void Activate(Card card, Player user, Game game) {
      if (InRange(user, game)) 
        game.Opponent(user).Damage(source.Damage(user, game));

      // TODO(ticktakashi): Implement arbitrary negation for spellshields.

      base.Activate(card, user, game);
    }

    protected bool InRange(Player user, Game game) {
      return game.Distance(user, game.Opponent(user)) < 
        source.Range(user, game);
    }

    public override bool CanActivate(Card card, Player user, Game game) {
      return InRange(user, game) && !user.IsCCd(CCType.STUN) &&
        !user.IsCCd(CCType.SILENCE) && base.CanActivate(card, user, game);
    }

    public override string ToString() {
      if (Next != null)
        return Next.ToString();
      else
        return "";
    }
  }

  public class SkillshotEffect : DamageEffect {
    public SkillshotEffect(SkillCard card) : base(card) {}

    public override void Activate(Card card, Player user, Game game) {
      if (InRange(user, game)) {
        // Allow the opponent to dodge this card
        Player opponent = game.Opponent(user);
        CardCondition isdash = delegate(Card c) {
          return c.IsDash;
        };
        List<Card> dodge_cards = opponent.HandCardsWhichMeetCriteria(isdash);

        CardChoiceCallback callback = delegate(Card c) {
          if (c != null) {
            opponent.Discard(c);
          } else {
            base.Activate(card, user, game);
            user.NotifyAll(new PlayerEvent(PlayerEvent.Type.SKILLSHOT_HIT, 
              user));
          }
        };
        game.SetState(new ChooseCardState(opponent, dodge_cards, callback,
                                          game));
      } else {
        base.Activate(card, user, game);
      }
    }
  }

  public class MeleeEffect : DamageEffect {
    public MeleeEffect(MeleeCard source) : base(source) {}

    // For "This Counts as Melee" situations
    public MeleeEffect(DamageCard source) : base(source) {}
    
    public override bool CanActivate(Card card, Player user, Game game) {
      return !user.IsCCd(CCType.BLIND) && base.CanActivate(card, user, game);
    }
  }

  public class BuffActivationEffect : NormalEffect {
    private BuffCard source;

    public BuffActivationEffect(BuffCard source) {
      this.source = source;
    }

    public override void Activate(Card card, Player user, Game game) {
      source.Buff.Activate(source.Time(user, game), card, user, game);
      base.Activate(card, user, game);
    }

    public override bool CanActivate(Card card, Player user, Game game) {
      return source.Buff.CanActivate(
        source.Time(user, game), card, user, game);
    }
  }
}