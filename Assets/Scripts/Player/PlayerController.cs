using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour,IDataPersistence
{
    public AnimationController animator { get; private set; }

    public CharacterController characterController { get; private set; }

    public InputController inputController { get; private set; }

    public PlayerPowerTracking powerTracking { get; private set; }
    public PathManager pathManager { get; private set; }

    public AnimationController animationController { get; private set; }

    [SerializeField] CharacterData characterData;
    [SerializeField] Transform visualContainer;
    [SerializeField] Vector3 LastCheckPointPos;

    [Header("VFX")]
    [SerializeField] GameObject ShieldVFX;
    [SerializeField] GameObject TrapVFX;
    [SerializeField] GameObject debuffVFX;

    private GameObject powerVFX;
    public CharacterData CharacterData=>characterData;

    public bool isInvincible;
    public bool isUsingShieldSkill = false;
    public bool isDead { get; private set; }
    public bool isGameOver;
    private void Awake()
    {
        animator = GetComponent<AnimationController>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<InputController>();
        powerTracking = GetComponent<PlayerPowerTracking>();
        pathManager = GetComponent<PathManager>();
        animationController = GetComponent<AnimationController>();
    }
    private void Start()
    {
        PickupTracking.Instance.OnGemPickup += Instance_OnGemPickup;
        PickupTracking.Instance.OnPowerUp += Instance_OnPowerUp;
        PickupTracking.Instance.OntriggerDead += Instance_OntriggerDead;

        pathManager.isDoubleJumpAllowed = false;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            powerTracking.ActivatePower();
        }
    }
    private void Instance_OnPowerUp(PowerUpType type,bool isActive)
    {
        switch (type)
        {
            case PowerUpType.DoubleJump:
                pathManager.isDoubleJumpAllowed = isActive;
                break;
            case PowerUpType.FrozenTime:
                GameManager.Instance.FreezeCountdown = isActive;
                break;
            case PowerUpType.SpeedUp:

                break;
        }    
    }
    private void Instance_OnGemPickup(int amount)
    {
        powerTracking.TrackingPower(amount);
    }
    public void ApplyPowerEffect()
    {
        switch (characterData.power.type)
        {
            case PowerData.PowerType.SpeedUp:
                pathManager.moveSpeed = 15f;
                break;
            case PowerData.PowerType.Immortal:
                isInvincible = true;
                isUsingShieldSkill = true;
                break;
            case PowerData.PowerType.BonusPoint:
                PickupTracking.Instance.UpdateGemScoreValue(PickupTracking.Instance.CurrentGem + 100);
                var fx = Instantiate(characterData.power.vfx, visualContainer.transform);
                Destroy(fx,1);
                break;
        }
        if(powerVFX == null && characterData.power.vfx!=null && characterData.power.type != PowerData.PowerType.BonusPoint)
        {
            powerVFX = Instantiate(characterData.power.vfx,visualContainer.transform);
        }
        if(powerVFX != null)
        {
            powerVFX.SetActive(true);
        }
    }
    public void RemovePowerEffect()
    {
        switch (characterData.power.type)
        {
            case PowerData.PowerType.SpeedUp:
                pathManager.moveSpeed = 10f;
                break;
            case PowerData.PowerType.Immortal:
                Debug.Log("disable immortal");
                isUsingShieldSkill = false;
                isInvincible = false;
                break;
            case PowerData.PowerType.BonusPoint:
                break;
        }
        if(powerVFX!=null && characterData.power.type != PowerData.PowerType.BonusPoint)
        {
            powerVFX.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPickupable pickupable))
        {
            AudioManager.Instance.PlaySFX("Pickup");
            PickupItem pickupItem = other.gameObject.GetComponent<PickupItem>();
            switch(pickupItem.Type)
            {
                case PickupItem.PickupType.Key:
                    GameManager.Instance.ToNextLevel();
                    break;
                case PickupItem.PickupType.PowerUp:
                    PowerUpItem powerUpItem = pickupItem as PowerUpItem;
                    PickupTracking.Instance.AddPowerUp(powerUpItem);
                    break;
                case PickupItem.PickupType.Gem:
                    GemItem gemItem = pickupItem as GemItem;
                    Debug.Log($"pickup an Gem {gemItem.GemType}");
                    PickupTracking.Instance.AddGem(gemItem);
                    break;
            }
            if(pickupItem.pickupVFX)
            {
                Instantiate(pickupItem.pickupVFX,other.ClosestPoint(transform.position),Quaternion.identity);
            }
            pickupable.Pickup();
        }
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            var checkPoint = other.gameObject.GetComponent<CheckPoint>();
            if(!checkPoint || !checkPoint.isChecked)
            {
                AudioManager.Instance.PlaySFX("CheckPoint");
                checkPoint.Trigger();
            }
        }
        if(other.gameObject.CompareTag("DeadZone"))
        {
            Instance_OntriggerDead(false);
        }
    }
    public void TrapTriggerAction(Vector3 hitPosition)
    {
        if (!isInvincible && !isUsingShieldSkill)
        {
            PickupTracking.Instance.DescreaseGem();
            ApplyKnockBack(hitPosition);
            if (TrapVFX != null)
            {
                Instantiate(TrapVFX, hitPosition, Quaternion.identity);
            }
        }
    }
    /*  private void OnControllerColliderHit(ControllerColliderHit hit)
      {
          if (hit.gameObject.CompareTag("Traps"))
          {
              if (!isInvincible)
              {
                  PickupTracking.Instance.DescreaseGem();
                  ApplyKnockBack(hit.transform.position);
                  if (TrapVFX != null)
                  {
                      Instantiate(TrapVFX, hit.point, Quaternion.identity);
                  }
              }
          }
      }*/
      void ApplyKnockBack(Vector3 trapPos)
      {
          pathManager.ApplyKnockback(trapPos, 1);
          StartCoroutine(ApplyInvincible());
      }

    private void Instance_OntriggerDead(bool isGameOver)
    {
        isDead = true;
        this.isGameOver = isGameOver;
        animationController.SettriggerDead();
    }
    public void DeadAnimationEvent()
    {
        if (isGameOver)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            pathManager.gravityChange = .3f;
            GameManager.Instance.ReloadGame();
        }
    }
    public IEnumerator ApplyInvincible()
    {
        ShieldVFX.SetActive(true);
        isInvincible = true;
        debuffVFX.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        ShieldVFX.SetActive(false);
        isInvincible = false;
        debuffVFX.gameObject.SetActive(false);

    }
    public void Save(GameData data)
    {
        data.isSaved = true;
        data.LastPositionPos = transform.position;
        data.WaypointID = pathManager.currentWaypoint.waypointID;
    }

    public void Load(GameData data)
    {
        characterData = CharacterDataBase.Instance.GetPickupCharacter(data.CharacterType);
        LastCheckPointPos = data.LastPositionPos;
        Debug.Log("load");
        pathManager.visualBody = Instantiate(characterData.Visual, visualContainer);
        animator.Animator = pathManager.visualBody.GetComponent<Animator>();
        if (data.isSaved && SceneManager.GetActiveScene().name == data.SceneName)
        {
            characterController.enabled = false;
            gameObject.transform.position = LastCheckPointPos;
            characterController.enabled = true;
            pathManager.currentWaypoint = FindWaypointByID(data.WaypointID);
            Debug.Log(pathManager.currentWaypoint != null);
        }
    }
    private Waypoint FindWaypointByID(string waypointID)
    {
        if (string.IsNullOrEmpty(waypointID)) return null;

        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        return allWaypoints.FirstOrDefault(w => w.waypointID == waypointID);
    }
}
