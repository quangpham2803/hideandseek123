using System.Collections;
using UnityEngine;
using Photon.Pun;
namespace CoolBattleRoyaleZone
{
    /// <summary>
    /// Class which controls visualizing of next safe zone circle cylinder or something your own
    /// </summary>

    public class NextSafeZoneVisualizer : MonoBehaviourPun
    {

        // Update is called once per frame
        public void UpdateZone ( Zone.Timer timer )
        {
            if (Zone.Instance != null)
            {
                transform.position = Zone.Instance.NextSafeZone.Position;
                Vector3 tm = Vector3.one * Zone.Instance.NextSafeZone.Radius * 2;
                tm.y = 5;
                transform.localScale = tm;
            }
            else
            {
                Debug.Log("Reset map");
                StartCoroutine(waitForNewGame(timer));
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(SentDataForClient(transform.position, transform.localScale));
                }
            }
        }
        IEnumerator waitForNewGame(Zone.Timer timer)
        {
            yield return new WaitUntil(haveZone);
            UpdateZone(timer);
        }
        bool haveZone()
        {
            return Zone.Instance != null;
        }
        [PunRPC]
        void SentDataForClientCmd(Vector3 position, Vector3 size)
        {
            transform.position = position;
            transform.localScale = size;
        }
        IEnumerator SentDataForClient(Vector3 position, Vector3 size)
        {
            photonView.RPC("SentDataForClientCmd", RpcTarget.AllBuffered, position, size);
            yield return new WaitForSeconds(10f);
            StartCoroutine(SentDataForClient(transform.position, transform.localScale));
        }
    }
}
