using System;

[System.Serializable]
public struct SphereC
{
    #region FIELDS
    public Vector3C position;
    public float radius;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public SphereC(Vector3C position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public bool IsInside(Vector3C point)
    {
        float distance = (float)Math.Sqrt(
            ((point.x - position.x) * (point.x - position.x)) +
            ((point.y - position.y) * (point.y - position.y)) +
            ((point.z - position.z) * (point.z - position.z)));

        return distance <= radius;
    }
    public Vector3C NearestPoint(Vector3C point)
    {
        PlaneC plane = new PlaneC(position, (point - position).normalized);

        return plane.NearestPoint(point);
    }
    public Vector3C IntersectionWithLine(LineC line)
    {
        PlaneC plane = new PlaneC(position, (line.origin - position).normalized);

        return plane.IntersectionWithLine(line);
    }
    #endregion

    #region FUNCTIONS
    #endregion

}