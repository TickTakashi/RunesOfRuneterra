using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Compiler {
  public interface IPlayer {
    void Damage(int value);
    void Heal(int value);
    void Draw(int value);

    void Discard(int index);
  }
}
