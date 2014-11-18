using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CARDScript.Model.Cards {
  internal abstract class GameCard {
    private int _id;
    public int ID { get { return _id; } }

    private string _name;
    public string Name { get { return _name; } }

    public GameCard(string name, int id) {
      this._name = name;
      this._id = id;
    }
  }
}
