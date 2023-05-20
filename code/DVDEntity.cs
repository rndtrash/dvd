using Sandbox;

namespace DVD;

public partial class DVDEntity : Entity
{
    public static DVDEntity Instance { get; internal set; }

    public static readonly Color[] HexColors = new[] {
        Color.Parse("#be00ff") ?? Color.Magenta,
        Color.Parse("#00feff") ?? Color.Magenta,
        Color.Parse("#ff8300") ?? Color.Magenta,
        Color.Parse("#0026ff") ?? Color.Magenta,
        Color.Parse("#fffa01") ?? Color.Magenta,
        Color.Parse("#ff2600") ?? Color.Magenta,
        Color.Parse("#ff008b") ?? Color.Magenta,
        Color.Parse("#25ff01") ?? Color.Magenta
    };

    [Net] public float xConverted { get; set; }
    [Net] public float yConverted { get; set; }
    [Net] public int currentColor { get; set; }

    public Sound currentSound { get; internal set; }

    // X & Y in [0; 1]
    private float X = 1;
    private float Y = 1;

    //[Net]
    float xDir = 1.0f;
    //[Net]
    float yDir = 1.0f;
    readonly float speed = 100f;
    bool hyped = false;
    Vector2 cornerOfInterest = new(0, 0);

    public override void Spawn()
    {
        base.Spawn();

        Transmit = TransmitType.Always;

        SetCornerOfInterest();

        Instance = this;
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();

        Instance = this;
    }

    [GameEvent.Tick.Server]
    public void Tick()
    {
        X = MathX.Clamp(X + xDir * speed * Time.Delta, 0, 1920);
        Y = MathX.Clamp(Y + yDir * speed * Time.Delta, 0, 1080);

        xConverted = X / 1920.0f;
        yConverted = Y / 1080.0f;

        if (!hyped)
        {
            if (Vector3.DistanceBetween(new Vector3(cornerOfInterest, 0), new Vector3(X, Y, 0)) < 300.0f)
            {
                hyped = true;
                PlayScreenSound(To.Everyone, "close");
            }
        }

        var ts = TouchSides();
        var tcf = TouchCeilFloor();
        if (ts || tcf)
        {
            NextColor();
            if (ts)
                xDir *= -1;
            if (tcf)
                yDir *= -1;
            if (ts && tcf)
            {
                Game.AddScore();
                PlayScreenSound(To.Everyone, "yooouuu");
            }
            else if (hyped)
            {
                PlayScreenSound(To.Everyone, "damn");
            }
            hyped = false;

            SetCornerOfInterest();
        }
    }

    [ClientRpc]
    public static void PlayScreenSound(string sound)
    {
        Log.Info($"playing sound {sound}");
        Instance.currentSound.Stop();
        Instance.currentSound = Sound.FromScreen(sound);
    }

    private void NextColor()
    {
        currentColor = (currentColor + 1) % HexColors.Length;
    }

    private void SetCornerOfInterest()
    {
        cornerOfInterest = new Vector2(xDir < 0 ? 0 : 1920, yDir < 0 ? 0 : 1080);
    }

    private bool TouchSides()
    {
        return ((int)X <= 0) || ((int)X >= 1920);
    }

    private bool TouchCeilFloor()
    {
        return ((int)Y <= 0) || ((int)Y >= 1080);
    }
}
