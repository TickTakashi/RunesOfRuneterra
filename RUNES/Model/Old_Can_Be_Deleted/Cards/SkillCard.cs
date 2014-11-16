using CARDScript.Compiler.Effects;
using CARDScript.Model.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  /* The SkillCard Class
   * 
   * Represents a Skillshot in league of legends. These skills are similar to
   * SpellCards except for the fact that they can be dodged. They also have an
   * additional activation hook, as something may happen only when they are
   * dodged.
   */
  public class SkillCard_O : SpellCard {
    public SkillCard(string name, int id, int damage, int range, int cost,
        int limit, Effect effect) : base(name, id, damage,
        range, cost, limit, effect) {
    }
  }
}
