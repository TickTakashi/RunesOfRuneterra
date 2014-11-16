using CARDScript.Compiler.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public abstract class Card {
    protected string _name;
    public string Name { get { return _name; } }

    protected int _id;
    public int ID { get { return _id; } }

    protected bool _is_ult;
    public bool IsUltimate { get { return _is_ult; } }

    protected bool _is_dash;
    public bool IsDash { get { return _is_dash; } }

    protected int _dash_distance;
    public int DashDistance { get { return _dash_distance; } }

    protected int _cost;
    public int Cost(Player user, Game game) {
      // TODO(ticktakashi): Factor in player buffs
      return _cost;
    }

    protected int _limit;
    public int Limit { get { return _limit; } }

    public Effect effect;

    public bool CanActivate(Player user, Game game) {
      return (!user.IsCCd(CCType.SNARE) && !user.IsCCd(CCType.SILENCE) &&
        !user.IsCCd(CCType.STUN) && IsDash) ||
        effect.CanActivate(this, user, game);
    }

    public void Activate(Player user, Game game) {
      if (IsDash) {
        IGameState old = game.GetState();
        game.SetState(new ChooseLocationState(user, DashDistance,
           delegate() {
             game.SetState(old);
             effect.Activate(this, user, game);
           },
           game));
      } else {
        effect.Activate(this, user, game);
      }
    }

    public bool CanNegate(Player user, Game game, NormalEffect e) {
      return effect.CanNegate(this, user, game, e);
    }

    public abstract NormalEffect CreateEffect();
  }

  public delegate bool CardCondition(Card card);
  public delegate void CardChoiceCallback(Card card);

  public abstract class DamageCard : Card {
    protected int _damage;
    public virtual int Damage(Player user, Game game) {
      return _damage;
    }

    protected int _range;
    public virtual int Range(Player user, Game game) {
      return _range;
    }
  }

  public class SkillCard : DamageCard {
    public override int Damage(Player user, Game game) {
      return base.Damage(user, game) + user.BonusSkillDamage();
    }

    public override int Range(Player user, Game game) {
      return base.Range(user, game) + user.BonusSkillRange();
    }

    public override NormalEffect CreateEffect() {
      return new SkillshotEffect(this);
    }
  }

  public class SpellCard : DamageCard {
    public override int Damage(Player user, Game game) {
      return base.Damage(user, game) + user.BonusSpellDamage();
    }

    public override int Range(Player user, Game game) {
      return base.Range(user, game) + user.BonusSpellRange();
    }

    public override NormalEffect CreateEffect() {
      return new DamageEffect(this);
    }
  }

  public class MeleeCard : DamageCard {
    public override int Damage(Player user, Game game) {
      return user.MeleeDamage() + base.Damage(user, game);
    }

    public override int Range(Player user, Game game) {
      return user.MeleeRange();
    }

    public override NormalEffect CreateEffect() {
      return new MeleeEffect(this);
    }
  }

  public class BuffCard : Card {
    private int _time;
    public int Time(Player user, Game game) {
      return _time;
    }

    private BuffEffect _buff;
    public BuffEffect Buff { get { return _buff; } }

    public override NormalEffect CreateEffect() {
      return new BuffActivationEffect(this);
    }
  }
}
