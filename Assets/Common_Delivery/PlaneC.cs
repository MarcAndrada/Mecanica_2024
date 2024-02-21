using System;

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

    public PlaneC(Vector3C D, float n) 
    {
        this.position = new Vector3C();
        this.normal = new Vector3C();
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public (float A, float B, float C, float D) ToEquation()
    {
        return (normal.x, normal.y, normal.z, -position.x * normal.x);
    }
    #endregion

    #region FUNCTIONS
    #endregion

}