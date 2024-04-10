using System;
using System.Runtime.CompilerServices;

[System.Serializable]
public struct Vector3C
{
    #region FIELDS
    public float x;
    public float y;
    public float z;
    #endregion

    #region PROPIERTIES
    public float r { get => x; set => x = value; }
    public float g { get => y; set => y = value; }
    public float b { get => z; set => z = value; }
    public float magnitude { 
        get 
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        } 
    }
    public Vector3C normalized 
    { 
        get 
        {
            return new Vector3C(x / magnitude, y / magnitude, z / magnitude); 
        } 
    }

    public static Vector3C zero { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C one { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C right { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C up { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C forward { get { return new Vector3C(0, 0, 1); } }

    public static Vector3C black { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C white { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C red { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C green { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C blue { get { return new Vector3C(0, 0, 1); } }
    #endregion

    #region CONSTRUCTORS
    public Vector3C(float x = 0, float y = 0, float z = 0)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public Vector3C(Vector3C a, Vector3C b)
    {
        this.x = b.x - a.x; 
        this.y = b.y - a.y; 
        this.z = b.z - a.z;
    }
    #endregion

    #region OPERATORS
    public static Vector3C operator +(Vector3C a)
    {
        return new Vector3C(Math.Abs(a.x), Math.Abs(a.y), Math.Abs(a.z));
    }
    public static Vector3C operator -(Vector3C a)
    {
        return new Vector3C(a.x * -1, a.y * -1, a.z * -1);
    }
    public static bool operator ==(Vector3C a, Vector3C b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }
    public static bool operator !=(Vector3C a, Vector3C b)
    {
        return a.x != b.x && a.y != b.y && a.z != b.z;
    }
    public static Vector3C operator +(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Vector3C operator -(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static float operator *(Vector3C a, Vector3C b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    public static float operator /(Vector3C a, Vector3C b)
    {
        return a.x / b.x + a.y / b.y + a.z / b.z;
    }
    public static Vector3C operator *(Vector3C a, float number)
    {
        return new Vector3C(a.x * number, a.y * number, a.z * number);
    }
    public static Vector3C operator /(Vector3C a, float number)
    {
        return new Vector3C(a.x / number, a.y / number, a.z / number);
    }
    #endregion

    #region METHODS
    public void Normalize()
    {
        x = normalized.x; y = normalized.y; z = normalized.z;  
    }

    public override bool Equals(object obj)
    {
        if(obj is Vector3C)
        {
            Vector3C other = (Vector3C)obj;
            return other == this;
        }
        return false;
    }
    #endregion

    #region FUNCTIONS
    public static float Dot(Vector3C v1, Vector3C v2)
    {
        return v1 * v2;
    }
    public static Vector3C Cross(Vector3C v1, Vector3C v2)
    {
        return new Vector3C((v1.y * v2.z - v1.z * v2.y), -(v1.x * v2.z - v1.z * v2.x), (v1.x * v2.y - v1.y * v2.x));
    }
    #endregion

}