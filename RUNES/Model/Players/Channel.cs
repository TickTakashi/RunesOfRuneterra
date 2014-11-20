using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Players {
  public class Channel : RoRObservable<ChannelEvent> {
    private int _time;
    public int Time { get { return _time; } }

    private GameCard _card;
    public GameCard Card { get { return _card; } }

    internal Channel(GameCard card) {
      this._card = card;
      NotifyAll(new ChannelEvent(ChannelEvent.Type.SET, this, _card));
    }

    internal void Tick(Player player, Game game) {
      _time++;
      NotifyAll(new ChannelEvent(ChannelEvent.Type.TICK, this, _card));
      if (Time >= _card.Cost(player, game)) {
        NotifyAll(new ChannelEvent(ChannelEvent.Type.READY, this, _card));
      }
    }

    internal void Activate(Player player, Game game) {
      if (Time >= _card.Cost(player, game)) {
        _card.Activate(player, game);
        NotifyAll(new ChannelEvent(ChannelEvent.Type.ACTIVATED, this, _card));
      }
    }

    internal bool CanActivate(Player player, Game game) {
      return Time >= _card.Cost(player, game);
    }
  }

  public class ChannelEvent {
    public enum Type {
      SET,
      TICK,
      READY,
      ACTIVATED,
    }

    public Type type;
    public Channel channel;
    public GameCard card;

    public ChannelEvent(Type type, Channel channel, GameCard card) {
      this.type = type;
      this.channel = channel;
      this.card = card;
    }
  }
}
