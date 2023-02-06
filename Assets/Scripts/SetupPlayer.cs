using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SetupPlayer : MonoBehaviourPunCallbacks
{
    private bool temp = false;
    public GameObject[] test;
    public GameManager.HideOrSeek role;
    public GameManager.Character character;
    public GameManager.Character characterTemp;
    public int roleNumber;
    GameManager gameManager;
    SkillManager skillManager;
    ModelManager modelManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        skillManager = GetComponent<SkillManager>();
        modelManager = GetComponent<ModelManager>();

    }
    private bool sameRole =true;
    public int numberID =0;
    private void Start()
    {
        StartCoroutine(CheckJointMatch());
    }
    void AddCharacterToList(GameManager.HideOrSeek _role, GameManager.Character _character, int _number)
    {
        if (_role == GameManager.HideOrSeek.Hide)
        {
            for (int i = 0; i < _number; i++)
            {
                gameManager.hideBotList.Add(_character);
            }
        }
        else
        {
            for (int i = 0; i < _number; i++)
            {
                gameManager.seekBotList.Add(_character);
            }
        }
    }
    public float winPercent;
    int numberRanSeek;
    int numberRanHide;
    IEnumerator CheckJointMatch()
    {
        yield return new WaitUntil(CheckPlayerJointMatch);
        if (PhotonNetwork.IsMasterClient)
        {
            if (RoomManager2.Instance.needBot)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - RoomManager2.Instance.players; i++)
                {
                    PlayerSetup bot = PhotonNetwork.Instantiate(Path.Combine("Bot", "Bot"), Vector3.zero, Quaternion.identity).GetComponent<PlayerSetup>();
                    photonView.RPC("SetBot", RpcTarget.AllBuffered, bot.photonView.ViewID);
                }
            }
            foreach (PlayerSetup p in gameManager.players)
            {
                winPercent += p.winPercent;
                if (p.isBot)
                {
                    gameManager.botList.Add(p);
                }
                else
                {
                    gameManager.playerListRoom.Add(p);
                }
            }
            winPercent = winPercent / gameManager.playerListRoom.Count;
            SetRolePlayer();
            //PlayerSetup player = gameManager.playerListRoom[0];
            //if (player.player == GameManager.HideOrSeek.Hide)
            //{
            //    if (winPercent > 50)
            //    {
            //        numberRanSeek = 0;
            //        numberRanHide = 1;

            //    }
            //    //else if (winPercent < 25)
            //    //{
            //    //    numberRanSeek = Random.Range(2, 4);
            //    //    numberRanHide = Random.Range(2, 4);
            //    //}
            //    //else
            //    //{
            //    //    numberRanSeek = Random.Range(1, 3);
            //    //    numberRanHide = Random.Range(1, 3);
            //    //}
            //}
            //else
            //{
            //    if (winPercent > 50)
            //    {
            //        numberRanSeek = Random.Range(1, 2);
            //        numberRanHide = Random.Range(1, 2);
            //    }
            //    else if (winPercent < 25)
            //    {
            //        numberRanSeek = 0;
            //        numberRanHide = 1;
            //    }
            //    else
            //    {
            //        numberRanSeek = 1;
            //        numberRanHide = 2;
            //    }
            //}
            //AddCharacterToList(GameManager.HideOrSeek.Seek, GameManager.Character.Jokers, numberRanSeek);
            //AddCharacterToList(GameManager.HideOrSeek.Hide, GameManager.Character.Wizard, numberRanHide);
            //gameManager.seekCharacterBot.Remove(GameManager.Character.Jokers);
            //gameManager.hideCharacterBot.Remove(GameManager.Character.Wizard);
            for (int i = 0; i < gameManager.numberHide - numberRanHide; i++)
            {
                GameManager.Character charac = (GameManager.Character)gameManager.hideCharacterBot[Random.Range(0, gameManager.hideCharacterBot.Count)];
                gameManager.hideBotList.Add(gameManager.hideCharacterBot[Random.Range(0, gameManager.hideCharacterBot.Count)]);
            }
            for (int i = 0; i < gameManager.numberSeek - numberRanSeek; i++)
            {
                GameManager.Character charac = (GameManager.Character)gameManager.seekCharacterBot[Random.Range(0, gameManager.seekCharacterBot.Count)];
                gameManager.seekBotList.Add(gameManager.seekCharacterBot[Random.Range(0, gameManager.seekCharacterBot.Count)]);
            }
            //SetRole();
            SetRole2();
            StartCoroutine(Wait2s());
        }
        if (RoomManager2.Instance.needBot)
        {
            StartCoroutine(IESetRoleDone());
        }
        else
        {
            StartCoroutine(IESetRoleDone2());
        }
    }
    IEnumerator Wait2s()
    {
        yield return new WaitForSeconds(2f);
        SetCharacter();
    }
    IEnumerator IESetRoleDone()
    {
        yield return new WaitUntil(SetRoleDone);
        gameManager.state = GameManager.GameState.SettingPlayer;
    }
    IEnumerator IESetRoleDone2()
    {
        yield return new WaitUntil(SetRoleDone2);
        gameManager.state = GameManager.GameState.SettingPlayer;
    }
    [PunRPC]
    void SetBot(int id)
    {
        PlayerSetup bot = PhotonView.Find(id).GetComponent<PlayerSetup>();
        gameManager.players.Add(bot.GetComponent<PlayerSetup>());
    }
    private bool playersJointMatch;
    bool CheckPlayerJointMatch()
    {
        int temp1 = 0;
        foreach (PlayerSetup p in gameManager.players)
        {
            if (p.jointMatch)
            {
                temp1++;
            }
        }
        if (temp1 == gameManager.numberPlayer)
        {
            playersJointMatch = true;
        }
        else
        {
            playersJointMatch = false;
        }
        return playersJointMatch == true;
    }
    bool SetRoleDone()
    {
        return numberID == PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    bool SetRoleDone2()
    {
        return numberID == PhotonNetwork.CurrentRoom.PlayerCount;
    }
    public int idPlayer = 0;
    void SetRolePlayer()
    {
        for (int i = 0; i < gameManager.playerListRoom.Count; i++)
        {
            roleNumber = Random.Range(0, gameManager.hideorseek.Count);
            role = (GameManager.HideOrSeek)gameManager.hideorseek[roleNumber];
            int id = gameManager.playerListRoom[i].GetComponent<PhotonView>().ViewID;
            photonView.RPC("AddPlayerJobCmd", RpcTarget.AllBufferedViaServer, id, (byte)role, (byte)role, (byte)idPlayer);
            //photonView.RPC("AddPlayerJobCmd", RpcTarget.AllBufferedViaServer, id, (byte)GameManager.HideOrSeek.Seek, (byte)GameManager.HideOrSeek.Seek, (byte)idPlayer);
            gameManager.hideorseek.RemoveAt(roleNumber);
            //gameManager.hideorseek.Remove(GameManager.HideOrSeek.Seek);
            idPlayer++;
        }
    }
    void SetCharacter()
    {
        for (int i = 0; i < gameManager.players.Count; i++)
        {
            int id = gameManager.players[i].GetComponent<PhotonView>().ViewID;
            int characterNumber;
            int characterNumberTemp;
            if (gameManager.players[i].player == GameManager.HideOrSeek.Seek)
            {
                if (gameManager.players[i].isBot)
                {
                    characterNumberTemp = Random.Range(0, gameManager.hideCharacter.Count);
                    characterNumber = Random.Range(0, gameManager.seekBotList.Count);
                    character = (GameManager.Character)gameManager.seekBotList[characterNumber];
                    characterTemp = (GameManager.Character)gameManager.hideCharacter[characterNumberTemp];
                    gameManager.seekBotList.Remove(character);
                }
                else
                {
                    characterNumber = gameManager.players[i].seekId;
                    characterNumberTemp = gameManager.players[i].hideId;
                    character = (GameManager.Character)gameManager.seekCharacter[characterNumber];
                    characterTemp = (GameManager.Character)gameManager.hideCharacter[characterNumberTemp];
                }
            }
            else
            {
                if (gameManager.players[i].isBot)
                {
                    characterNumber = Random.Range(0, gameManager.hideBotList.Count);
                    character = (GameManager.Character)gameManager.hideBotList[characterNumber];
                    gameManager.hideBotList.Remove(character);
                }
                else
                {
                    characterNumber = gameManager.players[i].hideId;
                    character = (GameManager.Character)gameManager.hideCharacter[characterNumber];
                }
            }
            photonView.RPC("AddPlayerCharacterCmd", RpcTarget.AllBufferedViaServer, id, (byte)character, (byte)characterTemp);
        }
    }
    void SetRole2()
    {
        for (int i = 0; i < gameManager.botList.Count; i++)
        {
            roleNumber = Random.Range(0, gameManager.hideorseek.Count);
            role = (GameManager.HideOrSeek)gameManager.hideorseek[roleNumber];
            int id = gameManager.botList[i].GetComponent<PhotonView>().ViewID;
            //photonView.RPC("AddPlayerJobCmd", RpcTarget.AllBufferedViaServer, id, (byte)role, (byte)role, (byte)idPlayer);
            photonView.RPC("AddPlayerJobCmd", RpcTarget.AllBufferedViaServer, id, (byte)role, (byte)role, (byte)idPlayer);
            gameManager.hideorseek.RemoveAt(roleNumber);
            idPlayer++;
        }
    }
    public bool setRoleDone;
    [PunRPC]
    void AddPlayerJobCmd(int id, byte job, byte jobTemp, byte _idPlayer)
    {
        PlayerSetup playerSetup = PhotonView.Find(id).GetComponent<PlayerSetup>();
        playerSetup.player = (GameManager.HideOrSeek)job;
        playerSetup.playerTemp = (GameManager.HideOrSeek)jobTemp;
        playerSetup.id = _idPlayer;
    }
    [PunRPC]
    void AddPlayerCharacterCmd(int id, byte _character, byte _characterTemp)
    {
        PlayerSetup playerSetup = PhotonView.Find(id).GetComponent<PlayerSetup>();
        playerSetup.character = (GameManager.Character)_character;
        playerSetup.characterTemp = (GameManager.Character)_characterTemp;
        numberID++;
    }
    public void SetObjectPlayerLocal(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Jokers:
                {
                    for (int i = 0; i < gameManager.players.Count; i++)
                    {
                        if (gameManager.players[i].player == GameManager.HideOrSeek.Seek)
                        {
                            continue;
                        }
                        GameObject pingObj = Instantiate(gameManager.pingToxicObject, Vector3.zero, GameManager.instance.pingObject.transform.rotation);
                        pingObj.GetComponent<PingToxic>().owner = gameManager.players[i];
                        gameManager.pingToxic.Add(pingObj.GetComponent<PingToxic>());
                        pingObj.SetActive(false);
                    }
                    //GameObject jokerBomb = Instantiate(SummonStat.instance.jokerBomb, Vector3.zero, SummonStat.instance.jokerBomb.transform.rotation);
                    //jokerBomb.GetComponent<JokerBomb>().owner = _player;
                    //gameManager.jokerBombList.Add(jokerBomb.GetComponent<JokerBomb>());
                    //jokerBomb.SetActive(false);
                    //GameObject jokerToxic = Instantiate(GameManager.instance.jokerToxicObject, Vector3.zero, GameManager.instance.jokerToxicObject.transform.rotation);
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    //jokerToxic.GetComponent<JokerToxic>().owner = _player;
                    //gameManager.jokerToxic.Add(jokerToxic.GetComponent<JokerToxic>());
                    //jokerToxic.SetActive(false);
                    break;
                }
            case GameManager.Character.DarkRaven:
                {
                    for (int i = 0; i < gameManager.players.Count; i++)
                    {
                        if (gameManager.players[i].player==GameManager.HideOrSeek.Seek)
                        {
                            continue;
                        }
                        GameObject slowEffectObj = Instantiate(SummonStat.instance.slowEffect, Vector3.zero, SummonStat.instance.slowEffect.transform.rotation);
                        slowEffectObj.GetComponent<FearEffect>().owner = gameManager.players[i];
                        gameManager.effectRaven.Add(slowEffectObj);
                        slowEffectObj.SetActive(false);
                    }
                    GameObject shotravenObj = Instantiate(SummonStat.instance.shotPrefab, Vector3.zero, SummonStat.instance.shotPrefab.transform.rotation);
                    shotravenObj.GetComponent<Pulse>().owner = _player;
                    gameManager.shotRaven.Add(shotravenObj.GetComponent<Pulse>());
                    shotravenObj.SetActive(false);
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    break;
                }
            case GameManager.Character.Wizard:
                {
                    GameObject wizardObj = Instantiate(SummonStat.instance.wizardObj, Vector3.zero, Quaternion.identity);
                    wizardObj.GetComponent<WizardObject>().owner = _player;
                    gameManager.wizardObj.Add(wizardObj.GetComponent<WizardObject>());
                    _player.ObjectInvisible.Add(wizardObj.GetComponent<WizardObject>().bulletEffect);
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    //_player.ObjectInvisible.Add(wizardObj.GetComponent<WizardObject>().waypoint);
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    GameObject invisibleObj = Instantiate(SummonStat.instance.invisibleObject, Vector3.zero, Quaternion.identity);
                    invisibleObj.GetComponent<Invisible>().owner = _player;
                    _player.invisibleObject = invisibleObj;
                    gameManager.invisibleObject.Add(invisibleObj.GetComponent<Invisible>());
                    invisibleObj.SetActive(false);
                    GameObject transformObject = Instantiate(SummonStat.instance.tranformObj, Vector3.zero, Quaternion.identity);
                    transformObject.transform.SetParent(_player.transform);
                    transformObject.transform.position = _player.transform.position;
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    break;
                }
            case GameManager.Character.SpeedMan:
                {
                    GameObject dash = Instantiate(SummonStat.instance.speedManDash, Vector3.zero, Quaternion.identity);
                    dash.transform.SetParent(_player.transform);
                    dash.transform.position = _player.transform.position;
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    break;
                }
            case GameManager.Character.Davros:
                {
                    GameObject scanObj = Instantiate(SummonStat.instance.davrosScanObject, Vector3.zero, SummonStat.instance.davrosScanObject.transform.rotation);
                    scanObj.GetComponent<ScanDavros>().owner = _player;
                    gameManager.scanDavros.Add(scanObj.GetComponent<ScanDavros>());
                    scanObj.SetActive(false);
                    GameObject slowEffectAfterRun = Instantiate(GameManager.instance.slowEffectAffterRunningPrefab, Vector3.zero, GameManager.instance.slowEffectAffterRunningPrefab.transform.rotation);
                    _player.slowEffectAffterRunningObj = slowEffectAfterRun;
                    slowEffectAfterRun.SetActive(false);
                    GameObject bandage = Instantiate(SummonStat.instance.banDageObject, Vector3.zero, SummonStat.instance.banDageObject.transform.rotation);
                    bandage.GetComponent<Bandage>().owner = _player;
                    bandage.GetComponent<Bandage>().id = _player.photonView.ViewID;
                    gameManager.bandageList.Add(bandage.GetComponent<Bandage>());
                    bandage.SetActive(false);
                    GameObject lineRender = Instantiate(SummonStat.instance.lineRenderObject, Vector3.zero, SummonStat.instance.lineRenderObject.transform.rotation);
                    lineRender.GetComponent<LineRender>().owner = _player;
                    gameManager.lineRenderer.Add(lineRender.GetComponent<LineRender>());
                    lineRender.SetActive(false);
                    break;
                }

        }
    }
    [PunRPC]
    void SetOwner(int id, int idOwner)
    {
        PlayerSetup owner = PhotonView.Find(idOwner).transform.GetComponent<PlayerSetup>();
        if (owner.character == GameManager.Character.Jokers)
        {
            PlayerJoker joker = PhotonView.Find(id).transform.GetComponent<PlayerJoker>();
            joker.owner = owner;
        }
        else if (owner.character == GameManager.Character.DarkRaven)
        {
            Raven raven = PhotonView.Find(id).transform.GetComponent<Raven>();
            raven.owner = owner;
            owner.summonDarkRaven = raven;
        }
    }
    public void SetObjectPlayerPhoton(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Jokers:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject jokerObj = PhotonNetwork.Instantiate(Path.Combine("Summon", SummonStat.instance.jokerSummon), Vector3.zero, Quaternion.identity);
                        photonView.RPC("SetOwner", RpcTarget.AllBuffered, jokerObj.GetComponent<PlayerJoker>().photonView.ViewID, _player.photonView.ViewID);
                    }
                    break;
                }
            case GameManager.Character.DarkRaven:
                {
                    GameObject ravenObj = PhotonNetwork.Instantiate(Path.Combine("Summon", SummonStat.instance.darkravenSummon), Vector3.zero, Quaternion.identity);
                    photonView.RPC("SetOwner", RpcTarget.AllBuffered, ravenObj.GetComponent<Raven>().photonView.ViewID, _player.photonView.ViewID);
                    break;
                }
            case GameManager.Character.Wizard:
                {
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    break;
                }
        }
    }
    public void GetCharacter(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Jokers:
                {
                    SetCharacter(_player, modelManager.jokerModel);
                    break;
                }
            case GameManager.Character.DarkRaven:
                {
                    SetCharacter(_player, modelManager.darkRavenModel);
                    break;
                }
            case GameManager.Character.Wizard:
                {
                    SetCharacter(_player, modelManager.wizardModel);
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    SetCharacter(_player, modelManager.tranformManModel);
                    break;
                }
            case GameManager.Character.SpeedMan:
                {
                    SetCharacter(_player, modelManager.speedManModel);
                    break;
                }
            case GameManager.Character.Davros:
                {
                    SetCharacter(_player, modelManager.davrosModel);
                    break;
                }
        }
    }
    public void SetSkill(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Jokers:
                {
                    InstantiateSkill(_player, skillManager.summonJokerSkill);
                    InstantiateSkill(_player, skillManager.swapJokerSkill);
                    //InstantiateSkill(_player, skillManager.darvosRunSkill);
                    break;
                }
            case GameManager.Character.DarkRaven:
                {
                    InstantiateSkill(_player, skillManager.darkRavenSkill);
                    InstantiateSkill(_player, skillManager.shotRavenSkill);
                    //InstantiateSkill(_player, skillManager.darvosRunSkill);
                    break;
                }
            case GameManager.Character.Wizard:
                {
                    InstantiateSkill(_player, skillManager.shootWizardSkill);
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    InstantiateSkill(_player, skillManager.tranformSkill);
                    break;
                }
            case GameManager.Character.SpeedMan:
                {
                    InstantiateSkill(_player, skillManager.speedManDash);
                   // InstantiateSkill(_player, skillManager.speedManBlackHoleSkill);
                    break;
                }
            case GameManager.Character.Davros:
                {
                    //InstantiateSkill(_player, skillManager.davrosScanSkill);
                    InstantiateSkill(_player, skillManager.banDageSkill);
                    //InstantiateSkill(_player, skillManager.darvosRunSkill);
                    break;
                }
        }
    }
    public void InstantiateSkill(PlayerSetup _player, GameObject _ui)
    {
        GameObject ui = Instantiate(_ui, Vector3.zero, _ui.transform.rotation);
        _player.skill.Add(ui);
        ui.GetComponent<UIOwner>().owner = _player;
        ui.transform.SetParent(GameManager.instance.canavasPlayingGame.transform);
        //ui.transform.position = _player.transform.position;
    }
    public void SetCharacter(PlayerSetup _player, GameObject _model)
    {
        GameObject model = Instantiate(_model, new Vector3(0f, 0.2f, 0f), _model.transform.rotation);;
        _player.model = model;
        model.transform.SetParent(_player.transform);
        model.transform.SetParent(_player.transform);
        model.transform.localPosition = new Vector3(0f, -0.2f, 0f);
    }
    public void SetModelSeeker(PlayerSetup _player, GameObject _model)
    {
        GameObject model = Instantiate(_model, new Vector3(0f, 0.2f, 0f), _model.transform.rotation);
        _player.seekModelHider = model;
        model.transform.SetParent(_player.transform);
        model.transform.localPosition = new Vector3(0f, -0.2f, 0f);
    }
    public void GetCharacterSeekerModelHider(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Wizard:
                {
                    SetModelSeeker(_player, modelManager.wizardModel);
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    SetModelSeeker(_player, modelManager.tranformManModel);
                    break;
                }
            case GameManager.Character.SpeedMan:
                {
                    SetModelSeeker(_player, modelManager.speedManModel);
                    break;
                }
        }
    }
    public void SetSkillSeekerModelHider(PlayerSetup _player, GameManager.Character _character)
    {
        switch (_character)
        {
            case GameManager.Character.Wizard:
                {
                    InstantiateSkillSeekerModelHider(_player, skillManager.shootWizardSkill);
                    break;
                }
            case GameManager.Character.TransformMan:
                {
                    InstantiateSkillSeekerModelHider(_player, skillManager.tranformSkill);
                    break;
                }
            case GameManager.Character.SpeedMan:
                {
                    InstantiateSkillSeekerModelHider(_player, skillManager.speedManDash);
                    //InstantiateSkillSeekerModelHider(_player, skillManager.speedManBlackHoleSkill);
                    break;
                }
        }
    }
    public void InstantiateSkillSeekerModelHider(PlayerSetup _player, GameObject _ui)
    {
        GameObject ui = Instantiate(_ui, Vector3.zero, _ui.transform.rotation);
        _player.skillTemp.Add(ui);
        ui.GetComponent<UIOwner>().owner = _player;
        ui.transform.SetParent(GameManager.instance.canavasPlayingGame.transform);
        //ui.transform.position = _player.transform.position;
    }
}
