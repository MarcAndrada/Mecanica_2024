using System;
using System.Collections.Generic;
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
        public float maxParticlesPerSecond;
        public float minParticlesPerSecond;
        public float maxParticlesLife;
        public float minParticlesLife;
        private float delta;
        public void SetDelta(float dt)
        {
            delta = dt;
        }
        public float GetDelta()
        {
            return delta;
        }
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

        private float delta;
        public void SetDelta(float dt)
        {
            delta = dt;
        }
        public float GetDelta()
        {
            return delta;
        }
    }
    public SettingsCannon settingsCannon;

    [System.Serializable]
    public struct SettingsCollision
    {
        public PlaneC[] planes;
        public SphereC[] spheres;
        public CapsuleC[] capsules;
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
        public void AddForce(Vector3C force, Vector3C gravity)
        {
            acceleration = force / mass;
            acceleration += gravity;
        }

    }

   

    Random rnd = new Random(); 
    Particle[] particles = new Particle[1000];


    public Particle[] Update(float dt)
    {

        CascadeSpawner(dt);
        CannonSpawner(dt);

        SolverEuler(dt);

        DisableParticles(dt);
            
        return particles;
    }
    private void CascadeSpawner(float dt)
    {
        settingsCascade.SetDelta(settingsCascade.GetDelta() + dt);

        if (settingsCascade.GetDelta() >= 1)
        {
            settingsCascade.SetDelta(0);
            int particlesPerSecond = (int)RandomRangeFloats(settingsCascade.minParticlesPerSecond, settingsCascade.maxParticlesPerSecond); 
            for (int i = 0; i < particlesPerSecond; i++)
            {
                for (int j = 0; j < particles.Length; j++)
                {
                    if (!particles[j].active)
                    {
                        particles[j].active = true;
                        float randomForce = RandomRangeFloats(settingsCascade.minForce, settingsCascade.maxForce);
                        Vector3C dir;
                        if (settingsCascade.randomDirection)
                        {
                            float randomX = RandomRangeFloats(-1, 1);
                            float randomY = RandomRangeFloats(-1, 1);
                            float randomZ = RandomRangeFloats(-1, 1);

                            dir = new Vector3C(randomX, randomY, randomZ) * randomForce;
                        }
                        else
                            dir = settingsCascade.direction * randomForce; 
                        
                        particles[j].AddForce(dir, settings.gravity);
                        
                        float randomLifeTime = RandomRangeFloats(settingsCascade.minParticlesLife, settingsCascade.maxParticlesLife);
                        particles[j].lifeTime = randomLifeTime;

                        Vector3C randPoint = settingsCascade.PointA + (settingsCascade.PointB - settingsCascade.PointA) * (float)rnd.NextDouble();
                        particles[j].position = randPoint;

                        break;
                    }
                }
            }

            

        }
    }
    private void CannonSpawner(float dt)
    {
        settingsCannon.SetDelta(settingsCannon.GetDelta() + dt);

        if (settingsCannon.GetDelta() >= 1)
        {
            settingsCannon.SetDelta(0);
            int particlesPerSecond = (int)RandomRangeFloats(settingsCannon.minParticlesPerSecond, settingsCannon.maxParticlesPerSecond);
            for (int i = 0; i < particlesPerSecond; i++)
            {
                for (int j = 0; j < particles.Length; j++)
                {
                    if (!particles[j].active)
                    {
                        particles[j].active = true;
                        float randomForce = RandomRangeFloats(settingsCannon.minForce, settingsCannon.maxForce);
                        Vector3C dir;
                        
                        do
                        {
                            float randomX = RandomRangeFloats(-1, 1);
                            float randomY = RandomRangeFloats(-1, 1);
                            float randomZ = RandomRangeFloats(-1, 1);
                            dir = new Vector3C(randomX, randomY, randomZ);

                        } while (Vector3C.Dot(settingsCannon.Direction, dir) > settingsCannon.angle);

                        particles[j].AddForce(dir, settings.gravity);

                        float randomLifeTime = RandomRangeFloats(settingsCannon.minParticlesLife, settingsCannon.maxParticlesLife);
                        particles[j].lifeTime = randomLifeTime;

                        particles[j].position = settingsCannon.Start;
                        break;
                    }
                }
            }



        }
    }

    private void SolverEuler(float dt)
    {
        for (int i = 0; i < particles.Length; ++i)
        {
            if (!particles[i].active)
            {
                particles[i].position = new Vector3C(1000,1000, 1000);
                particles[i].size = 0.1f;
                particles[i].velocity = Vector3C.zero;
                particles[i].mass = 1;
            }
            else
            {
                particles[i].velocity += particles[i].acceleration * dt;
                particles[i].position += particles[i].velocity * dt;
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
