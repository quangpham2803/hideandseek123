using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace CoolBattleRoyaleZone
{
	/// <summary>
	/// Class which controls health of simple characters
	/// </summary>

	public class SimpleHealth : MonoBehaviourPun
	{
        PlayerSetup owner;
		public float Health = 6; // Default amount of health
		//public Text  HealthText;   // Text for showing current amount of health

		private bool _wait;
        public bool isOut;
		private void Start ( )
		{
			// Setup default color to text
			//if ( HealthText ) HealthText.text = "Health: <color=green>" + Health + "</color>";
            owner = GetComponent<PlayerSetup>();
            isOut = false;
		}

		private void Update ( )
		{
            if(GameManager.instance.zone.activeSelf && !owner.dead)
            {
                // Getting zone current safe zone values
                var zonePos = Zone.Instance.CurrentSafeZone.Position;
                var zoneRadius = Zone.Instance.CurrentSafeZone.Radius;
                // Checking distance between player and circle
                var dstToZone = Vector3.Distance(new Vector3(transform.position.x, zonePos.y, transform.position.z),
                                                   zonePos);
                // Checking if we inner of circle or not by radius and if not, start applying damage to health
                if (dstToZone > zoneRadius && !_wait && !owner.dead)
                {
                    isOut = true;
                    if (GameManager.instance.mainPlayer == owner)
                    {
                        if (!GameManager.instance.alertZone.activeSelf)
                        {
                            GameManager.instance.alertZone.SetActive(true);
                            GameManager.instance.currentSafeZone.enabled = true;
                        }
                    }
                    else if (GameManager.instance.mainPlayer.player != owner.player)
                    {
                        if (!owner.target.enabled)
                        {
                            owner.target.enabled = true;
                        }
                    }
                    if (owner.isBot)
                    {
                        if (owner.player == GameManager.HideOrSeek.Hide)
                        {
                            owner.GetComponent<Player>().caseBot = Player.CaseBot.GrassTable;
                            owner.GetComponent<Player>().CaseHiderBot();
                        }
                        else
                        {
                            owner.GetComponent<Player>().caseBot = Player.CaseBot.Grass;
                            owner.GetComponent<Player>().CaseSeekerBot();
                        }
                    }
                    StartCoroutine(DoDamageCoroutine());
                }
                if (dstToZone < zoneRadius && isOut == true)
                {
                    if(GameManager.instance.mainPlayer == owner)
                    {
                        GameManager.instance.alertZone.SetActive(false);
                        GameManager.instance.currentSafeZone.enabled = false;
                    }
                    else if (GameManager.instance.mainPlayer.player != owner.player)
                    {
                        owner.target.enabled = false;
                    }
                    Health = 6 + Zone.Instance.CurStep;
                    isOut = false;
                }                   
            }
		}

		// Method for waiting time between applying damage
		private IEnumerator DoDamageCoroutine ( )
		{
			_wait = true;
			DoDamage ( );
			yield return new WaitForSeconds ( 1 ); // Waiting between damages.
			_wait = false;
		}

		// Method for applying damage to health
		private void DoDamage ( )
		{
            if (owner.dead == false && DoorGame.door.isWatching == false)
            {
                float curSpeed = owner.currentSpeed;
                if (owner.player == GameManager.HideOrSeek.Hide)
                {
                    Health -= 1; 
                    var hpColor = Health > 3 ? "<color=green>" : "<color=red>";
                    if (Health <= 0)
                    {
                        if ((owner.photonView.IsMine && !owner.isBot) ||(owner.isBot && PhotonNetwork.IsMasterClient))
                        {
                            owner.photonView.RPC("Catch", RpcTarget.AllBuffered, "Zone", Vector3.zero);
                            Health = 3;
                            if (GameManager.instance.mainPlayer == owner)
                            {
                                if (GameManager.instance.alertZone.activeSelf)
                                {
                                    GameManager.instance.alertZone.SetActive(false);
                                    GameManager.instance.currentSafeZone.enabled = false;
                                }
                            }
                            else if (GameManager.instance.mainPlayer.player != owner.player)
                            {
                                if (owner.target.enabled)
                                {
                                    owner.target.enabled = false;
                                }
                            }
                        }                  
                    }
                    if (GameManager.instance.mainPlayer == owner)
                    {
                        GameManager.instance.alertTimeZone.text = Health.ToString();
                    }
                }
                else
                {
                    owner.StartCoroutine(owner.FastZoneTime(0.5f, owner, curSpeed * 0.3f));
                    if (GameManager.instance.mainPlayer == owner)
                    {
                        GameManager.instance.alertTimeZone.text = "";
                    }
                }
            }
            else
            {
                if (GameManager.instance.mainPlayer == owner)
                {
                    if (GameManager.instance.alertZone.activeSelf)
                    {
                        GameManager.instance.alertZone.SetActive(false);
                    }
                }
                else if (GameManager.instance.mainPlayer.player != owner.player)
                {
                    if (owner.target.enabled)
                    {
                        owner.target.enabled = false;
                    }
                }
            }
        }
	}
}
