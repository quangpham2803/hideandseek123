using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerJoker : MonoBehaviourPunCallbacks
{
    public float speed;
    public Animator animator;
    public PlayerSetup owner;
    public float range = 6f;
    public List<PlayerSetup> inRange = new List<PlayerSetup>();
    public NavMeshAgent agent;
    public Text name;
    public bool inGrass;
    public List<Material> defaultMaterial;
    public List<GameObject> ObjectInvisible = new List<GameObject>();
    public Material[] invisibleMaterial;
    public Material[] invisibleOtherTeamMaterial;
    public bool canClick;
    // Start is called before the first frame update
    private void OnEnable()
    {
        agent.enabled = true;
        //StartCoroutine(MoveToTarget());
        inRange.Clear();
        if (owner == null)
            return;
        //GetComponent<PlayerSetup>().rpc.SetDefaultMaterial(GetComponent<PlayerSetup>());
        inGrass = false;
        StartCoroutine(DetectPlayers());
        StartCoroutine(Disapear());
    }
    private void Start()
    {
        GameManager.instance.summonJoker.Add(this);
        GetMaterial();
        GetInvisibleMaterial();
        GetInvisibleOtherTeamMaterial();
        //foreach (PlayerSetup p in GameManager.instance.players)
        //{
        //    if (photonView.Owner == p.photonView.Owner)
        //    {
        //        owner = p;
        //        p.summonJoker = this;
        //        break;
        //    }
        //}
        gameObject.SetActive(false);
    }
    IEnumerator DetectPlayers()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            canClick = false;
            inRange.Clear();
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p != owner
                    && Vector3.Distance(p.transform.position, transform.position) < range
                    && p.player != owner.player && !p.dead && !p.invisible)
                {
                    inRange.Add(p);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void AgentMove(Vector3 _targetPoint)
    {
        transform.LookAt(_targetPoint);
        agent.SetDestination(_targetPoint);
    }
    private void Update()
    {
        if (inGrass && inRange.Count==0)
        {
            if (GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
            {
                InvisibleTeam(invisibleMaterial);
            }
            else
            {
                InvisibleOtherTeam(invisibleOtherTeamMaterial);
            }
            inGrass = false;
        }
        if (inRange.Count != 0)
        {
            if (Vector3.Distance(transform.position,inRange[0].transform.position)< 2.5f)
            {
                canClick = true;
            }
            foreach (GameObject item in owner.skill)
            {
                if (item.TryGetComponent<SwapJokerSkill>(out var comp1) && !comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.activeSelf)
                {
                    comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.SetActive(true);
                }
            }
            animator.SetBool("isRunning", true);
            foreach (PlayerSetup item in inRange)
            {
                if (item.inGrass)
                {
                    item.rpc.SetDefaultMaterial(item);
                }
            }
            Vector3 pos = new Vector3(inRange[0].transform.position.x, 2.5f, inRange[0].transform.position.z);
            AgentMove(pos);
        }
        else
        {
            canClick = false; 
            foreach (PlayerJoker pj in GameManager.instance.summonJoker)
            {
                if (pj.inRange.Count != 0)
                {
                    otherTarget = true;
                    break;
                }
            }
            if (!otherTarget)
            {
                otherTarget = false;
                foreach (GameObject item in owner.skill)
                {
                    if (item.TryGetComponent<SwapJokerSkill>(out var comp1) && comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.activeSelf)
                    {
                        comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.SetActive(false);
                    }
                }
            }

            animator.SetBool("isRunning", false);
            AgentMove(transform.position);
        }
    }
    private bool otherTarget = false;
    [PunRPC]
    public void TimeOut()
    {
        agent.enabled = false;
        foreach (GameObject item in owner.skill)
        {
            if (item.TryGetComponent<SwapJokerSkill>(out var comp1) && comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.activeSelf)
            {
                comp1.GetComponent<SwapJokerSkill>().haveTarget.gameObject.SetActive(false);
            }
        }
        canClick = false;
        gameObject.SetActive(false);
    }

    public IEnumerator Disapear()
    {
        yield return new WaitForSeconds(30f);
        foreach (GameObject item in owner.skill)
        {
            if (item.TryGetComponent<SummonJokerSkill>(out var comp2))
            {
                comp2.GetComponent<SummonJokerSkill>().numberClone--;
                comp2.GetComponent<SummonJokerSkill>().numberCloneText.text = comp2.GetComponent<SummonJokerSkill>().numberClone +"/3";
            }
        }
        photonView.RPC("TimeOut", RpcTarget.AllBuffered);
    }
    public void InvisibleTeam(Material[] material)
    {
        ChangeMaterial(material);
        InvisibleObjectPlayer(true);
    }
    public void InvisibleOtherTeam(Material[] material)
    {
        ChangeMaterial(material);
        InvisibleObjectPlayer(false);
    }
    void ChangeMaterial(Material[] newMat)
    {
        Renderer[] children;
        int index = 0;
        children = GetComponentsInChildren<SkinnedMeshRenderer>();
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
    public void GetMaterial()
    {
        Renderer[] children;
        children = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer rend in children)
        {
            for (var j = 0; j < rend.materials.Length; j++)
            {
                defaultMaterial.Add(rend.materials[j]);
            }
        }
    }
    public void GetInvisibleMaterial()
    {
        invisibleMaterial = GetComponent<PlayerStat>().invisibleMaterial;
    }
    public void GetInvisibleOtherTeamMaterial()
    {
        invisibleOtherTeamMaterial = GetComponent<PlayerStat>().invisibleOtherTeamMaterial;
    }
    public void InvisibleObjectPlayer(bool t)
    {
        foreach (GameObject item in ObjectInvisible)
        {
            item.SetActive(t);
            if ((item.name == "RunEffect") && !inGrass && t)
            {
                item.SetActive(false);
            }
        }
    }
    public void SetDefaultMaterial()
    {
        Renderer[] children;
        int index = 0;
        children = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = defaultMaterial[index];
                index++;
            }
            rend.materials = mats;
        }
    }
}
