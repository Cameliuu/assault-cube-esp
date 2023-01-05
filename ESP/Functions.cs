using System.Numerics;
using System.Text;
using Swed32;
namespace ESP;

public class Methods
{
    public Swed mem;
    public IntPtr moduleBase;

    public Entity ReadLocalPlayer()
    {
        
        var localPlayer = ReadEntity(mem.ReadPointer(moduleBase, Offsets.iLocalPlayer));
        localPlayer.viewAngles.X = mem.ReadFloat(localPlayer.baseAdress, Offsets.vAngles);
        localPlayer.viewAngles.Y = mem.ReadFloat(localPlayer.baseAdress, Offsets.vAngles + 0x4);
        Console.WriteLine($"[ + ] LOADED LOCAL PLAYER NAMED {localPlayer.name}");
        return localPlayer;
    }
    
    public Entity ReadEntity(IntPtr entBase)
    {
        Console.WriteLine("Reading entity");
        var ent = new Entity();
        ent.baseAdress = entBase;
        ent.currentAmmo = mem.ReadInt(ent.baseAdress, Offsets.vAmmo);
        ent.health = mem.ReadInt(ent.baseAdress, Offsets.vHealth);
        ent.team = mem.ReadInt(ent.baseAdress, Offsets.vTeam);
        ent.feetPos = mem.ReadVec(ent.baseAdress, Offsets.vFeet);
        ent.headPos = mem.ReadVec(ent.baseAdress, Offsets.vHead);
        ent.name = Encoding.UTF8.GetString(mem.ReadBytes(ent.baseAdress, Offsets.vName,11));
        ent.alive = mem.ReadInt(ent.baseAdress, Offsets.vAlive);
        return ent;
    }

    public static float CalcMag(Entity localPlayer, Entity target)
    {
        return (float)Math.Sqrt(Math.Pow(target.feetPos.X - localPlayer.feetPos.X, 2) +
                                Math.Pow(target.feetPos.Y - localPlayer.feetPos.Y, 2) +
                                Math.Pow(target.feetPos.Z - localPlayer.feetPos.Z, 2));
    }
    public static float CalcDistance(Entity localPlayer, Entity target)
    {
        return (float)Math.Sqrt(Math.Pow(target.feetPos.X - localPlayer.feetPos.X, 2) +
                                Math.Pow(target.feetPos.Y - localPlayer.feetPos.Y, 2));
    }
    public Vector2 CalcAngles(Entity localPlayer, Entity target)
    {
        float x, y;
        var deltaX = target.headPos.X - localPlayer.headPos.X;
        var deltaY = target.headPos.Y - localPlayer.headPos.Y;
        var deltaZ = target.headPos.Z - localPlayer.headPos.Z;
        var dist = CalcDistance(localPlayer, target);
        x = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI) + 90;
        y = (float)(Math.Atan2(deltaZ, dist) * 180 / Math.PI);
        return new Vector2(x, y);
    }

    public void Aim(Entity localPlayer, float x, float y)
    {
        mem.WriteFloat(localPlayer.baseAdress, Offsets.vAngles, x);
        mem.WriteFloat(localPlayer.baseAdress, Offsets.vAngles + 0x4,y);
    }


    public List<Entity> ReadEntities(Entity localPlayer)
    {
        Console.WriteLine();
        var entities = new List<Entity>();
        var entityList = mem.ReadPointer(moduleBase, Offsets.iEntityList);
        for (int i = 0; i < 9; i++)
        {
            var currentEntBase = mem.ReadPointer(entityList, i * 0x4);
            var ent = ReadEntity(currentEntBase);
            ent.mag = CalcMag(localPlayer, ent);
            if (ent.health > 0 && ent.health <= 100)
            {
                entities.Add(ent);
            }
        }

        foreach (var e in entities)
        {
            Console.WriteLine($"[ + ] LOADED ENTITY NAMEDl {e.name}");
        }
        return entities;
    }

    public Methods()
    {
        mem = new Swed("ac_client");
        moduleBase = mem.GetModuleBase(".exe");
    }
}