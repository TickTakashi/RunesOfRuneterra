using CARDScript.Model.BuffEffects;
using CARDScript.Model.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public class Player : RoRObservable<PlayerEvent>, IGameToken {
    internal static readonly int MAX_HEALTH = 20;
    internal static readonly int MAX_AP = 3;
    internal static readonly int BASIC_ATTACK_DAMAGE;
    internal static readonly int BASIC_ATTACK_COST;
    internal static readonly int BASIC_ATTACK_RANGE;
    private int health;
    private int action_points;
    private List<StatModifier> modifiers;
    private List<CC> cc;
    private List<ActiveBuff> buffs;
    private CardCollection deck;
    private CardCollection hand;
    private CardCollection cooldown;
    private Passive current_passive;
    private Game game;

    internal Player(CardCollection deck, Game game) {
      this.deck = deck;
      this.hand = new CardCollection();
      this.cooldown = new CardCollection();
      this.game = game;
      this.health = MAX_HEALTH;
      this.action_points = MAX_AP;
      this.cc = new List<CC>();
      this.modifiers = new List<StatModifier>();
    }

    internal void SetPassive(Passive passive) {
      this.current_passive = passive;
      // TODO(ticktakashi): Do we need to fire a game event here?
    }

    internal void ShuffleDeck() {
      deck.Shuffle();
    }

    internal void Draw() {
      Card to_hand = deck.RemoveFirst();
      if (to_hand == null)
        NotifyAll(new PlayerEvent(PlayerEvent.Type.DECK_OUT, this));
      else
        hand.Add(to_hand);
    }

    internal void Discard(Card card) {
      hand.Remove(card);
      cooldown.Add(card);
    }

    internal void Damage(int damage) {
      foreach (Buff b in buffs) {
        damage = b.ModifyDamage(damage, this, game);
      }

      health -= damage;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.DAMAGE, this, damage));
      if (health <= 0) {
        NotifyAll(new PlayerEvent(PlayerEvent.Type.DEAD, this));
      }
    }

    internal void Heal(int strength) {
      foreach (Buff b in buffs) {
        strength = b.ModifyHeal(strength, this, game);
      }

      health += strength;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.HEAL, this, strength));
      if (health > MAX_HEALTH) {
        health = MAX_HEALTH;
        NotifyAll(new PlayerEvent(PlayerEvent.Type.OVERHEAL, this));
      }
    }

    internal void ResetHealth() {
      health = MAX_HEALTH;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.HEAL, this));
    }

    internal void ResetActionPoints() {
      action_points = MAX_AP;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.AP_GAIN, this));
    }

    private void Spend(int amount) {
      action_points -= amount;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.SPEND, this, amount));
      if (action_points < 0)
        throw new RoRException("Action points are below zero!");
      else if (action_points == 0)
        NotifyAll(new PlayerEvent(PlayerEvent.Type.AP_OUT, this));
    }

    internal void ModifyAP(int delta) {
      action_points += delta;
    }

    internal bool IsDead() {
      return health <= 0;
    }

    internal List<Card> HandCardsWhichMeetCriteria(CardCondition criteria) {
      return hand.CardsWhichSatisfy(criteria);
    }

    internal bool IsTurn() {
      return game.IsTurn(this);
    }

    internal bool HasAP(int amount) {
      return action_points >= amount;
    }

    internal int MovementCost() {
      // TODO(ticktakashi): Implement slows.
      return 1;
    }

    internal void NormalMove(int destination) {
      game.SetPosition(this, destination);
    }

    internal bool CanActivate(Card card) {
      return IsTurn() && HasAP(card.Cost(this, game)) && 
        card.CanActivate(this, game) && hand.Contains(card);
    }

    internal void PlayCard(Card card) {
      if (CanActivate(card)) {
        Spend(card.Cost(this, game));
        card.Activate(this, game);
      }
    }

    internal bool CanMelee() {
      return !IsCCd(CCType.BLIND) && !IsCCd(CCType.STUN);
    }

    internal void BasicAttack(Player target) {
      Spend(BASIC_ATTACK_COST);
      target.Damage(MeleeDamage());
    }

    internal bool IsCCd(CCType type) {
      foreach (CC c in cc) {
        if (c.type == type) {
          return true;
        }
      }
      return false;
    }

    internal void ApplyCC(CCType type, int duration) {
      cc.Add(new CC(type, duration));
    }

    internal void ApplyBuff(ActiveBuff buff) {
      this.buffs.Add(buff);
    }

    internal void TickAllBuffs() {
      for (int i = buffs.Count; i > 0; --i) {
        if (buffs[i].Tick())
          buffs.RemoveAt(i);
      }
    }

    internal bool IsStatModified(StatType stat) {
      foreach (StatModifier sm in modifiers) {
        if (sm.type == stat) {
          return true;
        }
      }
      return false;
    }

    internal void ApplyStatModifier(StatType type, int strength, int time) {
      modifiers.Add(new StatModifier(type, strength, time));
    }

    private int TotalStrength(StatType stat) {
      int acc = 0;
      foreach (StatModifier sm in modifiers) {
        if (sm.type == stat)
          acc += sm.strength;
      }
      return acc;
    } 

    internal int MeleeDamage() {
      return BASIC_ATTACK_DAMAGE + TotalStrength(StatType.MELEE_DAMAGE);
    }

    internal int MeleeRange() {
      return BASIC_ATTACK_RANGE + TotalStrength(StatType.MELEE_RANGE);
    }

    internal int BonusSkillDamage() {
      return TotalStrength(StatType.SKILL_DAMAGE);
    }

    internal int BonusSkillRange() {
      return TotalStrength(StatType.SKILL_RANGE);
    }

    internal int BonusSpellDamage() {
      return TotalStrength(StatType.SPELL_DAMAGE);
    }

    internal int BonusSpellRange() {
      return TotalStrength(StatType.SPELL_RANGE);
    }
  }

  internal enum CCType {
    STUN,
    SNARE,
    SILENCE,
    SLOW,
    BLIND
  }

  internal struct CC {
    internal CCType type;
    internal int duration;

    internal CC(CCType type, int duration) {
      this.type = type;
      this.duration = duration;
    }
  }

  internal class ActiveBuff : Buff {
    int duration;

    internal ActiveBuff(Buff b, int duration) {
      this.duration = duration;
      this.Next = b;
    }

    internal bool Tick() {
      return --duration <= 0;
    }
  }

  internal enum StatType {
    MELEE_DAMAGE,
    MELEE_RANGE,
    SKILL_DAMAGE,
    SKILL_RANGE,
    SPELL_DAMAGE,
    SPELL_RANGE,
  }

  internal struct StatModifier {
    internal StatType type;
    internal int strength;
    internal int duration;

    internal StatModifier(StatType type, int strength, int duration) {
      this.type = type;
      this.strength = strength;
      this.duration = duration;
    }
  }

  public struct PlayerEvent {
    public enum Type {
      DAMAGE,
      HEAL,
      OVERHEAL,
      SPEND,
      DECK_OUT,
      DEAD,
      AP_GAIN,
      AP_OUT,
      SKILLSHOT_HIT,
      MELEE_HIT,
    }

    public Type type;
    public Player player;
    public int scalar;

    public PlayerEvent(Type type, Player player, int scalar = -1) {
      this.type = type;
      this.player = player;
      this.scalar = scalar;
    }
  }
}
