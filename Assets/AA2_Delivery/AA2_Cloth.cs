using System;
using System.Collections;
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
        //int yVertices = settings.yPartSize + 1;

        Vector3C[] forces = new Vector3C[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            StructuralSpring(forces, i, xVertices);
            ShearSpring(forces, i, xVertices);
            BendingSpring(forces, i, xVertices);
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i != 0 && i != xVertices - 1)
            {
                points[i].Euler(settings.gravity + forces[i], dt);
                CheckSphereCollisions(i);
            }
        }
    }

    private void CheckSphereCollisions(int index)
    {


        if (settingsCollision.sphere.IsInside(points[index].actualPosition))
        {
            Vector3C distanceToParticle = (points[index].actualPosition - settingsCollision.sphere.position).normalized;
            Vector3C collisionPoint = settingsCollision.sphere.position + distanceToParticle * settingsCollision.sphere.radius;
            PlaneC plane = new PlaneC(collisionPoint, distanceToParticle);

            points[index].actualPosition = collisionPoint;
            Vector3C tangent = points[index].velocity - plane.normal * Vector3C.Dot(plane.normal,  points[index].velocity);
            points[index].velocity = new Vector3C(tangent.x, 0, tangent.z);
        }
    }

    private void StructuralSpring(Vector3C[] forces, int index, int vertices)
    {
        //STRUCTURAL VERTICAL
        if (index > vertices - 1)
        {
            float structMagnitudeY = (points[index - vertices].actualPosition - points[index].actualPosition).magnitude
                                             - clothSettings.structuralSpringLenght;
            Vector3C structForceVector = (points[index - vertices].actualPosition
                                - points[index].actualPosition).normalized * structMagnitudeY * clothSettings.structuralElasticCoef;

            Vector3C structDampingForce = (points[index].velocity - points[index - vertices].velocity) * clothSettings.structuralDamptCoef;
            Vector3C structSpringForce = structForceVector * clothSettings.structuralElasticCoef - structDampingForce;


            forces[index] += structSpringForce;
            forces[index - vertices] += -structSpringForce;
        }

        //STRUCTURAL HORIZONTAL
        if (index % vertices != 0)
        {
            float structMagnitudeX = (points[index - 1].actualPosition - points[index].actualPosition).magnitude
                                             - clothSettings.structuralSpringLenght;
            Vector3C structForceVector = (points[index - 1].actualPosition
                                - points[index].actualPosition).normalized * structMagnitudeX * clothSettings.structuralElasticCoef;

            Vector3C structDampingForce = (points[index].velocity - points[index - 1].velocity) * clothSettings.structuralDamptCoef;
            Vector3C structSpringForce = structForceVector * clothSettings.structuralElasticCoef - structDampingForce;

            forces[index] += structSpringForce;
            forces[index - 1] += -structSpringForce;
        }
    }
    private void ShearSpring(Vector3C[] forces, int index, int vertices)
    {
        //SHEAR
        if (index > vertices - 1 && index % vertices - 1 != 0)
        {
            float shearMagnitude = (points[index - vertices + 1].actualPosition - points[index].actualPosition).magnitude
                                             - clothSettings.shearSpringLenght;
            Vector3C shearForceVector = (points[index - vertices + 1].actualPosition
                                - points[index].actualPosition).normalized * shearMagnitude * clothSettings.shearElasticCoef;


            Vector3C shearDampingForce = (points[index].velocity - points[index - vertices + 1].velocity) * clothSettings.shearDamptCoef;
            Vector3C shearSpringForce = shearForceVector * clothSettings.shearElasticCoef - shearDampingForce;

            forces[index] += shearSpringForce;
            forces[index - vertices + 1] += -shearSpringForce;
        }
    }
    private void BendingSpring(Vector3C[] forces, int index, int vertices)
    {
        //BENDING VERTICAL
        if (index > vertices * 2 - 1)
        {
            float bendMagnitudeY = (points[index - vertices * 2].actualPosition - points[index].actualPosition).magnitude
                                             - clothSettings.bendingSpringLenght;
            Vector3C bendForceVector = (points[index - vertices * 2].actualPosition
                                - points[index].actualPosition).normalized * bendMagnitudeY * clothSettings.bendingElasticCoef;

            Vector3C bendDampingForce = (points[index].velocity - points[index - vertices * 2].velocity) * clothSettings.bendingDamptCoef;
            Vector3C bendSpringForce = bendForceVector * clothSettings.bendingElasticCoef - bendDampingForce;


            forces[index] += bendSpringForce;
            forces[index - vertices * 2] += -bendSpringForce;
        }

        //BENDING HORIZONTAL
        if (index % vertices != 0 && index % vertices != 1)
        {
            float bendMagnitudeX = (points[index].actualPosition - points[index - 2].actualPosition).magnitude
                                             - clothSettings.bendingSpringLenght;
            Vector3C bendForceVector = (points[index - 2].actualPosition
                                - points[index].actualPosition).normalized * bendMagnitudeX * clothSettings.bendingElasticCoef;

            Vector3C bendDampingForce = (points[index].velocity - points[index - 2].velocity) * clothSettings.bendingDamptCoef;
            Vector3C bendSpringForce = bendForceVector * clothSettings.bendingElasticCoef - bendDampingForce;


            forces[index] += bendSpringForce;
            forces[index - 2] += -bendSpringForce;
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
