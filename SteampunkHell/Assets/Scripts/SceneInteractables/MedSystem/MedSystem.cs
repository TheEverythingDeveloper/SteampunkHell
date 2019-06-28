using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MedSystem : MonoBehaviour, InteractableMachine
{
    private Model _actualUser;
    private AudioSource _audioSrc;
    private PlayerLifeController _lifeController;

    private ParticleSystem _particles;

    private MedSystemView _view;
    private MedSystemAudioController _audioControl;

    public bool canUse = true;

    public event Action<bool> OnTrigger = delegate { }; //true = enter, false = exit
    public event Action<bool> OnUsing = delegate { };

    private void Awake()
    {
        _view = GetComponent<MedSystemView>();
        _audioControl = GetComponent<MedSystemAudioController>();
        _particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        OnTrigger += _view.CanHeal;
        OnTrigger += _audioControl.CanHealAudio;

        OnUsing += _view.Using;
        OnUsing += _audioControl.Using;

        EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, StartWave);
        EventsManager.SubscribeToEvent(TypeOfEvent.FinishWave, EndWave);
    }

    public void StartWave(params object[] parameters)
    {
        canUse = false;
        OnTrigger(false);
    }

    public void EndWave(params object[] parameters)
    {
        canUse = true;
    }

    public void PlayerEnter(Model model)
    {
        if (!canUse) return;
        OnUsing(true);
        _actualUser = model;
        _lifeController = model.lifeControl;
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        _particles.Play();
        yield return new WaitForSeconds(2f);
        //TODO: Activar un sonido general aca.
        while (_actualUser != null && _lifeController.life < 100)
        {
            Debug.Log(_lifeController.life);
            _lifeController.ReceiveDamage(-10, Vector3.zero);
            //TODO: Hacer sonido aca como feedback de que te esta curando. Un Beep asi nomas.
            yield return new WaitForSeconds(2f);
        }
        _particles.Stop();
        StartCoroutine(ExitCoroutine());
    }

    public void PlayerOnTrigger(Model model, bool on)
    {
        OnTrigger(on);
    }

    public IEnumerator ExitCoroutine()
    {
        yield return new WaitForSeconds(1f);
        _actualUser.EndShopping();
        _actualUser = null;
        yield return new WaitForSeconds(1);
        GameManager.Instance.canPause = true;
        OnUsing(false);
    }

    private void Update()
    {
        if (_actualUser == null) return; //No hacer nada en caso de que no tenga usuario comprando

        _view.ArtificialUpdate();
    }
}
