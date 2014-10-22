namespace CARDScript.Compiler.Effects.ScalarEffects {
  public class Draw : ScalarEffect {

    public override bool Activate() {
      Log("Draw event. " + target + " draws " + value);
      for (int i = 0; i < value; i++)
        target.Draw();
      return base.Activate();
    }

    protected override string Noun() { return value > 1 ? "cards" : "card" ; }
  }
}
