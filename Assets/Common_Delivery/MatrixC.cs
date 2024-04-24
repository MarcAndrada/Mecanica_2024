using System;

[System.Serializable]
public struct MatrixC
{
    #region FIELDS
    public float[,] data;
    int size;
    #endregion

    #region PROPIERTIES

    #endregion

    #region CONSTRUCTORS
    public MatrixC(float[,] data)
    {
        this.data = data; 
        size = (int)Math.Sqrt(data.Length);
    }
    #endregion

    #region OPERATORS
    public static Vector3C operator *(MatrixC a, Vector3C b)
    {
        Vector3C result = new Vector3C();

        result.x = a.data[0,0] * b.x + a.data[0, 1] * b.y + a.data[0, 2] * b.z;
        result.y = a.data[1,0] * b.x + a.data[1, 1] * b.y + a.data[1, 2] * b.z;
        result.z = a.data[2, 0] * b.x + a.data[2, 1] * b.y + a.data[2, 2] * b.z;

        return result;
    }
    #endregion

    #region METHODS
    public static float Determintate(MatrixC m)
    {
        if (m.size != 3)
        {
            return 0;
        }
        float determinate =
            ((m.data[0, 0] * m.data[1, 1] * m.data[2, 2]) + (m.data[0, 1] * m.data[1, 2] * m.data[2, 0]) + (m.data[0, 2] * m.data[1, 0] * m.data[2, 1])) -
            ((m.data[0, 2] * m.data[1, 1] * m.data[2, 0]) + (m.data[0, 0] * m.data[1, 2] * m.data[2, 1]) + (m.data[0, 1] * m.data[1, 0] * m.data[2, 2]));


        return determinate;
    }
    public MatrixC Transposed(MatrixC m)
    {
        MatrixC transposed = new MatrixC();

        for (int i = 0; i < m.size; i++)
        {
            for (int j = 0; j < m.size; j++)
            {
                transposed.data[j, i] = m.data[i, j];
            }
        }

        return transposed;
    }
    #endregion

    #region FUNCTIONS
    //public static MatrixC Inverse(MatrixC m)
    //{
    //    float determinate = Determintate(m);

    //    if (determinate == 0)
    //        return new MatrixC();

    //    MatrixC inverse = new MatrixC();
    //    float invDet = 1.0f / determinate;

    //    inverse.data[0, 0] = (m.data[1, 1] * m.data[2, 2] - m.data[1, 2] * m.data[2, 1]) * invDet;
    //    inverse.data[0, 1] = (m.data[0, 2] * m.data[2, 1] - m.data[0, 1] * m.data[2, 2]) * invDet * -1;
    //    inverse.data[0, 2] = (m.data[0, 1] * m.data[1, 2] - m.data[0, 2] * m.data[1, 1]) * invDet;
    //    inverse.data[1, 0] = (m.data[1, 2] * m.data[2, 0] - m.data[1, 0] * m.data[2, 2]) * invDet * -1;
    //    inverse.data[1, 1] = (m.data[0, 0] * m.data[2, 2] - m.data[0, 2] * m.data[2, 0]) * invDet;
    //    inverse.data[1, 2] = (m.data[0, 2] * m.data[1, 0] - m.data[0, 0] * m.data[1, 2]) * invDet * -1;
    //    inverse.data[2, 0] = (m.data[1, 0] * m.data[2, 1] - m.data[1, 1] * m.data[2, 0]) * invDet;
    //    inverse.data[2, 1] = (m.data[0, 1] * m.data[2, 0] - m.data[0, 0] * m.data[2, 1]) * invDet * -1;
    //    inverse.data[2, 2] = (m.data[0, 0] * m.data[1, 1] - m.data[0, 1] * m.data[1, 0]) * invDet;

    //    return Transposed(inverse);
    //}

    public static MatrixC RotateX(float angles)
    {
        MatrixC rotatedXMatrix = new MatrixC();

        rotatedXMatrix.data = new float[3, 3];
        rotatedXMatrix.data[0, 0] = 1;
        rotatedXMatrix.data[0, 1] = 0;
        rotatedXMatrix.data[0, 2] = 0;
        rotatedXMatrix.data[1, 0] = 0;
        rotatedXMatrix.data[1, 1] = (float)Math.Cos(angles);
        rotatedXMatrix.data[1, 2] = (float)Math.Sin(angles);
        rotatedXMatrix.data[2, 0] = 0;
        rotatedXMatrix.data[2, 1] = (float)-Math.Sin(angles);
        rotatedXMatrix.data[2, 2] = (float)Math.Cos(angles);


        return rotatedXMatrix;

    }
    public static MatrixC RotateY(float angles)
    {
        MatrixC rotatedXMatrix = new MatrixC();

        rotatedXMatrix.data = new float[3, 3];
        rotatedXMatrix.data[0, 0] = (float)Math.Cos(angles);
        rotatedXMatrix.data[0, 1] = 0;
        rotatedXMatrix.data[0, 2] = (float)-Math.Sin(angles);
        rotatedXMatrix.data[1, 0] = 0;
        rotatedXMatrix.data[1, 1] = 1;
        rotatedXMatrix.data[1, 2] = 0;
        rotatedXMatrix.data[2, 0] = (float)Math.Sin(angles);
        rotatedXMatrix.data[2, 1] = 0;
        rotatedXMatrix.data[2, 2] = (float)Math.Cos(angles);


        return rotatedXMatrix;

    }
    public static MatrixC RotateZ(float angles)
    {
        MatrixC rotatedXMatrix = new MatrixC();

        rotatedXMatrix.data = new float[3, 3];
        rotatedXMatrix.data[0, 0] = (float)Math.Cos(angles);
        rotatedXMatrix.data[0, 1] = (float)Math.Sin(angles);
        rotatedXMatrix.data[0, 2] = 0;
        rotatedXMatrix.data[1, 0] = (float)-Math.Sin(angles);
        rotatedXMatrix.data[1, 1] = (float)Math.Cos(angles);
        rotatedXMatrix.data[1, 2] = 0;
        rotatedXMatrix.data[2, 0] = 0;
        rotatedXMatrix.data[2, 1] = 0;
        rotatedXMatrix.data[2, 2] = 1;


        return rotatedXMatrix;

    }
    #endregion

}