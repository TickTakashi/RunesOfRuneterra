using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  /* The Game class
   * 
   * A game of RunesOfRuneterra. It uses the observer pattern to allow for an
   * MVC architecture, decoupling RoR from any specific C# based game engine.
   * Also uses the State pattern to control what can be manipulated at any 
   * given time.
   * 
   * Members:
   *    field - an array of Lists of Game Tokens, since more than one token
   *      can be on any slot of the field at any time.
   *    
   *    player_1 & player_2 - The actual players of the game.
   *    
   *    round_number - the current round number.
   */
  public class Game : RoRObservable<GameEvent>, IRoRObserver<PlayerEvent> {
    internal static readonly Random rng = new Random();
    internal static int FIELD_SIZE = 7;
    internal static int MAX_ROUNDS = 3;
    internal static int STARTING_HAND_SIZE = 4;

    private List<IGameToken>[] field;

    private Player _player_1;
    public Player Player1 { get { return _player_1; } }

    private Player _player_2;
    public Player Player2 { get { return _player_2; } }

    private GameState state;

    private int round_num;

    private int player_1_wins;
    private int player_2_wins;

    public Game(List<GameCard> deck_1, List<PassiveCard> p_deck_1, 
      List<GameCard> deck_2, List<PassiveCard> p_deck_2) {
      Player player_1 = new Player(deck_1, p_deck_1, this);
      Player player_2 = new Player(deck_2, p_deck_2, this);
      round_num = 0;

      field = new List<IGameToken>[FIELD_SIZE];
      for (int i = 0; i < field.Length; i++) {
        field[i] = new List<IGameToken>();
      }

      this._player_1 = player_1;
      this._player_1.Attach(this);
      this._player_2 = player_2;
      this._player_2.Attach(this);

      field[FIELD_SIZE / 2 - 1].Add(player_1);
      field[FIELD_SIZE / 2 + 1].Add(player_2);
    }

    public void BeginGame() {
      NotifyAll(new GameEvent(GameEvent.Type.GAME_BEGIN, this));
      StartRound();
    }

    private void StartRound() {
      round_num++;
      NotifyAll(new GameEvent(GameEvent.Type.ROUND_START, this));
      _player_1.ResetHealth();
      _player_2.ResetHealth();

      for (int i = 0; i < STARTING_HAND_SIZE; i++) {
        _player_1.Draw();
        _player_2.Draw();
      }

      Player second_player = round_num % 2 == 0 ? _player_1 : _player_2;
      SetState(new ChoosePassivesState(second_player, this));
    }

    private void EndRound() {
      Player round_winner = null;

      if (!_player_1.IsDead()) {
        round_winner = _player_1;
        player_1_wins++;
      }

      if (!_player_2.IsDead()) {
        round_winner = _player_2;
        player_2_wins++;
      }

      NotifyAll(new GameEvent(GameEvent.Type.ROUND_END, this, round_winner));

      if (round_num == MAX_ROUNDS || player_1_wins == (1 + MAX_ROUNDS / 2) ||
        player_2_wins == (1 + MAX_ROUNDS / 2)) {
        EndGame();
      } else {
        StartRound();
      }
    }

    public void Notify(PlayerEvent change_event) {
      if (change_event.type == PlayerEvent.Type.DEAD)
        EndRound();
      else if (change_event.type == PlayerEvent.Type.AP_OUT)
        Pass(change_event.player);
    }

    internal GameState GetState() {
      return state;
    }

    internal void SetState(GameState new_state) {
      state = new_state;
    }

    internal Player Opponent(Player a) {
      return a == _player_1 ? _player_2 : _player_1;
    }

    internal int GetPosition(Player p) {
      for (int i = 0; i < field.Length; i++) {
        if (field[i].Contains(p)) {
          return i;
        }
      }

      throw new RoRException("Player is not on the field!");
    }

    internal void SetPosition(Player p, int position) {
      int cur = GetPosition(p);
      field[position].Add(p);
      field[cur].Remove(p);
      NotifyAll(new GameEvent(GameEvent.Type.MOVED, this, p));
    }

    internal int Distance(int a, int b) {
      return Math.Abs(a - b);
    }

    internal int Distance(Player p, int location) {
      return Distance(GetPosition(p), location);
    }

    internal int Distance(Player p, Player q) {
      return Distance(p, GetPosition(q));
    }

    private void EndGame() {
      Player winner = null;
      winner = player_2_wins > player_1_wins ? _player_2 : winner;
      winner = player_1_wins > player_2_wins ? _player_1 : winner;
      NotifyAll(new GameEvent(GameEvent.Type.GAME_OVER, this, winner));
    }

    // Stateful methods - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public bool IsTurn(Player p) {
      return state.IsTurn(p);
    }

    public void SetPassive(Player p, PassiveCard q) {
      state.SetPassive(p, q);
    }

    public void Pass(Player p) {
      state.Pass(p);
    }

    public bool Selectable(Player p, GameCard c) {
      return state.Selectable(p, c); 
    }

    public void Select(Player p, GameCard c) {
      state.Select(p, c);
    }

    public bool CanMoveTo(Player p, int location) {
      return state.CanMoveTo(p, location);
    }

    public void MoveTo(Player p, int location) {
      state.MoveTo(p, location);
    }

    public virtual bool CanBasicAttack(Player p) {
      return state.CanBasicAttack(p);
    }

    public virtual void BasicAttack(Player p) {
      state.BasicAttack(p);
    }

    public virtual void SelectLeft(Player p) {
      state.SelectLeft(p);
    }

    public virtual void SelectRight(Player p) {
      state.SelectRight(p);
    }

    public virtual void SelectChannel(Player p, Channel c) {
      state.SelectChannel(p, c);
    }

    public virtual void StartChanneling(Player p, GameCard c) {
      state.StartChanneling(p, c);
    }
  }

  public struct GameEvent {
    public enum Type {
      GAME_BEGIN,
      GAME_OVER,
      TURN_END,
      TURN_START,
      ROUND_START,
      ROUND_END,
      MOVED,
      PASSIVE_CHOICE,
      PASSIVE_CHOSEN,
      MOVE_CHOICE,
      MOVE_CHOSEN,
      CARD_CHOICE,
      CARD_CHOSEN,
      DIRECTION_CHOICE,
      DIRECTION_CHOSEN,
      CHANNEL_CHOICE,
      CHANNEL_CHOSEN,
    }

    public Type type;
    public Game game;
    public Player player;

    public GameEvent(Type type, Game game, Player player = null) {
      this.type = type;
      this.game = game;
      this.player = player;
    }
  }

  internal abstract class GameState {
    public virtual void SetPassive(Player p, PassiveCard q) {
      return; // Do nothing.
    }

    public virtual bool IsTurn(Player p) {
      return false;
    }
    
    public virtual void Pass(Player p) {
      return; // Do nothing.
    }

    public virtual bool Selectable(Player p, GameCard c) {
      return false;
    }

    public virtual void Select(Player p, GameCard c) {
      return; // Do nothing.
    }

    public virtual bool CanBasicAttack(Player p) {
      return false;
    }

    public virtual void BasicAttack(Player p) {
      return; // Do nothing.
    }
    
    public virtual bool CanMoveTo(Player p, int location) {
      return false;
    }

    public virtual void MoveTo(Player p, int location) {
      return; // Do nothing.
    }

    public virtual void SelectLeft(Player p) {
      return; // Do nothing.
    }

    public virtual void SelectRight(Player p) {
      return; // Do nothing.
    }

    public virtual void SelectChannel(Player p, Channel c) {
      return; // Do nothing.
    }

    public virtual void StartChanneling(Player p, GameCard c) {
      return; // Do nothing.
    }
  }

  internal class GameTurnState : GameState {
    private Player player;
    private Game game;

    internal GameTurnState(Player player, Game game) {
      this.player = player;
      this.game = game;
      player.ResetActionPoints();
      player.Draw();
      player.TickChannels();
      game.NotifyAll(new GameEvent(GameEvent.Type.TURN_START, game, player));
    }

    public override void Pass(Player p) {
      if (player == p) {
        game.NotifyAll(new GameEvent(GameEvent.Type.TURN_END, game, player));
        p.CheckAllBuffs();
        game.SetState(new GameTurnState(game.Opponent(player), game));
      }
    }

    public override bool IsTurn(Player p) {
      return player == p;
    }

    public override bool CanMoveTo(Player p, int location) {
      return IsTurn(p) && p.HasAP(p.MovementCost() *
        game.Distance(p, location));
    }

    public override void MoveTo(Player p, int location) {
      if (CanMoveTo(p, location)) {
        p.NormalMove(location);
      }
    }

    public override bool Selectable(Player p, GameCard card) {
      return player == p && player.CanActivate(card);
    }

    public override void Select(Player p, GameCard card) {
      if (p == player && Selectable(p, card)) {
        player.PlayCard(card);
      }
    }

    public override bool CanBasicAttack(Player p) {
      return game.Distance(game.Opponent(p), p) <= p.MeleeRange() && 
        p.CanMelee();
    }

    public override void BasicAttack(Player p) {
      if (CanBasicAttack(p)) {
        player.BasicAttack(game.Opponent(p));
      }
    }

    public override void SelectChannel(Player p, Channel c) {
      if (p == player && player.OwnsChannel(c) && c.CanActivate(p, game)) {
        player.ActivateChannel(c);
      }
    }

    public override void StartChanneling(Player p, GameCard c) {
      if (p == player && player.EmptyChannelSlots() > 0 &&
        player.Hand.Contains(c)) {
          player.BeginChannel(c);
      }
    }
  }
  
  public delegate void DialogueCallback();
  internal class ChooseLocationState : GameState {
    private Player player;
    private int range;
    private DialogueCallback callback;
    private Game game;

    internal ChooseLocationState(Player player, int range,
      DialogueCallback callback, Game game) {
      this.player = player;
      this.range = range;
      this.callback = callback;
      this.game = game;
      game.NotifyAll(new GameEvent(GameEvent.Type.MOVE_CHOICE, game, player));
    }

    public override bool CanMoveTo(Player p, int location) {
      return p == player && game.Distance(p, location) < range;
    }

    public override void MoveTo(Player p, int location) {
      if (CanMoveTo(p, location)) {
        game.NotifyAll(new GameEvent(GameEvent.Type.MOVE_CHOSEN, game,
          player));
        game.SetPosition(p, location);
        callback();
      }
    }
  }

  internal delegate void CardChoiceCallback(GameCard card);
  internal class ChooseCardState : GameState {
    private Player player;
    private CardChoiceCallback callback;
    private List<GameCard> potential_cards;
    private Game game;
    private bool is_optional;

    internal ChooseCardState(Player player, List<GameCard> potential_cards,
      CardChoiceCallback callback, Game game, bool is_optional) {
      this.player = player;
      this.callback = callback;
      this.potential_cards = potential_cards;
      this.game = game;
      this.is_optional = is_optional;
      
      if (potential_cards == null || potential_cards.Count == 0)
        throw new RoRException("Card Dialogue with no cards!");

      // TODO(ticktakashi): Without passing the list of cards, I am forcing
      // the view to search through all possible cards for eligability.
      // Fix this.
      game.NotifyAll(new GameEvent(GameEvent.Type.CARD_CHOICE, game,
        player));
    }

    public override bool Selectable(Player p, GameCard card) {
      return p == player && potential_cards.Contains(card);
    }

    public override void Select(Player p, GameCard card) {
      if (p == player && Selectable(p, card)) {
        game.NotifyAll(new GameEvent(GameEvent.Type.CARD_CHOSEN, game, 
          player));
        callback(card);
      } 
    }

    public override void Pass(Player p) {
      if (is_optional)
        callback(null);
    }
  }

  internal delegate void DirectionCallback(bool direction);

  internal class ChooseDirectionState : GameState {
    private Player player;
    private Game game;
    private DirectionCallback callback;

    internal ChooseDirectionState(Player player, DirectionCallback callback, Game game) {
      this.player = player;
      this.game = game;
      this.callback = callback;
      game.NotifyAll(new GameEvent(GameEvent.Type.DIRECTION_CHOICE, game, 
        player));
    }

    public override void SelectLeft(Player p) {
      game.NotifyAll(new GameEvent(GameEvent.Type.DIRECTION_CHOSEN, game, 
        player));
      callback(false);
    }

    public override void SelectRight(Player p) {
      game.NotifyAll(new GameEvent(GameEvent.Type.DIRECTION_CHOSEN, game, 
        player));
      callback(true);
    }
  }

  internal delegate bool ChannelCallback(Channel channel);
  internal class ChooseChannelState : GameState {
    private Player player;
    private Game game;
    private ChannelCallback callback;
    
    internal ChooseChannelState(Player player, Game game,
      ChannelCallback callback) {
        this.player = player;
        this.game = game;
        this.callback = callback;
        game.NotifyAll(new GameEvent(GameEvent.Type.CHANNEL_CHOICE, game,
          player));
    }

    public override void SelectChannel(Player p, Channel c) {
      if (p == player && callback(c)) {
        game.NotifyAll(new GameEvent(GameEvent.Type.CHANNEL_CHOSEN, game,
          player));
      }
    }
  }

  internal class ChoosePassivesState : GameState {
    private Player player;
    private bool p_set = false;
    private bool o_set = false;
    private Game game;

    internal ChoosePassivesState(Player player, Game game) {
      this.player = player;
      this.game = game;
      game.NotifyAll(new GameEvent(GameEvent.Type.PASSIVE_CHOICE, game));
    }

    public override void SetPassive(Player player, PassiveCard passive) {
      player.SetPassive(passive);

      if (this.player == player)
        p_set = true;
      else
        o_set = true;

      if (p_set && o_set) {
        game.NotifyAll(new GameEvent(GameEvent.Type.PASSIVE_CHOSEN, game));
        game.SetState(new GameTurnState(game.Opponent(player), game));
      }
    }
  }
  
  public class RoRException : Exception {
    public RoRException(string message) : base(message) { }
  }

  internal interface IGameToken { }
}
