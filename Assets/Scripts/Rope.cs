using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace my
{
    public class Rope : MonoBehaviour
    {
        public Vector3 gravity = new Vector3(0, -9.8f, 0);

        public float stiffness = 10000f;

        public float damping = 0.001f;

        public int particleNum = 1;

        private MassParticle[] _particles;
        private Spring[] _springs;

        // Start is called before the first frame update
        void Start()
        {
            _particles = new MassParticle[particleNum];
            _springs = new Spring[particleNum - 1];

            GameObject go = new GameObject("p0");
            MassParticle p = go.AddComponent<MassParticle>();
            p.mass = 1;
            p.pined = true;
            p.transform.parent = this.transform;
            p.transform.localPosition = Vector3.zero;
            _particles[0] = p;

            for (int i = 1; i < particleNum; i++)
            {
                GameObject goS = new GameObject("p" + i);
                MassParticle pS = goS.AddComponent<MassParticle>();
                pS.mass = 1;
                pS.transform.parent = this.transform;
                pS.transform.localPosition = new Vector3(0, -i, 0);
                _particles[i] = pS;

                Spring s = new Spring();
                s.particleA = _particles[i - 1];
                s.particleB = _particles[i];
                s.restLength = 1f;
                _springs[i - 1] = s;
            }
           
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _springs.Length; i++)
            {
                Spring s = _springs[i];

                MassParticle pA = s.particleA;
                MassParticle pB = s.particleB;

                if (!pA.pined)
                {
                    pA.forces += gravity * pA.mass;

                    Vector3 offsetA = pB.transform.position - pA.transform.position;

                    pA.forces += stiffness * (offsetA.magnitude - s.restLength) * offsetA.normalized;
                }

                if (!pB.pined)
                {
                    pB.forces += gravity * pB.mass;

                    Vector3 offsetB = pA.transform.position - pB.transform.position;

                    pB.forces += stiffness * (offsetB.magnitude - s.restLength) * offsetB.normalized;
                }
            }

            for (int i = 0; i < _particles.Length; i++)
            {
                MassParticle p = _particles[i];
                p.velocity *= (1 - damping);
                p.velocity += Time.fixedDeltaTime * (p.forces / p.mass);

                p.transform.position += p.velocity * Time.fixedDeltaTime;

                p.forces = Vector3.zero;
            }
        }

        private void OnDrawGizmos()
        {
            if (_particles != null && _particles.Length > 0)
            {
                Gizmos.color = Color.yellow;

                for (int i = 0; i < _particles.Length; i++)
                {
                    Gizmos.DrawSphere(_particles[i].transform.position, 0.2f);
                }
            }
        }
    }
}


