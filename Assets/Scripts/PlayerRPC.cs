using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using Photon.Realtime;

public class PlayerRPC : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public PlayerSetup player;
    public SkillData data;
    private void Start()
    {
        player = GetComponent<PlayerSetup>();
        data = GetComponent<SkillData>();
        
    }
    //Invisible Skill
    [PunRPC]
    public void Invisible()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            p.changePlayerInGrass = true;
        }
        foreach (Invisible s in GameManager.instance.invisibleObject)
        {
            if (s.owner == player)
            {
                foreach (GameObject r in GameManager.instance.effectRaven)
                {
                    if (r.GetComponent<FearEffect>().owner == player && r.activeSelf && GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
                    {
                        r.SetActive(false);
                    }
                }
                s.owner.invisible = true;
                GameManager.instance.isInvisible = true;
                s.gameObject.SetActive(true);
                s.GetComponent<Invisible>().smoke.transform.position = s.owner.transform.position;
                s.timeInvisible = 0;
            }
        }
    }
    public void InvisibleTeam(Material[] material, PlayerSetup setup)
    {
        ChangeMaterial(material, setup);
        InvisibleObjectPlayer(setup, true);
    }
    public void InvisibleOtherTeam(Material[] material,PlayerSetup setup)
    {
        ChangeMaterial(material, setup);
        InvisibleObjectPlayer(setup,false);
    }
    void ChangeMaterial(Material[] newMat,PlayerSetup setup)
    {
        if (!setup.dead)
        {
            Renderer[] children;
            int index = 0;
            setup.defaultMater = false;
            children = setup.model.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer rend in children)
            {
                var mats = new Material[rend.materials.Length];
                for (var j = 0; j < rend.materials.Length; j++)
                {
                    mats[j] = newMat[index];
                    index++;
                }
                rend.materials = mats;
            }
        }       
    }
    
    public void GetMaterial(PlayerSetup setup)
    {
        Renderer[] children;
        children = setup.model.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer rend in children)
        {
            for (var j = 0; j < rend.materials.Length; j++)
            {
                setup.defaultMaterial.Add(rend.materials[j]);
            }
        }
    }
    public void GetMaterialSeek(PlayerSetup setup)
    {
        Renderer[] children;
        children = setup.seekModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer rend in children)
        {
            for (var j = 0; j < rend.materials.Length; j++)
            {
                setup.defaultMaterialSeek.Add(rend.materials[j]);
            }
        }
    }
    public void GetInvisibleMaterial(PlayerSetup setup)
    {
        setup.invisibleMaterial = setup.model.GetComponent<PlayerStat>().invisibleMaterial;
    }
    public void GetInvisibleOtherTeamMaterial(PlayerSetup setup)
    {
        setup.invisibleOtherTeamMaterial = setup.model.GetComponent<PlayerStat>().invisibleOtherTeamMaterial;
    }
    public void SetDefaultMaterial(PlayerSetup setup)
    {
        if (!setup.dead)
        {
            Renderer[] children;
            int index = 0;
            setup.defaultMater = true;
            children = setup.model.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer rend in children)
            {
                var mats = new Material[rend.materials.Length];
                for (var j = 0; j < rend.materials.Length; j++)
                {
                    mats[j] = setup.defaultMaterial[index];
                    index++;
                }
                rend.materials = mats;
            }
            InvisibleObjectPlayer(setup, true);
        }
    }
    #region Run Skill
    //Run Skill
    [PunRPC]
    public void FastRunSkill(bool t)
    {
        player.run = t;
        player.pAnimator.SetBool("isRunningSkill", t);
        if (t)
        {
            player.speedUp = data.runSkillData.runningSpeed;
            
        }
        else
        {
            player.speedUp = 0;
            
        }
    }
    public IEnumerator Run()
    {
        yield return new WaitForSeconds(data.runSkillData.timeDuration);
        photonView.RPC("FastRunSkill", RpcTarget.AllBuffered, false);
    }
    [PunRPC]
    public void ImproveLegSkill(bool t)
    {
        player.pAnimator.SetBool("isRunningSkill", t);
        if (t)
        {
            player.speedUp = data.runSkillData.improveSpeed;
            player.isImmute = true;
        }
        else
        {
            player.speedUp = 0;
            player.isImmute = false;
        }
    }
    public IEnumerator Improve()
    {
        yield return new WaitForSeconds(data.runSkillData.timeDuration2);
        photonView.RPC("ImproveLegSkill", RpcTarget.AllBuffered, false);
    }
    public bool isDash = false;
    [PunRPC]
    void ActiveDash(bool t)
    {
        isDash = t;
        player.pAnimator.SetBool("isRunningSkill", t);
    }
    [PunRPC]
    public void FollowSkill(float time, float range, float _speedSlow)
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player != player.player && Vector3.Distance(p.transform.position, player.transform.position) <= range)
            {
                p.rpc.StartCoroutine(Follow(time, player.transform, p, _speedSlow));
            }
        }
    }
    public bool isBlackHoleOn;
    [PunRPC]
    public void SummonBlackHole()
    {
        StartCoroutine(BlackHoleDuration());
    }
    IEnumerator BlackHoleDuration()
    {
        isBlackHoleOn = true;
        yield return new WaitForSeconds(5f);
        isBlackHoleOn = false;
    }
    IEnumerator Follow(float time, Transform target, PlayerSetup p, float _speedSlow)
    {
        p.isFollow = true;
        p.agentPlayer.enabled = true;
        p.targetFollow = target;
        if (p.photonView.IsMine)
        {
            GameManager.instance.debuff.gameObject.SetActive(false);
            GameManager.instance.debuff.gameObject.SetActive(true);
            GameManager.instance.debuff.text = "<incr>Follow";
            GameManager.instance.debuff.color = GameManager.instance.colorDebuff;
        }
        p.speedDown += _speedSlow;
        yield return new WaitForSeconds(time);     
        p.isFollow = false;
        p.agentPlayer.enabled = false;
        p.targetFollow = null;
        p.speedDown -= _speedSlow;
        if (p.photonView.IsMine)
        {
            if (GameManager.instance.debuff.text == "Follow")
            {
                GameManager.instance.debuff.text = "";
                GameManager.instance.debuff.gameObject.SetActive(false);
            }
        }
    }
    [PunRPC]
    void StunSeek(int id)
    {
        foreach(PlayerSetup seek in GameManager.instance.players)
        {
            if(seek.photonView.ViewID == id)
            {
                seek.StartCoroutine(seek.StunTime(3f));
            }
        }
    }
    #endregion
    #region Joker Skill
    [PunRPC]
    public void SummonJokerSkill(Vector3 pos)
    {
        foreach (PlayerJoker s in GameManager.instance.summonJoker)
        {
            if (s.owner == player && !s.gameObject.activeSelf)
            {
                s.gameObject.SetActive(true);
                s.agent.enabled = false;
                s.transform.position = pos;
                s.agent.enabled = true;
                break;
            }
        }
    }

    [PunRPC]
    public void SwapJokerSkill()
    {
        PlayerJoker clone=null;
        for (int i = GameManager.instance.summonJoker.Count-1; i >=0; i--)
        {
            if (GameManager.instance.summonJoker[i].owner == player && GameManager.instance.summonJoker[i].inRange.Count != 0)
            {
                if (GameManager.instance.summonJoker[i].inRange.Count != 0 && GameManager.instance.summonJoker[i].gameObject.activeSelf)
                {
                    clone = GameManager.instance.summonJoker[i];
                    break;
                }
            }
            else if (GameManager.instance.summonJoker[i].owner == player && GameManager.instance.summonJoker[i].gameObject.activeSelf)
            {
                clone = GameManager.instance.summonJoker[i];
            }
        }
        if (clone!=null)
        {
            clone.GetComponent<NavMeshAgent>().enabled = false;
            //clone.owner.GetComponent<CharacterController>().enabled = false;
            Vector3 temp = clone.owner.transform.position;
            clone.owner.transform.position = clone.transform.position;
            clone.owner.transform.rotation = Quaternion.identity;
            clone.transform.position = temp;
            //clone.owner.GetComponent<CharacterController>().enabled = true;
            clone.GetComponent<NavMeshAgent>().enabled = true;
            if (clone.owner.inGrass)
            {
                clone.owner.grass.GetComponent<InGrass>().player.Remove(clone.owner.gameObject);
                clone.owner.grass = null;
                clone.owner.inGrass = false;
                clone.owner.isWall = false;
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    p.changePlayerInGrass = true;
                    p.playerInRange.Remove(clone.owner);
                }
            }
            foreach (GameObject item in clone.owner.skill)
            {
                if (item.TryGetComponent<SummonJokerSkill>(out var comp2))
                {
                    comp2.GetComponent<SummonJokerSkill>().numberClone--;
                    comp2.GetComponent<SummonJokerSkill>().numberCloneText.text = comp2.GetComponent<SummonJokerSkill>().numberClone + "/3";
                }
            }
            clone.agent.enabled = false;
            foreach (GameObject item in clone.owner.skill)
            {
                if (item.TryGetComponent<SwapJokerSkill>(out var comp1) && comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.activeSelf)
                {
                    comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.SetActive(false);
                }
            }
            clone.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void JokerThrownToxic(/*Vector3 pos,Quaternion rot*/)
    {
        foreach (JokerBomb item in GameManager.instance.jokerBombList)
        {
            if (item.owner == player)
            {
                item.gameObject.SetActive(true);
                item.transform.position = player.transform.position + player.transform.forward;
                item.transform.rotation = player.shotWeapon.transform.rotation;
            }
        }
    }
    #endregion
    #region Dark Raven
    [PunRPC]
    public void SummonRavenSkill(Vector3 pos)
    {
        if (!player.isBot)
        {
            player.isSummonDarkRaven = true;
            Raven s = player.summonDarkRaven;
            player.agentPlayer.enabled = true;
            s.gameObject.SetActive(true);
            s.transform.position = pos + s.offset;
        }
        else
        {
            player.isSummonDarkRaven = true;
            Raven s = player.summonDarkRaven;
            s.gameObject.SetActive(true);
            s.transform.position = pos + s.offset;
        }
    }

    [PunRPC]
    public void ShotRaven(Vector3 pos)
    {
        foreach (Raven s in GameManager.instance.summonRaven)
        {
            if (s.owner == player)
            {
                if (!player.isBot)
                {
                    s.owner.agentPlayer.enabled = false;
                    s.owner.cam.cinemachine.LookAt = s.owner.transform;
                    s.owner.cam.cinemachine.Follow = s.owner.transform;
                    s.owner.cam.cinemachine.m_Lens.OrthographicSize = 6f;
                }
                s.isPress = true;
                player.isSummonDarkRaven = false;
            }
        }
        foreach (Pulse r in GameManager.instance.shotRaven)
        {
            if (r.GetComponent<Pulse>().owner == player)
            {
                r.gameObject.SetActive(true);
                r.transform.position = pos;
                r.owner.isSummonDarkRaven = false;
            }
        }
    }

    #endregion

    #region Grass
    public void InvisibleObjectPlayer(PlayerSetup setup, bool t)
    {
        foreach (GameObject item in setup.ObjectInvisible)
        {
            item.SetActive(t);
            if (GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Hide)
            {
                if (item.name == "TargetHide")
                {
                    item.SetActive(false);
                }
            }
            else
            {
                if (setup.player== GameManager.HideOrSeek.Seek)
                {
                    if (item.name == "TargetHide")
                    {
                        item.SetActive(false);
                    }
                }
            }
            if ((item.name== "RunEffect" || item.name=="WalkEffect"))
            {
                item.SetActive(false);
            }
        }
    }

    public void SetGrassMaterial(GameObject _Grass, bool inGrass)
    {
        StartCoroutine(wait(_Grass, inGrass));
    }
    public void SetDefautGrassMaterial(GameObject _Grass)
    {
        _Grass.GetComponent<MeshRenderer>().sharedMaterial = data.invisibleSkillData.invisibleTeam;
    }
    IEnumerator wait(GameObject _Grass, bool inGrass)
    {
        if (inGrass)
        {
            for (int i = 0; i < GameManager.instance.grassMaterial.Count; i++)
            {               
                yield return new WaitForSeconds(0.02f);                
                _Grass.GetComponent<MeshRenderer>().sharedMaterial = GameManager.instance.grassMaterial[i];
            }
        }
        else
        {
            for (int i = 0; i < GameManager.instance.grassMaterial.Count; i++)
            {
                yield return new WaitForSeconds(0.02f);
                _Grass.GetComponent<MeshRenderer>().sharedMaterial = GameManager.instance.grassMaterial[GameManager.instance.grassMaterial.Count - i - 1];
            }
        }
    }

    #endregion
    #region Mage Skill
    public bool isShoot;
    public bool isDisactive;
    public bool isActive;
    public bool isSlow;
    //PlayerRPC
    public void WizardShootAction()
    {
        photonView.RPC("WizardShootActionRPC", RpcTarget.AllBuffered, true);
    }
    public void WizardDisActiveAction()
    {
        photonView.RPC("WizardDisActiveRPC", RpcTarget.AllBuffered, true);
    }
    public void WizardActiveAction()
    {
        photonView.RPC("WizardActiveRPC", RpcTarget.AllBuffered, true);
    }
    [PunRPC]
    void WizardShootActionRPC(bool action)
    {
        isShoot = action;
        StartCoroutine(LookToSeek());
    }
    [PunRPC]
    void WizardDisActiveRPC(bool action)
    {
        isDisactive = action;
    }
    [PunRPC]
    void WizardActiveRPC(bool action)
    {
        isActive = action;
    }
    public void TurnOfUI(GameObject UI)
    {
        if (!photonView.IsMine)
        {
            UI.SetActive(false);
        }
    }
    IEnumerator LookToSeek()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player != player.player && Vector3.Distance(p.transform.position, player.transform.position) <= 20)
            {
                player.transform.LookAt(p.transform.position);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }
    [PunRPC]
    public void SlowOn()
    {
        isSlow = true;
    }
    [PunRPC]
    public void SlowOf()
    {
        isSlow = false;
    }
    #endregion
    #region Transform Skill
    public bool isTransformTo;
    public bool isTransformBack;
    public bool isButFakeProp;
    public int id;
    public int currentId;
    [PunRPC]
    public void Fake()
    {
        player.isFake = true;
        StartCoroutine(IEFake());
    }
    IEnumerator IEFake()
    {
        yield return new WaitForSeconds(3f);
        player.isFake = false;
    }
    //PlayerRPC
    public void TransformBodyTo(int giveId)
    {
        photonView.RPC("TransformBodyToCmd", RpcTarget.AllBuffered, giveId, true);
        InvisibleObjectPlayer(player, true);
    }
    public void FakePropPutIn(int giveId)
    {
        photonView.RPC("FakePropPutInCmd", RpcTarget.AllBuffered, giveId, true);
    }
    public void TransformBodyBack(int giveId)
    {        
        photonView.RPC("TransformBodyBackCmd", RpcTarget.AllBuffered, giveId, true);
        InvisibleObjectPlayer(player, false);
    }
    public void SetCurrentIdToZero(bool isTranformBack)
    {
        photonView.RPC("SetCurrentIdToZeroCmd", RpcTarget.AllBuffered, isTranformBack);
    }
    [PunRPC]
    void TransformBodyToCmd(int giveId, bool giveBool)
    {
        player.isTranform = true;
        isTransformTo = giveBool;
        id = giveId;
    }
    [PunRPC]
    void FakePropPutInCmd(int giveId, bool giveBool)
    {
        isButFakeProp = giveBool;
        id = giveId;
    }
    //Transform back to normal body
    [PunRPC]
    void TransformBodyBackCmd(int giveId, bool giveBool)
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            p.changePlayerInGrass = true;
        }
        foreach (GameObject item in player.ObjectInvisible)
        {
            item.SetActive(false);
        }
        player.isTranform = false;
        isTransformBack = giveBool;
        currentId = giveId;
    }
    #endregion
    #region Davros Skill
    [PunRPC]
    public void ScanSkill()
    {
        foreach (ScanDavros r in GameManager.instance.scanDavros)
        {
            if (r.owner == player)
            {
                r.gameObject.SetActive(true);
            }
        }
    }
    public void BandageTossSkill()
    {
        if ((player.photonView.IsMine && !player.isBot) || (player.isBot && PhotonNetwork.IsMasterClient))
        {
            photonView.RPC("BandageTossSkillCmd", RpcTarget.AllBuffered,
                player.transform.position + player.transform.forward,
                player.shotWeapon.transform.rotation);
        }
    }
    [PunRPC]
    public void BandageTossSkillCmd(Vector3 position, Quaternion rotation)
    {
        foreach (Bandage item in GameManager.instance.bandageList)
        {
            if (item.owner == player)
            {
                foreach (AnimatorControllerParameter pA in item.owner.pAnimator.parameters)
                {
                    if (pA.name != "Attack")
                    {
                        item.owner.pAnimator.SetBool(pA.name, false);
                    }
                }
                item.owner.pAnimator.SetBool("Toss",true);
                item.gameObject.SetActive(true);
                item.transform.position = position/*player.transform.position + player.transform.forward*/;
                item.transform.rotation = rotation/*player.shotWeapon.transform.rotation*/;
            }
        }
    }
    [PunRPC]
    public void MuscleCrampSkill()
    {
        foreach (MuscleCramp r in GameManager.instance.muscleCrampList)
        {
            if (r.owner == player)
            {
                r.gameObject.SetActive(true);
            }
        }
    }
    //Run Skill
    //[PunRPC]
    //public void FastRunDavrosSkill(bool t)
    //{
    //    player.run = t;
    //    player.pAnimator.SetBool("isRunningSkill", t);
    //    if (t)
    //    {
    //        player.speedUp = data.davrosRunSkillData.runningSpeed;
    //        if(player == GameManager.instance.mainPlayer)
    //        {
    //            player.cam.GetComponent<CameraFilterPack_Drawing_Manga_FlashWhite>().enabled = true;
    //            player.seekBuff.SetActive(true);
    //        }
            
    //    }
    //    else
    //    {
    //        player.speedUp = 0;
    //        player.StartCoroutine(player.SlowTimeAfterRun(data.davrosRunSkillData.timeslowDuration, player, player.slowEffectAffterRunningObj,data.davrosRunSkillData.slowSpeed));
    //        if (player == GameManager.instance.mainPlayer)
    //        {
    //            player.cam.GetComponent<CameraFilterPack_Drawing_Manga_FlashWhite>().enabled = false;
    //            player.seekBuff.SetActive(false);
    //        }
    //    }
    //}
    //public IEnumerator RunDavros()
    //{        
    //    yield return new WaitForSeconds(data.davrosRunSkillData.timeDuration);
    //    photonView.RPC("FastRunDavrosSkill", RpcTarget.AllBuffered, false);
    //}
    #endregion
}
