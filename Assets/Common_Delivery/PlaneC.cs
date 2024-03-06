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
        Vector3C vectorAB = new Vector3C(pointA, pointB); // Vector from A to B
        Vector3C vectorAC = new Vector3C(pointA, pointC); // Vector from A to C

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
        return (normal.x, normal.y, normal.z, -position.x * normal.x);
    }

    public Vector3C NearestPoint(Vector3C point)
    {
        float X = (point.x - this.position.x) * this.normal.x / (this.normal.magnitude * this.normal.magnitude) * this.normal.x;
        float Y = (point.y - this.position.y) * this.normal.y / (this.normal.magnitude * this.normal.magnitude) * this.normal.y;
        float Z = (point.z - this.position.z) * this.normal.z / (this.normal.magnitude * this.normal.magnitude) * this.normal.z;

        Vector3C proyection = new Vector3C(X, Y, Z);

        return point - proyection;
    }

    public Vector3C IntersectionWithLine()
    {
        return new Vector3C();
    }

    #endregion

    #region FUNCTIONS
    #endregion

}