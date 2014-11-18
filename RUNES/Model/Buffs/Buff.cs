using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.BuffEffects {

  internal abstract class Buff {
    public Card source;

    private Buff _next;
    public Buff Next {
      get { return _next; }
      set { _next = value; }
    }

    public virtual int ModifyCardCost(Card card, int cost, Player p, Game g) {
      return Next != null ? Next.ModifyCardCost(card, cost, p, g) : cost;
    }

    public virtual int ModifyActionPoints(int d, Player p, Game g) {
      return Next != null ? Next.ModifyActionPoints(d, p, g) : d;
    }

    public virtual int ModifyDamage(int d, Player p, Game g) {
      return Next != null ? Next.ModifyDamage(d, p, g) : d;
    }

    public virtual int ModifyHeal(int d, Player p, Game g) {
      return Next != null ? Next.ModifyHeal(d, p, g) : d;
    }

    public virtual int ModifyMeleeDamage(int d, Player p, Game g) {
      return Next != null ? Next.ModifyMeleeDamage(d, p, g) : d;
    }

    public virtual int ModifyMeleeRange(int d, Player p, Game g) {
      return Next != null ? Next.ModifyMeleeRange(d, p, g) : d;
    }

    public virtual int ModifySkillDamage(int d, Player p, Game g) {
      return Next != null ? Next.ModifySkillDamage(d, p, g) : d;
    }

    public virtual int ModifySkillRange(int d, Player p, Game g) {
      return Next != null ? Next.ModifySkillRange(d, p, g) : d;
    }

    public virtual int ModifySpellDamage(int d, Player p, Game g) {
      return Next != null ? Next.ModifySpellDamage(d, p, g) : d;
    }

    public virtual int ModifySpellRange(int d, Player p, Game g) {
      return Next != null ? Next.ModifySpellRange(d, p, g) : d;
    }

    public virtual bool IsFinished() {
      return true;
    }
  }
}
