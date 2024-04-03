using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class AA2_Cloth
{
    [System.Serializable]
    public struct Settings
    {
        public Vector3C gravity;
        [Min(1)]
        public float width;
        [Min(1)]
        public float height;
        [Min(2)]
        public int xPartSize;
        [Min(2)]
        public int yPartSize;
    }
    public Settings settings;
    [System.Serializable]
    public struct ClothSettings
    {
        [Header("Structural Spring")]
        public float structuralElasticCoef;
        public float structuralDamptCoef;
        public float structuralSpringLenght;

        [Header("Shear Spring")]
        public float shearElasticCoef;
        public float shearDamptCoef;
        public float shearSpringLenght;

        [Header("Bending Spring")]
        public float bendingElasticCoef;
        public float bendingDamptCoef;
        public float bendingSpringLenght;
    }
    public ClothSettings clothSettings;

    [System.Serializable]
    public struct SettingsCollision
    {
        public SphereC sphere;
    }
    public SettingsCollision settingsCollision;
    public struct Vertex
    {
        public Vector3C lastPosition;
        public Vector3C actualPosition;
        public Vector3C velocity;
        public Vertex(Vector3C _position)
        {
            this.actualPosition = _position;
            this.lastPosition = _position;
            this.velocity = new Vector3C(0, 0, 0);
        }

        public void Euler(Vector3C force, float dt)
        {
            lastPosition = actualPosition;
            velocity += force * dt;
            actualPosition += velocity * dt;
        }
    }
    public Vertex[] points;
    public void Update(float dt)
    {
        int xVertices = settings.xPartSize + 1;
        int yVertices = settings.yPartSize + 1;

        Vector3C[] forces = new Vector3C[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            //STRUCTURAL VERTICAL
            if (i > xVertices - 1)
            {
                float structMagnitudeY = (points[i - xVertices].actualPosition - points[i].actualPosition).magnitude
                                                 - clothSettings.structuralSpringLenght;
                Vector3C structForceVector = (points[i - xVertices].actualPosition
                                    - points[i].actualPosition).normalized * structMagnitudeY * clothSettings.structuralElasticCoef;

                Vector3C structDampingForce = (points[i - xVertices].actualPosition - points[i].actualPosition) * clothSettings.structuralDamptCoef;
                Vector3C structSpringForce = structForceVector * clothSettings.structuralElasticCoef - structDampingForce;


                forces[i] += structSpringForce;
                forces[i - xVertices] += -structSpringForce;
            }

            //STRUCTURAL HORIZONTAL
            if (i % xVertices != 0)
            {
                float structMagnitudeX = (points[i - 1].actualPosition - points[i].actualPosition).magnitude
                                                 - clothSettings.structuralSpringLenght;
                Vector3C structForceVector = (points[i - 1].actualPosition
                                    - points[i].actualPosition).normalized * structMagnitudeX * clothSettings.structuralElasticCoef;

                Vector3C structDampingForce = (points[i - 1].actualPosition - points[i].actualPosition) * clothSettings.structuralDamptCoef;
                Vector3C structSpringForce = structForceVector * clothSettings.structuralElasticCoef - structDampingForce;

                forces[i] += structSpringForce;
                forces[i - 1] += -structSpringForce;
            }

            //SHEAR
            if (i > xVertices - 1 && i % xVertices - 1 != 0)
            {
                float shearMagnitude = (points[i - xVertices + 1].actualPosition - points[i].actualPosition).magnitude
                                                 - clothSettings.shearSpringLenght;
                Vector3C shearForceVector = (points[i - xVertices + 1].actualPosition
                                    - points[i].actualPosition).normalized * shearMagnitude * clothSettings.shearElasticCoef;


                Vector3C shearDampingForce = (points[i - xVertices + 1].actualPosition - points[i].actualPosition) * clothSettings.shearDamptCoef;
                Vector3C shearSpringForce = shearForceVector * clothSettings.shearElasticCoef - shearDampingForce;

                forces[i] += shearSpringForce;
                forces[i - xVertices + 1] += -shearSpringForce;
            }

            //BENDING VERTICAL
            if (i > xVertices * 2 - 1)
            {
                float bendMagnitudeY = (points[i - xVertices * 2].actualPosition - points[i].actualPosition).magnitude
                                                 - clothSettings.bendingSpringLenght;
                Vector3C bendForceVector = (points[i - xVertices * 2].actualPosition
                                    - points[i].actualPosition).normalized * bendMagnitudeY * clothSettings.bendingElasticCoef;

                Vector3C bendDampingForce = (points[i - xVertices * 2].actualPosition - points[i].actualPosition) * clothSettings.bendingDamptCoef;
                Vector3C bendSpringForce = bendForceVector * clothSettings.bendingElasticCoef - bendDampingForce;


                forces[i] += bendSpringForce;
                forces[i - xVertices * 2] += -bendSpringForce;
            }

            //BENDING HORIZONTAL
            if (i % xVertices != 0 && i % xVertices != 1)
            {
                float bendMagnitudeX = (points[i - 2].actualPosition - points[i].actualPosition).magnitude
                                                 - clothSettings.bendingSpringLenght;
                Vector3C bendForceVector = (points[i - 2].actualPosition
                                    - points[i].actualPosition).normalized * bendMagnitudeX * clothSettings.bendingElasticCoef;

                Vector3C bendDampingForce = (points[i - 2].actualPosition - points[i].actualPosition) * clothSettings.bendingDamptCoef;
                Vector3C bendSpringForce = bendForceVector * clothSettings.bendingElasticCoef - bendDampingForce;


                forces[i] += bendSpringForce;
                forces[i - 2] += -bendSpringForce;
            }
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i != 0 && i != xVertices - 1)
                points[i].Euler(settings.gravity + forces[i], dt);
        }
    }

    public void Debug()
    {
        settingsCollision.sphere.Print(Vector3C.blue);

        if (points != null)
        {
            foreach (var item in points)
            {
                item.lastPosition.Print(0.05f);
                item.actualPosition.Print(0.05f);
            }
        }
    }
}
