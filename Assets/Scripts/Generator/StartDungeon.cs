﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class StartDungeon : MonoBehaviour
{

    public DungeonGenerator firstDungeon;

    public BossDungeon bossRoom;

    [SerializeField]
    GameObject playerObject;

    [SerializeField]
    NavMeshSurface navi;


    /// <summary>
    /// Note this is called before DungeonMaster Start()
    /// </summary>
    void Start()
    {
        DungeonMaster.Instance.dungeonStarter = this;
        DungeonMaster.Instance.navi = navi;
        
        if (DungeonMaster.Instance.currentDungeonCount != 0)
            DungeonMaster.Instance.ResetDungeonMaster();

        DungeonOn();
    }


    public void DungeonOn()
    {
        GameAudio.Instance.SetNormalMusic();
        SetupDungeon();
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        DungeonMaster.Instance.PlayerMoving = true;
        Invoke(nameof(ResetMoving), 2.0f);
        playerObject.transform.DOJump(Vector3.up * 1.5f, 1, 1, 2f);
    }

    private void SetupDungeon()
    {
        if(!firstDungeon.gameObject.activeInHierarchy)
        {
            GameObject newStartDungeon = Instantiate(firstDungeon.gameObject);
            newStartDungeon.transform.position = Vector3.zero * 1.5f;
            firstDungeon = newStartDungeon.GetComponent<DungeonGenerator>();

        }


        firstDungeon.gameObject.SetActive(true);
        firstDungeon.StartDungeon();

        DungeonMaster.Instance.currentLevelDungeons.Add(firstDungeon);
        DungeonMaster.Instance.SetNewDungeons(firstDungeon, Direction.nothing);
        DungeonMaster.Instance.descriptionText.fadeOutInfo.FadeOutText(string.Concat("Level ", DungeonMaster.Instance.levelCount));
    }

    void ResetMoving()
    {
        DungeonMaster.Instance.PlayerMoving = false;
    }
}
