using System.Numerics;

namespace ESP;

public class Entity
{
    public IntPtr baseAdress;
    public Vector3 headPos,feetPos;
    public Vector2 viewAngles;
    public float mag, viewOff;
    public int health, team, currentAmmo, alive;
    public string name;
}

public class ViewMatrix
{
    public float m11, m12, m13 ,m14;
    public float m21, m22, m23, m24;
    public float m31, m32, m33, m34;
    public float m41, m42, m43, m44;
}