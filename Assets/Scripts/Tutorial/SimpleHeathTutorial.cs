using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimpleHeathTutorial : MonoBehaviour
{
    PlayerSetupTutorial owner;
    public float Health = 6; // Default amount of health
                             //public Text  HealthText;   // Text for showing current amount of health
    private bool _wait;
    public bool isOut;
    private void Start()
    {
        // Setup default color to text
        //if ( HealthText ) HealthText.text = "Health: <color=green>" + Health + "</color>";
        owner = GetComponent<PlayerSetupTutorial>();
        isOut = false;
    }
    private void DoDamage()
    {
        if (owner.dead == false /*&& DoorGame.door.isWatching == false*/)
        {
            float curSpeed = owner.currentSpeed;
            if (owner.player == GameManagerTutorial.HideOrSeek.Hide)
            {
                Health -= 1;
                var hpColor = Health > 3 ? "<color=green>" : "<color=red>";
                if (Health <= 0)
                {
                    if (owner == GameManagerTutorial.instance.mainPlayer)
                    {
                        owner.Catch("Zone", Vector3.zero);
                        Health = 3;
                        if (GameManagerTutorial.instance.mainPlayer == owner)
                        {
                            if (GameManagerTutorial.instance.alertZone.activeSelf)
                            {
                                GameManagerTutorial.instance.alertZone.SetActive(false);
                            }
                        }
                        else if (GameManagerTutorial.instance.mainPlayer.player != owner.player)
                        {
                            if (owner.target.enabled)
                            {
                                owner.target.enabled = false;
                            }
                        }
                    }
                }
                if (GameManagerTutorial.instance.mainPlayer == owner)
                {
                    GameManagerTutorial.instance.alertTimeZone.text = Health.ToString();
                }
            }
        }
        else
        {
            if (GameManagerTutorial.instance.mainPlayer == owner)
            {
                if (GameManagerTutorial.instance.alertZone.activeSelf)
                {
                    GameManagerTutorial.instance.alertZone.SetActive(false);
                }
            }
            else if (GameManagerTutorial.instance.mainPlayer.player != owner.player)
            {
                if (owner.target.enabled)
                {
                    owner.target.enabled = false;
                }
            }
        }
    }
    private void Update()
    {
        if (GameManagerTutorial.instance.zone.activeSelf && !owner.dead )
        {
            // Getting zone current safe zone values
            var zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
            var zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
            // Checking distance between player and circle
            var dstToZone = Vector3.Distance(new Vector3(transform.position.x, zonePos.y, transform.position.z),
                                               zonePos);
            // Checking if we inner of circle or not by radius and if not, start applying damage to health
            if (dstToZone > zoneRadius && !_wait && !owner.dead)
            {
                if (owner.isBot)
                {
                    isOut = true;
                    StartCoroutine(DoDamageCoroutine());
                    owner.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.Grass;
                    owner.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
                    return;
                }
                if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene2)
                {
                    owner.isZone = true;
                }
                isOut = true;
                if (GameManagerTutorial.instance.mainPlayer == owner)
                {
                    if (!GameManagerTutorial.instance.alertZone.activeSelf)
                    {
                        GameManagerTutorial.instance.alertZone.SetActive(true);
                        //GameManagerTutorial.instance.currentSafeZone.enabled = true;
                    }
                }
                else if (GameManagerTutorial.instance.mainPlayer.player != owner.player)
                {
                    if (!owner.target.enabled)
                    {
                        owner.target.enabled = true;
                    }
                }
                StartCoroutine(DoDamageCoroutine());
            }
            if (dstToZone < zoneRadius)
            {
                if (GameManagerTutorial.instance.mainPlayer == owner)
                {
                    if (GameManagerTutorial.instance.alertZone.activeSelf)
                    {
                        GameManagerTutorial.instance.alertZone.SetActive(false);
                        //GameManagerTutorial.instance.currentSafeZone.enabled = true;
                    }
                }
                Health = 6 + ZoneTutorial.Instance.CurStep;
                isOut = false;
            }
            if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene2)
            {
                var zonePos2 = ZoneTutorial.Instance.NextSafeZone.Position;
                var zoneRadius2 = ZoneTutorial.Instance.NextSafeZone.Radius;
                // Checking distance between player and circle
                var dstToZone2 = Vector3.Distance(new Vector3(transform.position.x, zonePos.y, transform.position.z),
                                                   zonePos);
                if (dstToZone2 < zoneRadius2)
                {
                    owner.isZone = false;
                }
                if (dstToZone < zoneRadius && isOut == true)
                {
                    if (GameManagerTutorial.instance.mainPlayer == owner)
                    {
                        GameManagerTutorial.instance.alertZone.SetActive(false);
                        //GameManagerTutorial.instance.currentSafeZone.enabled = false;
                    }
                    else if (GameManagerTutorial.instance.mainPlayer.player != owner.player)
                    {
                        owner.target.enabled = false;
                    }
                    Health = 6 + ZoneTutorial.Instance.CurStep;
                    isOut = false;
                }
            }
            
        }
    }
    // Method for waiting time between applying damage
    private IEnumerator DoDamageCoroutine()
    {
        _wait = true;
        if (!owner.isBot)
        {
            DoDamage();
        }
        yield return new WaitForSeconds(1); // Waiting between damages.
        _wait = false;
    }
}