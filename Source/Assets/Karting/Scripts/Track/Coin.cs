using System;
using System.Collections;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// This is used to mark out key points on the track that a racer must pass through in order to count as having completed a lap.
    /// </summary>
    [RequireComponent (typeof(CapsuleCollider))]
    public class Coin : MonoBehaviour
    {
        /// <summary>
        /// This is subscribed to by the TrackManager in order to measure a racer's progress around the track.
        /// </summary>
        public event Action<IRacer, Coin> OnKartHitCoin;
        public int secondsToRespawn;

        [Tooltip ("The layers to check for a kart passing through this trigger.")]
        public LayerMask kartLayers;

        MeshRenderer m_Mesh;
        CapsuleCollider m_Collider;
        bool isActive = true;
        bool isActivating = false;

        private void Start()
        {
            m_Mesh = this.GetComponent<MeshRenderer>();
            m_Collider = this.GetComponent<CapsuleCollider>();
        }

        void Update ()
        {
            if (!isActive && !isActivating)
            {
                StartCoroutine(WaitForReenable());
            }
        }


        IEnumerator WaitForReenable()
        {
            isActivating = true;
            //Print the time of when the function is first called.
            yield return new WaitForSeconds(secondsToRespawn);

            //Print the time of when the function is first called.
            
            ActivateCoin();
            isActivating = false;

        }

        void OnTriggerEnter (Collider other)
        {
            if (kartLayers.ContainsGameObjectsLayer (other.gameObject) && m_Mesh.enabled && m_Collider.enabled)
            {
                DeactivateCoin();

                IRacer racer = other.GetComponent<IRacer> ();
                if (racer != null)
                {
                    OnKartHitCoin?.Invoke(racer, this);
                }
            }
        }

        void ActivateCoin() {
            isActive = true;
            m_Mesh.enabled = true;
            m_Collider.enabled = true;
        }
        void DeactivateCoin()
        {
            isActive = false;
            m_Mesh.enabled = false;
            m_Collider.enabled = false;
        }
    }
}