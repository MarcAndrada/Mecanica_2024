using System;
using System.Threading.Tasks;

[System.Serializable]
public class AA3_Waves
{
    [System.Serializable]
    public struct Settings
    {

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

        Random rnd = new Random();
        for(int i = 0; i < points.Length; i++)
        {
            points[i].position = points[i].originalposition;
            points[i].position.y = rnd.Next(100) * 0.01f;

            Vector3C position = Vector3C.zero;

            for (int j = 0; j < wavesSettings.Length; j++)
            {

                float k = 2 * (float)Math.PI / wavesSettings[j].frequency;

                position.x += points[i].originalposition.x + wavesSettings[j].amplitude * k
                    * (float)Math.Cos(k * (points[i].originalposition * wavesSettings[j].direction + (elapsedTime * wavesSettings[j].speed))
                    + wavesSettings[j].phase) * wavesSettings[j].direction.x;

                position.z += points[i].originalposition.z + wavesSettings[j].amplitude * k
                    * (float)Math.Cos(k * (points[i].originalposition * wavesSettings[j].direction + (elapsedTime * wavesSettings[j].speed))
                    + wavesSettings[j].phase) * wavesSettings[j].direction.z;

                position.y += wavesSettings[j].amplitude
                    * (float)Math.Sin(k * (points[i].originalposition * wavesSettings[j].direction + (elapsedTime * wavesSettings[j].speed))
                    + wavesSettings[j].phase);
            }
            points[i].position = position;
        }
    }

    public void Debug()
    {
        if(points != null)
        foreach (var item in points)
        {
            item.originalposition.Print(0.05f);
            item.position.Print(0.05f);
        }
    }
}
