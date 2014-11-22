using CARDScript.Model.Cards.CardConditions;
using CARDScript.Model.GameCards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {

  public class CardCollection : RoRObservable<CardCollectionEvent>, IEnumerable<GameCard> {
    private List<GameCard> cards;

    internal CardCollection(List<GameCard> cards) : base() {
      this.cards = cards;
    }

    internal CardCollection() : this(new List<GameCard>()) {}

    internal int Size { get { return cards.Count; } }

    internal List<GameCard> GetCards() {
      return cards;
    }

    internal void Shuffle() {
      for (int i = 0; i < cards.Count; i++) {
        int swap_index = Game.rng.Next(cards.Count);
        GameCard old = cards[i];
        cards[i] = cards[swap_index];
        cards[swap_index] = old;
      }
      NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.SHUFFLE,
                                        this));
    }

    internal void Add(GameCard c) {
      if (!cards.Contains(c)) {
        cards.Add(c);
        NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.ADD,
                                          this, c));
      } else
        throw new RoRException("The same card object was added twice!");
    }

    internal void Add(CardCollection cards) {
      for (int i = 0; i < cards.Size; i++)
        this.cards.Add(cards.RemoveFirst());
    }

    internal GameCard Remove(GameCard c) {
      if (cards.Contains(c)) {
        cards.Remove(c);
        NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.REMOVE,
                                          this, c));
        return c;
      } else
        throw new RoRException("Tried to remove a card which wasn't present!");
    }

    internal GameCard RemoveAt(int i) {
      // TODO(ticktakashi): Should this be a RoRException instead?
      return Remove(cards[i]);
    }

    internal GameCard RemoveFirst() {
      if (cards.Count > 0)
        return RemoveAt(0);
      else {
        NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.EMPTY,
                                          this));
        return null;
      }
    }

    internal GameCard RemoveRandom() {
      return RemoveAt(Game.rng.Next(cards.Count));
    }

    internal bool IsEmpty() {
      return Size == 0;
    }

    public bool Contains(GameCard card) {
      return cards.Contains(card);
    }

    internal List<GameCard> CardsWhichSatisfy(GameCardCondition cond) {
      List<GameCard> meet_criteria = new List<GameCard>();
      foreach (GameCard c in cards) {
        if (cond.Condition(c))
          meet_criteria.Add(c);
      }
      return meet_criteria;
    }

    public GameCard Current {
      get { throw new NotImplementedException(); }
    }

    public void Dispose() {
      throw new NotImplementedException();
    }

    public IEnumerator<GameCard> GetEnumerator() {
      return cards.GetEnumerator(); 
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return cards.GetEnumerator();
    }
  }
  
  internal delegate bool CardConditionCallback(GameCard card);

  public struct CardCollectionEvent {
    public enum Type {
      ADD,
      REMOVE,
      SHUFFLE,
      EMPTY
    }

    public Type type;
    public CardCollection collection;
    public GameCard card;

    public CardCollectionEvent(Type type,
      CardCollection collection, GameCard card = null) {
      this.type = type;
      this.collection = collection;
      this.card = card;
    }
  }
}
