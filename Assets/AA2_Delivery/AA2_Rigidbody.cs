using System;

using static AA1_ParticleSystem;

[System.Serializable]
public class AA2_Rigidbody
{
    [System.Serializable]
    public struct Settings
    {
        public Vector3C gravity;
        public float bounce;
    }
    public Settings settings;

    [System.Serializable]
    public struct SettingsCollision
    {
        public PlaneC[] planes;
    }
    public SettingsCollision settingsCollision;

    [System.Serializable]
    public struct CubeRigidbody
    {
        public Vector3C lastPosition;
        public Vector3C position;
        public Vector3C acceleration;
        public Vector3C linearVelocity;
        public Vector3C angularVelocity;
        public Vector3C size;
        public Vector3C force;
        public Vector3C euler;
        public float mass;
        public float density;

        public Vector3C[] vertexs;

        public CubeRigidbody(Vector3C _position, Vector3C _size, Vector3C _euler, float _density)
        {
            position = _position;
            size = _size;
            euler = _euler;
            density = _density;

            mass = size.x * density;

            lastPosition = Vector3C.zero;
            acceleration = Vector3C.zero;
            linearVelocity = Vector3C.zero;
            angularVelocity = Vector3C.zero;
            force = Vector3C.zero;
            vertexs = new Vector3C[8];
        }
        public void SolverEuler(float dt, Vector3C gravity)
        {
            lastPosition = position;

            acceleration = (force / mass) + gravity;
            linearVelocity += acceleration * dt;
            position += linearVelocity * dt;
            euler += angularVelocity * dt;

            force = Vector3C.zero;
        }
        public void CheckCollisionWithPlanes(PlaneC[] planes, float bounce)
        {
            CalculateVertexsPositions();

            for (int i = 0; i < planes.Length; i++)
            {
                for (int j = 0; j < vertexs.Length; j++)
                {
                    if (CheckIfCollideWithPlane(planes[i], vertexs[j]))
                    {
                        DoCollisionWithPlane(planes[i], vertexs[j], bounce);
                    }
                }

                
            }
        }
        private bool CheckIfCollideWithPlane(PlaneC plane, Vector3C vertex)
        {
            Vector3C directionPlaneToBody = (vertex + position) - plane.position;
            double distance = Vector3C.Dot(plane.normal, directionPlaneToBody);

            return distance < 0;

        }
        private void DoCollisionWithPlane(PlaneC plane, Vector3C vertex,  float bounce)
        {
            float planeDot = Vector3C.Dot(plane.normal, lastPosition - plane.position);
            Vector3C newPosition = plane.IntersectionWithLine(new LineC(lastPosition, position)) + plane.normal * planeDot;
            position = newPosition;

            float n = (linearVelocity * plane.normal) / plane.normal.magnitude;
            Vector3C normalVelocity = plane.normal * n;
            Vector3C tangentVelocity = linearVelocity - normalVelocity;
            linearVelocity = (-normalVelocity + tangentVelocity) * bounce;
        }

        private void CalculateVertexsPositions()
        {
            vertexs[0] = new Vector3C(+ size.x / 2, + size.y / 2, + size.z / 2);
            vertexs[1] = new Vector3C(- size.x / 2, + size.y / 2, + size.z / 2);
            vertexs[2] = new Vector3C(+ size.x / 2, + size.y / 2, - size.z / 2);
            vertexs[3] = new Vector3C(- size.x / 2, + size.y / 2, - size.z / 2);
            vertexs[4] = new Vector3C(+ size.x / 2, - size.y / 2, + size.z / 2);
            vertexs[5] = new Vector3C(- size.x / 2, - size.y / 2, + size.z / 2);
            vertexs[6] = new Vector3C(+ size.x / 2, - size.y / 2, - size.z / 2);
            vertexs[7] = new Vector3C(- size.x / 2, - size.y / 2, - size.z / 2);


        
            MatrixC zMatrix = MatrixC.RotateZ(euler.z);
            for (int i = 0; i < vertexs.Length; i++)
            {
                vertexs[i] = zMatrix * vertexs[i];
            }
            MatrixC xMatrix = MatrixC.RotateX(euler.x);
            for (int i = 0; i < vertexs.Length; i++)
            {
                vertexs[i] = xMatrix * vertexs[i];

            }
            MatrixC yMatrix = MatrixC.RotateY(euler.y);
            for (int i = 0; i < vertexs.Length; i++)
            {
                vertexs[i] = yMatrix * vertexs[i];

            }
        }
    }
    public CubeRigidbody crb = new CubeRigidbody(Vector3C.zero, new(.1f,.1f,.1f), Vector3C.zero, 1f);
    
    public void Update(float dt)
    {
        crb.SolverEuler(dt, settings.gravity);
        crb.CheckCollisionWithPlanes(settingsCollision.planes, settings.bounce);
    }



    public void Debug()
    {
        foreach (var item in settingsCollision.planes)
        {
            item.Print(Vector3C.red);
        }

        
    }
}
