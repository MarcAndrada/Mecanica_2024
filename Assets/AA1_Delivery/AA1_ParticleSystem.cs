using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering.VirtualTexturing;
using static AA1_ParticleSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[System.Serializable]
public class AA1_ParticleSystem
{
    [System.Serializable]
    public struct Settings
    {
        public Vector3C gravity;
        public float bounce;
        public int particlePoolCapacity;
    }
    public Settings settings;

    [System.Serializable]
    public struct SettingsCascade
    {
        public Vector3C PointA;
        public Vector3C PointB;
        public Vector3C direction;
        public bool randomDirection;
        public float maxForce;
        public float minForce;
        public float minParticlesPerSecond;
        public float maxParticlesPerSecond;
        public float maxParticlesLife;
        public float minParticlesLife;
        public float particleBatch;
    }
    public SettingsCascade settingsCascade;

    [System.Serializable]
    public struct SettingsCannon
    {
        public Vector3C Start;
        public Vector3C Direction;
        public float angle;
        public float maxForce;
        public float minForce;
        public float maxParticlesPerSecond;
        public float minParticlesPerSecond;
        public float maxParticlesLife;
        public float minParticlesLife;
        public float particleBatch;
    }
    public SettingsCannon settingsCannon;

    [System.Serializable]
    public struct SettingsCollision
    {
        public PlaneC[] planes;
        public SphereC[] spheres;
        public CapsuleC[] capsules;
        public float collisionFactor;
    }
    public SettingsCollision settingsCollision;

    //force = suma de fuerzas 
    //aceleracion = fuerza/masa
    //velocity = velocity + aceleracion * delta;
    //Posicion = posicion + velocidad * delta;

    public struct Particle
    {
        public Vector3C position;
        public float size;
        public Vector3C velocity;
        public Vector3C acceleration;
        public float mass;
        public bool active;
        public float lifeTime;
        public Vector3C lastPosition;
        public void AddForce(Vector3C force, Vector3C gravity)
        {
            acceleration = force / mass;
            acceleration += gravity;
        }

    }

    System.Random rnd = new System.Random(); 
    Particle[] particles;

    public Particle[] Update(float dt)
    {
        Initialize();


        CascadeSpawner(dt);
        CannonSpawner(dt);

        SolverEuler(dt);

        DisableParticles(dt);
            
        return particles;
    }
    private void Initialize()
    {
        if (particles != null)
            return;

        particles = new Particle[settings.particlePoolCapacity];

    }

    private void CascadeSpawner(float dt)
    {
        settingsCascade.particleBatch += RandomRangeFloats(settingsCascade.minParticlesPerSecond, settingsCascade.maxParticlesPerSecond) * dt; ;


        for (int j = 0; j < particles.Length; j++)
        {
            if (particles[j].active)
                continue;


            if (settingsCascade.particleBatch < 1)
                break;

            particles[j].active = true;
            float randomForce = RandomRangeFloats(settingsCascade.minForce, settingsCascade.maxForce);
            Vector3C dir;
            if (settingsCascade.randomDirection)
            {
                float randomX = RandomRangeFloats(-1, 1);
                float randomY = RandomRangeFloats(-1, 1);
                float randomZ = RandomRangeFloats(-1, 1);

                dir = new Vector3C(randomX, randomY, randomZ).normalized * randomForce;
            }
            else
                dir = settingsCascade.direction * randomForce;

            particles[j].AddForce(dir, settings.gravity);

            float randomLifeTime = RandomRangeFloats(settingsCascade.minParticlesLife, settingsCascade.maxParticlesLife);
            particles[j].lifeTime = randomLifeTime;

            Vector3C randPoint = settingsCascade.PointA + (settingsCascade.PointB - settingsCascade.PointA) * (float)rnd.NextDouble();
            particles[j].position = randPoint;

            settingsCascade.particleBatch--;
        }
            
    }
    private void CannonSpawner(float dt)
    {

        settingsCannon.particleBatch += RandomRangeFloats(settingsCannon.minParticlesPerSecond, settingsCannon.maxParticlesPerSecond) * dt; ;


        for (int j = 0; j < particles.Length; j++)
        {
            if (particles[j].active)
                continue;


            if (settingsCannon.particleBatch < 1)
                break;

            particles[j].active = true;
            float randomForce = RandomRangeFloats(settingsCannon.minForce, settingsCannon.maxForce);
            Vector3C dir;
            //Valor inicial para el dot (no es valor final)
            float dot = -100;
            do
            {
                float randomX = RandomRangeFloats(-1, 1);
                float randomY = RandomRangeFloats(-1, 1);
                float randomZ = RandomRangeFloats(-1, 1);
                dir = new Vector3C(randomX, randomY, randomZ).normalized;

                dot = Vector3C.Dot(settingsCannon.Direction.normalized, dir);

            } while (dot < settingsCannon.angle);

            particles[j].AddForce(dir * randomForce, settings.gravity);

            float randomLifeTime = RandomRangeFloats(settingsCannon.minParticlesLife, settingsCannon.maxParticlesLife);
            particles[j].lifeTime = randomLifeTime;

            particles[j].position = settingsCannon.Start;

            settingsCannon.particleBatch--;
        }

    }

    private void SolverEuler(float dt)
    {
        for (int i = 0; i < particles.Length; ++i)
        {
            if (!particles[i].active)
            {
                particles[i].position = new Vector3C(1000, 1000, 1000);
                particles[i].size = 0.025f;
                particles[i].velocity = Vector3C.zero;
                particles[i].mass = 1;
            }
            else
            {
                particles[i].lastPosition = particles[i].position;
                particles[i].velocity += particles[i].acceleration * dt;
                particles[i].position += particles[i].velocity * dt;
                CheckCollisions(i);
            }
        }
    }

    private void CheckCollisions(int index)
    {
        for (int i = 0; i < settingsCollision.planes.Length; i++)
        {
            if (settingsCollision.planes[i].DistanceToPoint(particles[index].position) <= settingsCollision.collisionFactor)
            {
                float n = (particles[index].velocity * settingsCollision.planes[i].normal) / settingsCollision.planes[i].normal.magnitude;
                Vector3C normalVelocity = settingsCollision.planes[i].normal * n;
                Vector3C tangentVelocity = particles[index].velocity - normalVelocity;
                particles[index].velocity = -normalVelocity + tangentVelocity;
            }
        }
    }

    private void DisableParticles(float dt)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].active)
            {
                particles[i].lifeTime -= dt;
                if (particles[i].lifeTime <= 0)
                {
                    particles[i].active = false;
                }
            }
        }
    }
    public void Debug()
    {
        foreach (var item in settingsCollision.planes)
        {
            item.Print(Vector3C.red);
        }
        foreach (var item in settingsCollision.capsules)
        {
            item.Print(Vector3C.green);
        }
        foreach (var item in settingsCollision.spheres)
        {
            item.Print(Vector3C.blue);
        }
    }
    private float RandomRangeFloats(float min, float max)
    {
        float randomValue = min + ((float)rnd.NextDouble() * (max - min));
        return randomValue;
    }
}
