using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model {
  public abstract class RoRObservable<E> {
    private List<IRoRObserver<E>> observers;

    public RoRObservable() {
      this.observers = new List<IRoRObserver<E>>();
    }

    public void Attach(IRoRObserver<E> observer) {
      if (!observers.Contains(observer)) {
        observers.Add(observer);
      } else {
        throw new RoRException("Observer added twice.");
      }
    }

    public void Detach(IRoRObserver<E> observer) {
      if (observers.Contains(observer)) {
        observers.Remove(observer);
      } else {
        throw new RoRException("Observer doesn't exist.");
      }
    }

    public void NotifyAll(E change_event) {
      foreach (IRoRObserver<E> o in observers) {
        o.Update(change_event);
      }
    }
  }

  public interface IRoRObserver<EventType> {
    void Update(EventType change_event);
  }
}
