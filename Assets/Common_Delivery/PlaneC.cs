using System;
using System.Xml.Serialization;
using UnityEngine.Experimental.GlobalIllumination;

[System.Serializable]
public struct PlaneC
{
    #region FIELDS
    public Vector3C position;
    public Vector3C normal;
    #endregion

    #region PROPIERTIES
    public static PlaneC right { get { return new PlaneC(new Vector3C(0, 0, 0), new Vector3C(1, 0, 0)); } }
    public static PlaneC up { get { return new PlaneC(new Vector3C(0, 0, 0), new Vector3C(0, 1, 0)); } }
    public static PlaneC forward { get { return new PlaneC(new Vector3C(0, 0, 0), new Vector3C(0, 0, 1)); } }
    #endregion

    #region CONSTRUCTORS
    public PlaneC(Vector3C position, Vector3C normal)
    {
        this.position = position;
        this.normal = normal.normalized;
    }
    public PlaneC(Vector3C pointA, Vector3C pointB, Vector3C pointC)
    {
        Vector3C vectorAB = new Vector3C(pointA, pointB); // vector from A to B
        Vector3C vectorAC = new Vector3C(pointA, pointC); // vector from A to C

        this.normal = Vector3C.Cross(vectorAB, vectorAC).normalized;

        this.position = pointA;
    }
    public PlaneC(float A, float B, float C, float D)
    {

        this.position = new Vector3C(- D / A, -D / B, -D / C);
        this.normal = new Vector3C(A, B, C);
    }

    public PlaneC(Vector3C N, float d) 
    {
        this.position = new Vector3C(-d / N.x, -d / N.y, -d / N.z);
        this.normal = N;
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public (float A, float B, float C, float D) ToEquation()
    {
        return (normal.x, normal.y, normal.z, -Vector3C.Dot(normal, position));
    }

    public Vector3C NearestPoint(Vector3C point)
    {
        float X = (point.x - this.position.x) * this.normal.x / (this.normal.magnitude * this.normal.magnitude) * this.normal.x;
        float Y = (point.y - this.position.y) * this.normal.y / (this.normal.magnitude * this.normal.magnitude) * this.normal.y;
        float Z = (point.z - this.position.z) * this.normal.z / (this.normal.magnitude * this.normal.magnitude) * this.normal.z;

        Vector3C proyection = new Vector3C(X, Y, Z);

        return point - proyection;
    }

    public Vector3C IntersectionWithLine(LineC line)
    {
        float distance = Vector3C.Dot(normal, position - line.origin) / Vector3C.Dot(normal, line.direction);
        return line.origin + line.direction * distance;
    }

    public float DistanceToPoint(Vector3C point)
    {
        float equation = ToEquation().A * point.x + ToEquation().B * point.y + ToEquation().C * point.z + ToEquation().D;

        if (normal.magnitude == 0.0f) 
        {
            return 0.0f;
        }

        return Math.Abs(equation) / normal.magnitude;
    }
    #endregion

    #region FUNCTIONS
    #endregion

}