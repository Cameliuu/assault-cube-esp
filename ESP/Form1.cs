using System.Runtime.InteropServices;
using System.Threading;
namespace ESP;

public partial class Form1 : Form
{
    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(Keys vKey);
    private Methods? m;
    private Entity localPlayer = new Entity();
    public Form1()
    {
        Console.WriteLine("test");
        InitializeComponent();
        Form1_Load();
    }

    void Main()
    {
        while (true)
        {
            CheckForIllegalCrossThreadCalls = false;
            m = new Methods();
            localPlayer=m.ReadLocalPlayer();
            var entities = m.ReadEntities(localPlayer);
            entities = entities.OrderBy(x => x.mag).ToList();
            if (GetAsyncKeyState(Keys.F1) < 0)
            {
                if(entities.Count > 0)
                    foreach (var e in entities)
                    {
                        if (e.team != localPlayer.team)
                        {
                            if( Methods.CalcDistance(localPlayer,e) < 100)

                            {
                                var angles = m.CalcAngles(localPlayer, e);
                                m.Aim(localPlayer, angles.X, angles.Y);
                                break;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ ! ] AIMBOT TARGET IS TOO FAR AWAY");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
            }

            Thread.Sleep(20);
        }
    }

    private void Form1_Load()
    {
        

        CheckForIllegalCrossThreadCalls = false;
        m = new Methods();
        if (m != null)
        {
            Thread thread = new Thread(Main) {IsBackground = true};
            thread.Start();
        }

       
        int i = 6;
    }
}