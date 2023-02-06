//using UnityEngine;
//using UnityEngine.UI;

//public class UISkill : MonoBehaviour
//{
//    [SerializeField] UnityEngine.UI.Button smallBtn, bigBtn;
//    [SerializeField] Image smallImg, bigImg;
//    Image cooldownImgSmall, cooldownImgBig;
//    [SerializeField] Skill smallSkill, bigSkill;
//    // Start is called before the first frame update
//    void Start()
//    {
//        //Get cooldown img
//        cooldownImgSmall = smallBtn.GetComponent<Image>();
//        cooldownImgBig = bigBtn.GetComponent<Image>();
//        //Add onclick for button
//        smallBtn.onClick.AddListener(smallSkill.SkillClick);
//        bigBtn.onClick.AddListener(bigSkill.SkillClick);
//        //Add sprite for img
//        smallImg.sprite = smallSkill.uiSprite;
//        bigImg.sprite = bigSkill.uiSprite;

//    }

//    // Update is called once per frame
//    void Update()
//    {       
//        if (smallSkill.isActived)
//        {
//            //fill amout to 0
//            if (smallSkill.currentcooldownTime == 0)
//            {
//                cooldownImgSmall.fillAmount = 0;
//            }
//            //Cooldow fill up
//            else
//            {
//                cooldownImgSmall.fillAmount += 1.0f / smallSkill.cooldownTime * Time.deltaTime;
//            }
//        }
//        if (bigSkill.isActived)
//        {
//            //fill amout to 0
//            if (bigSkill.currentcooldownTime == 0)
//            {
//                cooldownImgBig.fillAmount = 0;
//            }
//            //Cooldow fill up
//            else
//            {
//                cooldownImgBig.fillAmount += 1.0f / bigSkill.cooldownTime * Time.deltaTime;
//            }
//        }
//    }
//}
