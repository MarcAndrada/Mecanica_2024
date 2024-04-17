using System;

[System.Serializable]
public struct MatrixC
{
    #region FIELDS
    public float[] data;
    int size;
    #endregion

    #region PROPIERTIES

    #endregion

    #region CONSTRUCTORS
    public MatrixC(float[] data, int size)
    {
        this.data = data; 
        this.size = size;
    }
    #endregion

    #region OPERATORS
    public static Vector3C operator *(MatrixC a, Vector3C b)
    {
        Vector3C result = new Vector3C();

        result.x = a.data[0] * b.x + a.data[1] * b.y + a.data[2] * b.z;
        result.y = a.data[3] * b.x + a.data[4] * b.y + a.data[5] * b.z;
        result.z = a.data[6] * b.x + a.data[7] * b.y + a.data[8] * b.z;


        return result;
    }
    #endregion

    #region METHODS
    
    #endregion

    #region FUNCTIONS
    public static MatrixC Inverse(MatrixC a)
    {
        for (int i = 0; i < a.data.Length; i++)
        {
            a.data[i] *= -1;
        }
        return a;
    }
    #endregion

}