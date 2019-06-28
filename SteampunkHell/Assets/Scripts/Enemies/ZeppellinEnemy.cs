using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppellinEnemy : Enemy
{
    public Renderer _mainShapeRenderer;
    public ParticleSystem _smokeParticles;
    public Color brokenColor1, brokenColor2;
    private AudioSource _audiosrc;

    protected override void Awake()
    {
        base.Awake();
        _audiosrc = GetComponent<AudioSource>();

    }
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Vector3 playerPosInYPlane = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.forward = Vector3.Slerp(transform.forward,
            (playerPosInYPlane - transform.position).normalized,
            VariablesPointer.EnemyZeppellinState.rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * VariablesPointer.EnemyZeppellinState.movementSpeed *Time.deltaTime;
    }

    protected override void Reset()
    {
        base.Reset();
        _audiosrc.volume = 0.1f;
        _mainShapeRenderer.material.SetColor("_BaseColor", Color.white);
        _mainShapeRenderer.material.SetFloat("_BrokenAmount", 1);
        _smokeParticles.Stop();
    }

    protected override void DeathFeedback()
    {
        //TODO: Sonido de explosion
        EnemySpawner.Instance.ReturnEnemy(this);
    }

    public void ZeppellinChangeMode(int changeModeID)
    {
        if(changeModeID == 0)
        {
            _audiosrc.volume = 0.4f;
            _mainShapeRenderer.material.SetColor("_BaseColor", brokenColor1);
            _mainShapeRenderer.material.SetFloat("_BrokenAmount", 0.5f);
            _smokeParticles.Play();
            ParticleSystem.MainModule psMain = _smokeParticles.main;
            psMain.startSize = 4f;
        }
        else
        {
            _audiosrc.volume =1f;
            _mainShapeRenderer.material.SetColor("_BaseColor", brokenColor2);
            _mainShapeRenderer.material.SetFloat("_BrokenAmount", 0.2f);
            ParticleSystem.MainModule psMain = _smokeParticles.main;
            psMain.startSize = 10f;
            //Tirar particulas
        }
    }

    public override bool ReceiveDamage(float amount, Vector3 pushForce)
    {
        life -= amount;
        life = Mathf.Clamp(life, 0, _totalLife);
        ReceiveDamageFeedback(pushForce);
        if (life <= 0)
        {
            Death();
            return true;
        }
        else if (life <= _totalLife / 3)
        {
            ZeppellinChangeMode(1);
        }
        else if(life<= _totalLife / 3 * 2)
        {
            ZeppellinChangeMode(0);
        }
        return false;
    }

    protected override void Shoot() //En el zeppellin va a ser = spawn enemy explosive
    {
        EnemySpawner.Instance.GetEnemyExplosive(transform);
    }

    public static void TurnOn(ZeppellinEnemy b)
    {
        b.gameObject.SetActive(true);
        b.Reset();
    }

    public static void TurnOff(ZeppellinEnemy b)
    {
        b.gameObject.SetActive(false);
    }

}
