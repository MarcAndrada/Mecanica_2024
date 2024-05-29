using System;
using UnityEngine.UIElements;

[System.Serializable]
public class AA3_Waves
{

    public struct BuoySettings
    {
        public float boutancyCoefficient;
        public float bouyVelocity;
        public float mass;
    }
    public BuoySettings buoySettings;
    public SphereC buoy;

    [System.Serializable]
    public struct Settings
    {
        public float waterDensity;
        public float gravity;
    }
    public Settings settings;
    [System.Serializable]
    public struct WavesSettings
    {
        public float amplitude; // Amplitud de la ola
        public float frequency; // Frecuencia de la ola
        public float phase; // Fase inicial de la ola

        public Vector3C direction; // Direccion de la ola
        public float speed; // Velocidad de propagacion de la ola
    }
    public WavesSettings[] wavesSettings;
    public struct Vertex
    {
        public Vector3C originalposition;
        public Vector3C position;
        public Vertex(Vector3C _position)
        {
            this.position = _position;
            this.originalposition = _position;
        }
    }

    public Vertex[] points;
    private float elapsedTime; // Tiempo acumulado
    public AA3_Waves()
    {
        elapsedTime = 0.0f;
    }
    public void Update(float dt)
    {
        elapsedTime += dt;

        buoy.position.y = GetWaveHeight(buoy.position.x, buoy.position.z);


        for (int i = 0; i < points.Length; i++)
        {
            points[i].position = points[i].originalposition;


            Vector3C position = Vector3C.zero;

            for (int j = 0; j < wavesSettings.Length; j++)
            {
                float k = 2 * (float)Math.PI / wavesSettings[j].frequency;

                position.x += points[i].originalposition.x + wavesSettings[j].amplitude * k
                    * (float)Math.Cos(k * (points[i].originalposition * wavesSettings[j].direction + elapsedTime * wavesSettings[j].speed)
                    + wavesSettings[j].phase) * wavesSettings[j].direction.x;

                position.z += points[i].originalposition.z + wavesSettings[j].amplitude * k
                    * (float)Math.Cos(k * (points[i].originalposition * wavesSettings[j].direction + elapsedTime * wavesSettings[j].speed)
                    + wavesSettings[j].phase) * wavesSettings[j].direction.z;

                position.y += wavesSettings[j].amplitude
                    * (float)Math.Sin(k * (points[i].originalposition * wavesSettings[j].direction + elapsedTime * wavesSettings[j].speed)
                    + wavesSettings[j].phase);
            }

            points[i].position = position;
            //UnityEngine.Debug.Log("Position: " + points[i].position.y);

        }
    }
    public float GetWaveHeight(float x, float z)
    {
        float height = 0;

        for (int i = 0; i < wavesSettings.Length; i++)
        {
            float k = 2 * (float)Math.PI / wavesSettings[i].frequency;
            height += wavesSettings[i].amplitude
                    * (float)Math.Sin(k * (new Vector3C(x, 0 ,z) * wavesSettings[i].direction + elapsedTime * wavesSettings[i].speed)
                    + wavesSettings[i].phase);
        }

        return height;
    }

    public void Debug()
    {
        buoy.Print(Vector3C.green);
        if(points != null)
        foreach (var item in points)
        {
            item.originalposition.Print(0.05f);
            item.position.Print(0.05f);
        }
    }
}
