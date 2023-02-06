using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TransformSkill : MonoBehaviourPun
{
    GameObject playerBody;
    [SerializeField] GameObject[] propList;
    [SerializeField] GameObject[] propFakeList;
    //public GameObject currentFakeProp;
    //Props   
    public float currentTimeTransform;

    bool isGoThrought;
    List<GameObject> hideObject = new List<GameObject>();
    //Data
    public TransformSkillData data;

    //Data form PlayerRPC
    public PlayerRPC rpc;
    public checkcollision checkTrigger;
    // Start is called before the first frame update
    void Start()
    {
        isGoThrought = false;
        rpc = GetComponentInParent<PlayerRPC>();
        rpc.id = 0;
        rpc.currentId = 0;
        currentTimeTransform = 0;
        rpc.isTransformTo = false;
        rpc.isTransformBack = false;
        hideObject = GetComponentInParent<PlayerSetup>().ObjectInvisible;
        playerBody = GetComponentInParent<PlayerSetup>().model;
        foreach(GameObject p in propFakeList)
        {
            try
            {
                p.GetComponent<FakeProp>().skillHost = this;
            }
            catch
            {
                p.GetComponent<CloneTransformPlayer>().skillHost = this;
            }
        }
        checkTrigger.isCheck = false;
        checkTrigger.owner = rpc.transform;
        checkTrigger.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Wait for transform back
        if (rpc.currentId != 0 && rpc != null)
        {
            if (currentTimeTransform > 0 && rpc.player.isTranform)
            {               
                currentTimeTransform -= Time.deltaTime;
                playerBody.SetActive(false);
                if(currentTimeTransform <= 0)
                {
                    rpc.TransformBodyBack(rpc.currentId);
                }
            }
        }
        ActionAfterRPC();
    }
    //Start trigger
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "prop" )
    //    {
    //        rpc.id = other.GetComponent<Prop>().id;
    //    }
    //    if(other.tag == "prop")
    //    {
    //        if (isGoThrought == true)
    //        {
    //            Physics.IgnoreCollision(rpc.player.collider, other.GetComponent<BoxCollider>(), true);
    //        }
    //        else
    //        {
    //            Physics.IgnoreCollision(rpc.player.collider, other.GetComponent<BoxCollider>(), false);
    //        }
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "prop")
    //    {
    //        rpc.id = other.GetComponent<Prop>().id;
    //    }
    //    if (other.tag == "prop")
    //    {
    //        if (isGoThrought == true)
    //        {
    //            Physics.IgnoreCollision(rpc.player.collider, other.GetComponent<BoxCollider>(), true);
    //        }
    //        else
    //        {
    //            Physics.IgnoreCollision(rpc.player.collider, other.GetComponent<BoxCollider>(), false);
    //        }
    //    }
    //}
    //End trigger

    public void Use()
    {
        rpc.photonView.RPC("Invisible", RpcTarget.AllBuffered);
        rpc.photonView.RPC("Fake", RpcTarget.AllBuffered);
        rpc.FakePropPutIn(rpc.id); 
    }
    public void Use2()
    {
        StartCoroutine(GoThrow());
    }
    public void Use3()
    {
        rpc.TransformBodyTo(rpc.id);
    }
    IEnumerator GoThrow()
    {
        isGoThrought = true;
        rpc.player.AddDebuffText("BREAK WALL",0);
        yield return new WaitForSeconds(2f);
        rpc.player.RemoveDebuffText();
        isGoThrought = false;
        checkTrigger.gameObject.SetActive(true);
        checkTrigger.isCheck = true;
        yield return new WaitForSeconds(0.1f);
        checkTrigger.isCheck = false;
        checkTrigger.gameObject.SetActive(false);
    }
    void ActionAfterRPC()
    {
        if(rpc.isButFakeProp == true)
        {
            ButFakeProp();
        }
        if (rpc.isTransformTo == true)
        {
            TranformTo();
        }
        if (rpc.isTransformBack == true)
        {
            TransformBack();
        }
        
    }
    void ButFakeProp()
    {
        propFakeList[8].SetActive(true);
        propFakeList[8].transform.position = transform.position;
        propFakeList[8].transform.rotation = transform.rotation;
        propFakeList[8].GetComponent<CloneTransformPlayer>().currentTime = 0;
        propFakeList[8].transform.parent = null;
        rpc.isButFakeProp = false;
    }
    void TranformTo()
    {
        currentTimeTransform = data.timeTransform;
        playerBody.SetActive(false);
        propList[rpc.id - 1].SetActive(true);
        rpc.currentId = rpc.id;
        if (GameManager.instance.mainPlayer.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek)
        {
            foreach(GameObject p in hideObject)
            {
                p.SetActive(false);
            }
        }
        rpc.isTransformTo = false;
    }
    void TransformBack()
    {
        playerBody.SetActive(true);
        propList[rpc.currentId - 1].SetActive(false);
        foreach (GameObject item in rpc.player.ObjectInvisible)
        {
            if (item.name == "RunEffect" || item.name == "WalkEffect")
            {
                item.SetActive(false);
            }
        }
        if (GameManager.instance.mainPlayer.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek && !rpc.player.inGrass)
        {
            foreach (GameObject p in hideObject)
            {
                p.SetActive(true);
            }
        }
        rpc.currentId = 0;
        rpc.isTransformBack = false;
    }
}
