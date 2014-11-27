using CARDScript.Model.BuffEffects;
using CARDScript.Model.Buffs;
using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public class Player : RoRObservable<PlayerEvent>, IGameToken {
    public static readonly int MAX_HEALTH = 20;
    public static readonly int MAX_AP = 3;
    public static readonly int BASIC_ATTACK_DAMAGE = 1;
    public static readonly int BASIC_ATTACK_COST = 1;
    public static readonly int BASIC_ATTACK_RANGE = 1;
    public static readonly int MOVE_COST = 1;
    public static readonly int SLOW_MOVE_COST = 2;
    public static readonly int MAX_CHANNEL_SLOTS = 3;
    private int _health;
    public int Health { get { return _health; } }
    private int _action_points;
    public int ActionPoints { get { return _action_points; } }
    private List<CC> cc;
    private List<ActiveBuff> buffs;

    private CardCollection _deck;
    public CardCollection Deck { get { return _deck; } }

    private CardCollection _hand;
    public CardCollection Hand { get { return _hand; } }

    private CardCollection _cooldown;
    public CardCollection Cooldown { get { return _cooldown; } }

    private List<PassiveCard> _passive_deck;
    public List<PassiveCard> PassiveDeck { get { return _passive_deck; } }

    private PassiveCard _current_passive;
    public PassiveCard CurrenPassive { get { return _current_passive; } }

    private Channel[] _channel_slots;
    public Channel[] ChannelSlots { get { return _channel_slots; } }

    private Game _game;
    public Game Game { get { return _game; } }

    internal Player(List<GameCard> deck, 
     List<PassiveCard> passive_deck, Game game) {
      this._deck = new CardCollection(deck);
      this._hand = new CardCollection();
      this._cooldown = new CardCollection();
      this._game = game;
      this._health = MAX_HEALTH;
      this._action_points = MAX_AP;
      this._passive_deck = passive_deck;
      this.cc = new List<CC>();
      this.buffs = new List<ActiveBuff>();
      this._channel_slots = new Channel[MAX_CHANNEL_SLOTS];
    }

    internal void SetPassive(PassiveCard passive) {
      this._current_passive = passive;
      buffs.Add(new ActiveBuff(passive, passive.Buff));
    }

    internal void ShuffleDeck() {
      _deck.Shuffle();
    }

    internal void Draw() {
      GameCard to_hand = _deck.RemoveFirst();
      if (to_hand == null)
        NotifyAll(new PlayerEvent(PlayerEvent.Type.DECK_OUT, this));
      else {
        _hand.Add(to_hand);
        NotifyAll(new PlayerEvent(PlayerEvent.Type.DRAW, this, 1));
      }
    }

    internal void Draw(int amount) {
      for (int i = 0; i < amount; i++) {
        Draw();
      }
      NotifyAll(new PlayerEvent(PlayerEvent.Type.DRAW, this, amount));
    }

    internal void Discard(GameCard card) {
      _hand.Remove(card);
      _cooldown.Add(card);
    }

    internal void Damage(int damage) {
      foreach (Buff b in buffs) {
        damage = b.ModifyDamage(damage, this, _game);
      }

      _health -= damage;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.DAMAGE, this, damage));
      if (_health <= 0) {
        NotifyAll(new PlayerEvent(PlayerEvent.Type.DEAD, this));
      }
    }

    internal void Heal(int strength) {
      foreach (Buff b in buffs) {
        strength = b.ModifyHeal(strength, this, _game);
      }

      _health += strength;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.HEAL, this, strength));
      if (_health > MAX_HEALTH) {
        _health = MAX_HEALTH;
        NotifyAll(new PlayerEvent(PlayerEvent.Type.OVERHEAL, this));
      }
    }

    internal void ResetHealth() {
      _health = MAX_HEALTH;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.HEAL, this));
    }

    internal void ResetActionPoints() {
      _action_points = MAX_AP;

      foreach (Buff b in buffs) {
        _action_points = b.ModifyActionPoints(_action_points, this, _game);
      }

      NotifyAll(new PlayerEvent(PlayerEvent.Type.AP_SET, this));
    }

    private void Spend(int amount) {
      _action_points -= amount;
      NotifyAll(new PlayerEvent(PlayerEvent.Type.SPEND, this, amount));
      if (_action_points < 0)
        throw new RoRException("Action points are below zero!");
      else if (_action_points == 0)
        NotifyAll(new PlayerEvent(PlayerEvent.Type.AP_OUT, this));
    }

    internal bool IsDead() {
      return _health <= 0;
    }

    internal bool IsTurn() {
      return _game.IsTurn(this);
    }

    internal bool HasAP(int amount) {
      return _action_points >= amount;
    }

    internal int MovementCost() {
      return IsCCd(CCType.SLOW) ? SLOW_MOVE_COST : MOVE_COST;
    }

    internal void NormalMove(int destination) {
      _game.SetPosition(this, destination);
      Spend(MovementCost() * _game.Distance(this, destination));
    }

    internal bool CanActivate(GameCard card) {
      return IsTurn() && HasAP(card.Cost(this, _game)) && 
        card.CanActivate(this, _game) && _hand.Contains(card);
    }

    internal void PlayCard(GameCard card) {
      if (CanActivate(card)) {
        Spend(card.Cost(this, _game));
        card.Activate(this, _game);
      }
    }

    internal int CardCost(GameCard card, int base_cost) {
      int cost = base_cost;
      foreach (Buff b in buffs) {
        cost = b.ModifyCardCost(card, cost, this, _game);
      }
      return cost;
    }

    internal bool CanMelee() {
      return !IsCCd(CCType.BLIND);
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

    internal void ApplyBuff(Card source, Buff buff) {
      this.buffs.Add(new ActiveBuff(source, buff));
    }

    internal void CheckAllBuffs() {
      for (int i = buffs.Count; i > 0; --i) {
        if (buffs[i].IsFinished())
          buffs.RemoveAt(i);
      }
    }

    internal int MeleeDamage() {
      int damage = BASIC_ATTACK_DAMAGE;

      foreach (Buff b in buffs) {
        damage = b.ModifyMeleeDamage(damage, this, _game);
      }

      return damage;
    }

    internal int MeleeRange() {
      int range = BASIC_ATTACK_RANGE;

      foreach (Buff b in buffs) {
        range = b.ModifyMeleeRange(range, this, _game);
      }

      return range;
    }

    internal int BonusSkillDamage() {
      int bonus_damage = 0;

      foreach (Buff b in buffs) {
        bonus_damage = b.ModifySkillDamage(bonus_damage, this, _game);
      }

      return bonus_damage;
    }

    internal int BonusSkillRange() {
      int bonus_range = 0;

      foreach (Buff b in buffs) {
        bonus_range = b.ModifySkillRange(bonus_range, this, _game);
      }

      return bonus_range;
    }

    internal int BonusSpellDamage() {
      int bonus_damage = 0;

      foreach (Buff b in buffs) {
        bonus_damage = b.ModifySpellDamage(bonus_damage, this, _game);
      }

      return bonus_damage;
    }

    internal int BonusSpellRange() {
      int bonus_range = 0;

      foreach (Buff b in buffs) {
        bonus_range = b.ModifySpellRange(bonus_range, this, _game);
      }

      return bonus_range;
    }

    internal bool OwnsChannel(Channel c) {
      for (int i = 0; i < _channel_slots.Length; i++) {
        if (_channel_slots[i] == c)
          return true;
      }

      return false;
    }

    internal void BeginChannel(GameCard c) {
      if (EmptyChannelSlots() == 0) {
        throw new RoRException("No Channel Slots Remaining!");
      } else {
        Channel channel = new Channel(c);
        for (int i = 0; i < _channel_slots.Length; i++) {
          if (_channel_slots[i] == null) {
            _channel_slots[i] = channel;
            return;
          }
        }
      }
    }

    internal void ActivateChannel(Channel c) {
      for (int i = 0; i < _channel_slots.Length; i++) {
        if (_channel_slots[i] == c) {
          c.Activate(this, _game);
          _channel_slots[i] = null;
          return;
        }
      }
    }

    internal int EmptyChannelSlots() {
      int count = 0;
      for (int i = 0; i < _channel_slots.Length; i++) {
        if (_channel_slots[i] == null) {
          count++;
        }
      }
      return count;
    }

    internal void TickChannels() {
      for (int i = 0; i < _channel_slots.Length; i++) {
        _channel_slots[i].Tick(this, _game);
      }
    }
  }

  internal enum CCType {
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

  public struct PlayerEvent {
    public enum Type {
      DAMAGE,
      HEAL,
      OVERHEAL,
      SPEND,
      DECK_OUT,
      DEAD,
      AP_SET,
      AP_OUT,
      SKILLSHOT_HIT,
      MELEE_HIT,
      DRAW,
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
