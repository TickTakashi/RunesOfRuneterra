using CARDScript.Model.BuffEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public class Passive {
    private int _id;
    public int ID { get { return _id; } }

    private string _name;
    public string Name { get { return _name; } }

    private Buff _buff;
    public Buff Buff { get { return _buff; } }
  }
}
