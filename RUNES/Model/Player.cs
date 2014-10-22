using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  public abstract class Player {
    public abstract void Damage(int value);
    public abstract void Heal(int value);
    public abstract void Draw();
    public abstract void Discard(int index);

    public static string TargetName(Player user, Player target) {
      return target == user ? "you" : "your opponent";
    }
  }
}
