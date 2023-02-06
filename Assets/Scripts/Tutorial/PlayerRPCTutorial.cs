using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRPCTutorial : MonoBehaviour
{
    public PlayerSetupTutorial player;
    public SkillData data;
    private void Start()
    {
        player = GetComponent<PlayerSetupTutorial>();
        data = GetComponent<SkillData>();

    }
    public void ActiveDash(bool t)
    {
        isDash = t;
        player.pAnimator.SetBool("isRunningSkill", t);
    }
    public void StunSeek(PlayerSetupTutorial seek)
    {
        seek.StartCoroutine(seek.StunTime(3f));
    }
    public void InvisibleTeam(Material[] material, PlayerSetupTutorial setup)
    {
        ChangeMaterial(material, setup);
        InvisibleObjectPlayer(setup, true);
    }
    public void InvisibleOtherTeam(Material[] material, PlayerSetupTutorial setup)
    {
        ChangeMaterial(material, setup);
        InvisibleObjectPlayer(setup, false);
    }
    public void GetMaterialSeek(PlayerSetupTutorial setup)
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
    void ChangeMaterial(Material[] newMat, PlayerSetupTutorial setup)
    {
        Renderer[] children;
        int index = 0;
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

    public void GetMaterial(PlayerSetupTutorial setup)
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
    public void InvisibleObjectPlayer(PlayerSetupTutorial setup, bool t)
    {
        foreach (GameObject item in setup.ObjectInvisible)
        {
            item.SetActive(t);
            if (GameManagerTutorial.instance.mainPlayer.player == GameManagerTutorial.HideOrSeek.Hide)
            {
                if (item.name == "TargetHide")
                {
                    item.SetActive(false);
                }
            }
            else
            {
                if (setup.player == GameManagerTutorial.HideOrSeek.Seek)
                {
                    if (item.name == "TargetHide")
                    {
                        item.SetActive(false);
                    }
                }
            }
            if ((item.name == "RunEffect" || item.name == "WalkEffect"))
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
            for (int i = 0; i < GameManagerTutorial.instance.grassMaterial.Count; i++)
            {
                yield return new WaitForSeconds(0.02f);
                _Grass.GetComponent<MeshRenderer>().sharedMaterial = GameManagerTutorial.instance.grassMaterial[i];
            }
        }
        else
        {
            for (int i = 0; i < GameManagerTutorial.instance.grassMaterial.Count; i++)
            {
                yield return new WaitForSeconds(0.02f);
                _Grass.GetComponent<MeshRenderer>().sharedMaterial = GameManagerTutorial.instance.grassMaterial[GameManagerTutorial.instance.grassMaterial.Count - i - 1];
            }
        }
    }
    public void GetInvisibleMaterial(PlayerSetupTutorial setup)
    {
        setup.invisibleMaterial = setup.model.GetComponent<PlayerStat>().invisibleMaterial;
    }
    public void GetInvisibleOtherTeamMaterial(PlayerSetupTutorial setup)
    {
        setup.invisibleOtherTeamMaterial = setup.model.GetComponent<PlayerStat>().invisibleOtherTeamMaterial;
    }
    public void SetDefaultMaterial(PlayerSetupTutorial setup)
    {
        Renderer[] children;
        int index = 0;
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
    public bool isDash;
}
