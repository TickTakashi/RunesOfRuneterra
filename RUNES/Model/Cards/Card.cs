using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  public abstract class Card {
    private int _id;
    public int ID { get { return _id; } }

    private string _name;
    public string Name { get { return _name; } }

    public Card(string name, int id) {
      this._name = name;
      this._id = id;
    }
    
    public abstract string Description();
  }
}
