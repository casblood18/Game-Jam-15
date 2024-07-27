using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public static Action<float> OnCheckStage;

    public BossPaternPaths BossPaternPaths => bossPaternPaths;
    public Transform BossTransform => transform;
    public Transform PlayerTransform => playerTransform;
    public GameObject ConeObject => coneObject;
    public float MovementSpeed => movementSpeed;
    public Transform MiddlePoint => middlePoint.transform;

    public RedProjectile RedProjectile => redProjectile;
    public BlueProjectile BlueProjectile => blueProjectile;
    public YellowProjectile YellowProjectile => yellowProjectile;
    public PurpleProjectile PurpleProjectile => purpleProjectile;
    public OrangeProjectile OrangeProjectile => orangeProjectile;
    public GreenProjectile GreenProjectile => greenProjectile;

    [SerializeField] private StagesListSO stagesList;

    [Header("Projectile types")]
    [SerializeField] private RedProjectile redProjectile;
    [SerializeField] private BlueProjectile blueProjectile;
    [SerializeField] private YellowProjectile yellowProjectile;
    [SerializeField] private PurpleProjectile purpleProjectile;
    [SerializeField] private OrangeProjectile orangeProjectile;
    [SerializeField] private GreenProjectile greenProjectile;

    [Space(5)]
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject coneObject;
    [SerializeField] private GameObject middlePoint;

    [SerializeField] private BossPaternPaths bossPaternPaths;

    private StageBaseSO currentStage;
    private ushort currentStageIndex;

    private Transform playerTransform;

    private void OnEnable()
    {
        OnCheckStage += CheckStage;
    }

    private void OnDisable()
    {
        OnCheckStage -= CheckStage;
    }

    private void Start()
    {
        currentStageIndex = 0;
        currentStage = stagesList.Stages[currentStageIndex];

        playerTransform = GameObject.FindWithTag("Player").transform;

        StartCoroutine(currentStage.Attack(this));
    }

    private void Update()
    {
        currentStage.Movement(this);
    }

    private void CheckStage(float health)
    {
        StageBaseSO nextStage = stagesList.Stages[currentStageIndex + 1];

        if (health > nextStage.MinimumHealth) return;

        SwitchStage();
    }

    private void SwitchStage()
    {
        currentStageIndex++;
        currentStage = stagesList.Stages[currentStageIndex];

        StartCoroutine(currentStage.Attack(this));
    }
}
