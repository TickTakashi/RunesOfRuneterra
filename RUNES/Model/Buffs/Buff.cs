using CARDScript.Model.Cards;
using CARDScript.Model.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.BuffEffects {
  public abstract class Buff {
    private Buff _next;
    public Buff Next {
      get { return _next; }
      set { _next = value; }
    }

    public void Apply(Card source, Player player, Game game) {
      player.ApplyBuff(source, this);
      InitBuff(source, player, game);
    }

    public virtual void InitBuff(Card source, Player player, Game game) {
      if (Next != null) {
        Next.InitBuff(source, player, game);
      }
    }

    public virtual int ModifyCardCost(GameCard card, int cost, Player p,
      Game g) {
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

    public override string ToString() {
      return (Next != null) ? " " + Next.ToString() : "";
    }

    public virtual string Description() {
      return (Next != null) ? " " + Next.Description() : "";
    }
  }
}
