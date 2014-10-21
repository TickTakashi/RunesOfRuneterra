using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUNES.Runes.Model {
  public class Player {
    
    public virtual void Damage(int value) {}
    public virtual void Heal(int value) {}
    public virtual void Draw() {}

    public virtual void Discard(int index) {}

    public static string TargetName(Player user, Player target) {
      return target == user ? "you" : "your opponent";
    }
  }
}
