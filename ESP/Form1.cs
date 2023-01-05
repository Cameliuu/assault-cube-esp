using System.Runtime.InteropServices;
using System.Threading;
namespace ESP;

public partial class Form1 : Form
{
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
        }
    }

    private void Form1_Load()
    {
        

        CheckForIllegalCrossThreadCalls = false;
        m = new Methods();
        if (m == null)
        {
            Thread thread = new Thread(Main) {IsBackground = true};
            thread.Start();
        }

        localPlayer=m.ReadLocalPlayer();
        var entities = m.ReadEntities(localPlayer);
        int i = 6;
    }
}