namespace DDD.SuppleDesign.StandaloneClasses;

public class PigmentColor
{
    public int Red { get; set; }
    public int Yellow { get; set; }
    public int Blue { get; set; }

    public PigmentColor Mixin(PigmentColor anotherPigmentColor, double volume)
        => new PigmentColor();
}

public class Paint
{
    public double Volume { get; private set; }
    public PigmentColor PigmentColor { get; private set; }

    public Paint Mixin(Paint paint)
        => new Paint();
}
