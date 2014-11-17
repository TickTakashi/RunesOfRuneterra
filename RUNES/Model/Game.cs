using CARDScript.Model.Cards;
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
  public class Game : RoRObservable<GameEvent>, IRoRObserver<PlayerEvent>,
    IGameState {
    internal static readonly Random rng = new Random();
    internal static int FIELD_SIZE = 7;
    internal static int MAX_ROUNDS = 3;
    internal static int STARTING_HAND_SIZE = 4;

    private List<IGameToken>[] field;

    private Player player_1;
    private Player player_2;

    private IGameState state;

    private int round_num;

    private int player_1_wins;
    private int player_2_wins;

    internal Game(List<Card> player_1_deck, List<Card> player_2_deck) {
      round_num = 0;

      field = new List<IGameToken>[FIELD_SIZE];
      for (int i = 0; i < field.Length; i++) {
        field[i] = new List<IGameToken>();
      }

      player_1 = new Player(new CardCollection(player_1_deck), this);
      player_1.Attach(this);
      player_2 = new Player(new CardCollection(player_2_deck), this);
      player_2.Attach(this);

      field[FIELD_SIZE / 2 - 1].Add(player_1);
      field[FIELD_SIZE / 2 + 1].Add(player_2);

      NotifyAll(new GameEvent(GameEvent.Type.GAME_BEGIN, this));
      StartRound();
    }

    private void StartRound() {
      round_num++;
      NotifyAll(new GameEvent(GameEvent.Type.ROUND_START, this));
      player_1.ResetHealth();
      player_2.ResetHealth();

      for (int i = 0; i < STARTING_HAND_SIZE; i++) {
        player_1.Draw();
        player_2.Draw();
      }

      Player second_player = round_num % 2 == 0 ? player_1 : player_2;
      SetState(new ChoosePassivesState(second_player, this));
    }

    private void EndRound() {
      Player round_winner = null;

      if (!player_1.IsDead()) {
        round_winner = player_1;
        player_1_wins++;
      }

      if (!player_2.IsDead()) {
        round_winner = player_2;
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

    public void Update(PlayerEvent change_event) {
      if (change_event.type == PlayerEvent.Type.DEAD)
        EndRound();
      else if (change_event.type == PlayerEvent.Type.AP_OUT)
        Pass(change_event.player);
    }

    internal IGameState GetState() {
      return state;
    }

    internal void SetState(IGameState new_state) {
      state = new_state;
    }

    internal Player Opponent(Player a) {
      return a == player_1 ? player_2 : player_1;
    }

    internal int GetPosition(Player p) {
      // TODO(ticktakashi): Return the players position.
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
      winner = player_2_wins > player_1_wins ? player_2 : winner;
      winner = player_1_wins > player_2_wins ? player_1 : winner;
      NotifyAll(new GameEvent(GameEvent.Type.GAME_OVER, this, winner));
    }

    // Stateful methods - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public bool IsTurn(Player p) {
      return state.IsTurn(p);
    }

    public void SetPassive(Player p, Passive q) {
      state.SetPassive(p, q);
    }

    public void Pass(Player p) {
      state.Pass(p);
    }

    public bool Selectable(Card c) {
      return state.Selectable(c); 
    }

    public void Select(Card c) {
      state.Select(c);
    }

    public bool CanMoveTo(Player p, int location) {
      return state.CanMoveTo(p, location);
    }

    public void MoveTo(Player p, int location) {
      state.MoveTo(p, location);
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

  internal interface IGameState {
    void SetPassive(Player p, Passive q); 
    bool IsTurn(Player p);
    void Pass(Player p);
    bool Selectable(Card c);
    void Select(Card c);
    bool CanMoveTo(Player p, int location);
    void MoveTo(Player p, int location);
    bool CanBasicAttack(Player p);
    void BasicAttack(Player p);
  }

  internal class GameTurnState : IGameState {
    private Player player;
    private Game game;

    internal GameTurnState(Player player, Game game) {
      this.player = player;
      this.game = game;
      player.ResetActionPoints();
      player.Draw();
      game.NotifyAll(new GameEvent(GameEvent.Type.TURN_START, game, player));
    }

    public void SetPassive(Player p, Passive q) {
      return; // Do Nothing.
    }

    public void Pass(Player p) {
      if (player == p) {
        game.NotifyAll(new GameEvent(GameEvent.Type.TURN_END, game, player));
        p.CheckAllBuffs();
        game.SetState(new GameTurnState(game.Opponent(player), game));
      }
    }

    public bool IsTurn(Player p) {
      return player == p;
    }

    public bool CanMoveTo(Player p, int location) {
      return IsTurn(p) && p.HasAP(p.MovementCost() *
        game.Distance(p, location));
    }

    public void MoveTo(Player p, int location) {
      if (CanMoveTo(p, location)) {
        p.NormalMove(location);
      }
    }

    public bool Selectable(Card card) {
      return player.CanActivate(card);
    }

    public void Select(Card card) {
      if (Selectable(card)) {
        player.PlayCard(card);
      }
    }

    public bool CanBasicAttack(Player p) {
      return game.Distance(game.Opponent(p), p) <= p.MeleeRange() && 
        p.CanMelee();
    }

    public void BasicAttack(Player p) {
      if (CanBasicAttack(p)) {
        player.BasicAttack(game.Opponent(p));
      }
    }
  }
  
  public delegate void DialogueCallback();

  internal class ChooseLocationState : IGameState {
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

    public bool CanMoveTo(Player p, int location) {
      return p == player && game.Distance(p, location) < range;
    }

    public void MoveTo(Player p, int location) {
      if (CanMoveTo(p, location)) {
        game.NotifyAll(new GameEvent(GameEvent.Type.MOVE_CHOSEN, game,
          player));
        game.SetPosition(p, location);
        callback();
      }
    }
    
    public void SetPassive(Player p, Passive q) {
      return; // Do nothing.
    }

    public bool IsTurn(Player p) {
      return false;
    }

    public void Pass(Player p) {
      return; // Do nothing.
    }

    public bool Selectable(Card c) {
      return false;
    }

    public void Select(Card c) {
      return; // Do nothing.
    }

    public bool CanBasicAttack(Player p) {
      return false;
    }

    public void BasicAttack(Player p) {
      return; // Do nothing.
    }
  }

  internal delegate void CardChoiceCallback(Card card);
  internal class ChooseCardState : IGameState {
    private Player player;
    private CardChoiceCallback callback;
    private List<Card> potential_cards;
    private Game game;

    internal ChooseCardState(Player player, List<Card> potential_cards,
      CardChoiceCallback callback, Game game) {
      this.player = player;
      this.callback = callback;
      this.potential_cards = potential_cards;
      this.game = game;
      if (potential_cards.Count > 0)
        game.NotifyAll(new GameEvent(GameEvent.Type.CARD_CHOICE, game,
          player));
      else
        callback(null);
    }

    public bool Selectable(Card card) {
      return potential_cards.Contains(card);
    }

    public void Select(Card card) {
      if (Selectable(card)) {
        game.NotifyAll(new GameEvent(GameEvent.Type.CARD_CHOSEN, game, 
          player));
        callback(card);
      } 
    }

    public void Pass(Player p) {
      callback(null);
    }

    public void SetPassive(Player p, Passive q) {
      return; // Do nothing.
    }

    public bool IsTurn(Player p) {
      return false;
    }

    public bool CanMoveTo(Player p, int location) {
      return false;
    }

    public void MoveTo(Player p, int location) {
      return; // Do nothing.
    }

    public bool CanBasicAttack(Player p) {
      return false;
    }

    public void BasicAttack(Player p) {
      return; // Do nothing.
    }
  }


  internal class ChoosePassivesState : IGameState {
    private Player player;
    private bool p_set = false;
    private bool o_set = false;
    private Game game;

    internal ChoosePassivesState(Player player, Game game) {
      this.player = player;
      this.game = game;
      game.NotifyAll(new GameEvent(GameEvent.Type.PASSIVE_CHOICE, game));
    }

    public void SetPassive(Player player, Passive passive) {
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

    public bool IsTurn(Player p) {
      return false;
    }

    public void Pass(Player p) {
      return; // Do nothing.
    }

    public bool Selectable(Card c) {
      return false;
    }

    public void Select(Card c) {
      return; // Do nothing.
    }

    public bool CanMoveTo(Player p, int location) {
      return false;
    }

    public void MoveTo(Player p, int location) {
      return; // Do nothing.
    }

    public bool CanBasicAttack(Player p) {
      return false;
    }

    public void BasicAttack(Player p) {
      return; // Do nothing.
    }
  }
  
  public class RoRException : Exception {
    public RoRException(string message) : base(message) { }
  }

  internal interface IGameToken { }
}
