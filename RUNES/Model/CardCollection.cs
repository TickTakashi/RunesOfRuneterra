using CARDScript.Model.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {

  public class CardCollection : RoRObservable<CardCollectionEvent>, 
    IEnumerator {
    private List<Card> cards;

    internal CardCollection(List<Card> cards) : base() {
      this.cards = cards;
    }

    internal CardCollection() : this(new List<Card>()) {}

    internal int Size { get { return cards.Count; } }

    internal List<Card> GetCards() {
      return cards;
    }

    internal void Shuffle() {
      for (int i = 0; i < cards.Count; i++) {
        int swap_index = Game.rng.Next(cards.Count);
        Card old = cards[i];
        cards[i] = cards[swap_index];
        cards[swap_index] = old;
      }
      NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.SHUFFLE,
                                        this));
    }

    internal void Add(Card c) {
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

    internal Card Remove(Card c) {
      if (cards.Contains(c)) {
        cards.Remove(c);
        NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.REMOVE,
                                          this, c));
        return c;
      } else
        throw new RoRException("Tried to remove a card which wasn't present!");
    }

    internal Card RemoveAt(int i) {
      // TODO(ticktakashi): Should this be a RoRException instead?
      return Remove(cards[i]);
    }

    internal Card RemoveFirst() {
      if (cards.Count > 0)
        return RemoveAt(0);
      else {
        NotifyAll(new CardCollectionEvent(CardCollectionEvent.Type.EMPTY,
                                          this));
        return null;
      }
    }

    internal Card RemoveRandom() {
      return RemoveAt(Game.rng.Next(cards.Count));
    }

    internal bool IsEmpty() {
      return Size == 0;
    }

    internal bool Contains(Card card) {
      return cards.Contains(card);
    }

    internal List<Card> CardsWhichSatisfy(CardCondition cond) {
      List<Card> meet_criteria = new List<Card>();
      foreach (Card c in cards) {
        if (cond(c))
          meet_criteria.Add(c);
      }
      return meet_criteria;
    }
  }
  
  public struct CardCollectionEvent {
    public enum Type {
      ADD,
      REMOVE,
      SHUFFLE,
      EMPTY
    }

    public Type type;
    public CardCollection collection;
    public Card card;

    public CardCollectionEvent(Type type,
      CardCollection collection, Card card = null) {
      this.type = type;
      this.collection = collection;
      this.card = card;
    }
  }
}
