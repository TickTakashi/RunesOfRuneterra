using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Players {
  public class Status {
    public static int STATUS_COUNTER = 0;   // User to order statuses.

    public int status_id;    // Unique ID for this status
    public BuffType type;    // What does this buff do?
    public int power;        // Can be positive or negative

    public Status(BuffType type, int power) {
      this.status_id = STATUS_COUNTER++;
      this.type = type;
      this.power = power;
    }
  }


  public enum BuffType {
    SNARE,
    STUN,
    SLOW,
    SILENCE,
    BLIND,
    KNOCKUP,
    SHIELD,
    MELEE_RANGE,
    MELEE_DAMAGE,
    SPELL_RANGE,
    SPELL_DAMAGE,
    SKILL_RANGE,
    SKILL_DAMAGE,
    SELF_DURATION
  }
}