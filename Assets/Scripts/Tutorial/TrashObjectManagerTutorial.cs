using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashObjectManagerTutorial : MonoBehaviour
{
    public static TrashObjectManagerTutorial manager;
    public List<TrashObjectTutorial> trashObjectList;
    public float time;
    private void Awake()
    {
        manager = this;
    }
    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine(WaitToPlay());
    }
    IEnumerator WaitToPlay()
    {
        yield return new WaitUntil(isPlaying);
        int i = 0;
        foreach (TrashObjectTutorial can in trashObjectList)
        {
            can.id = i;
            i++;
        }
    }
    bool isPlaying()
    {
        return GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing;
    }
    public void HitTrashCan(TrashObjectTutorial can, int way)
    {
        HitTrashCanCmd(can, way);
    }

    public void HitTrashCanCmd(TrashObjectTutorial can, int way)
    {
        can.isFall = true;
        can.trashBody.SetActive(false);
        can.trashFall[way].SetActive(true);
        can.trashWater[way].SetActive(true);
        StartCoroutine(DisAbleTrashCan(can.trashFall[way]));
    }
    IEnumerator DisAbleTrashCan(GameObject can)
    {
        yield return new WaitForSeconds(time);
        can.SetActive(false);
    }
    public void Slide(TrashObjectTutorial can, int idWater, PlayerSetupTutorial p, int timeUse)
    {
        slideCmd(can, idWater, p, timeUse);
    }

    void slideCmd(TrashObjectTutorial can, int idWater, PlayerSetupTutorial p, int timeUse)
    {
        if (timeUse == 0)
        {
            can.trashWater[idWater].SetActive(false);
        }
        else
        {
            can.trashWater[idWater].GetComponent<TrashWaterTutorial>().timeUse = timeUse;
        }
        p.slide.StartCoroutine(p.slide.SlideTime());
    }
}
