using System.Runtime.InteropServices;
using System.Threading;
using ezOverLay;
namespace ESP;

public partial class Form1 : Form
{
    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(Keys vKey);
    private Methods? m;
    private Entity localPlayer = new Entity();
    private List<Entity> entities= new List<Entity>();
    ez ez = new ez();
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
            entities = m.ReadEntities(localPlayer);
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
            Form1 f = this;
            f.Refresh();
            Thread.Sleep(20);
        }
    }

    private void Form1_Load()
    {
        

      
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Pen red = new Pen(Color.Red,3);
        Pen green = new Pen(Color.Green, 3);
        foreach (var ent in entities.ToList())
        {
            var wtsFeet = m.WorldToScreen(m.ReadViewMatrix(), ent.feetPos, this.Width, this.Height);
            var wtsHed = m.WorldToScreen(m.ReadViewMatrix(), ent.headPos, this.Width, this.Height);

            if (wtsFeet.X > 0)
            {
                if (localPlayer.team == ent.team)
                {
                    g.DrawRectangle(green,m.CalcRect(wtsFeet,wtsHed));
                    g.DrawLine(green, new Point(Width / 2, Height / 2),wtsFeet);
                    
                }
                else
                {
                    g.DrawRectangle(red, m.CalcRect(wtsFeet, wtsHed));
                    g.DrawLine(red,new Point(Width / 2, Height / 2), wtsFeet);
                    Console.WriteLine(m.CalcRect(wtsFeet,wtsHed));
                }
            }
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        CheckForIllegalCrossThreadCalls = false;
        m = new Methods();
        if (m != null)
        {
            ez.SetInvi(this);
            ez.DoStuff("AssaultCube", this);
            Thread thread = new Thread(Main) { IsBackground = true };
            thread.Start();
        }


        int i = 6;
    }
}