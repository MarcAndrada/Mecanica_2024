using System;

[System.Serializable]
public struct LineC
{
    #region FIELDS
    public Vector3C origin;
    public Vector3C direction;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public LineC(Vector3C origin, Vector3C direction)
    {
        this.origin = origin;
        this.direction = direction;
    }
    #endregion

    #region OPERATORS
    public static bool operator ==(LineC a, LineC b)
    {
        return a.origin == b.origin && a.direction == b.direction;
    }
    public static bool operator !=(LineC a, LineC b)
    {
        return a.origin != b.origin && a.direction != b.direction;
    }
    #endregion

    #region METHODS
    public Vector3C NearestPointToPoint(Vector3C point)
    {
        Vector3C vector = point - origin;
        float dot = Vector3C.Dot(vector, direction);
        Vector3C nearestPoint = origin - direction * dot;

        return nearestPoint;
    }
    public Vector3C NearestPointToLine(LineC line)
    {
        return line.origin;
    }
    #endregion

    #region FUNCTIONS
    public static LineC CreateLinePointAPointB(Vector3C pointA, Vector3C pointB)
    {
        return new LineC(pointA, pointB - pointA);
    }
    #endregion

}