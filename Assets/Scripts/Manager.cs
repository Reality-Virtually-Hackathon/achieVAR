using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class Manager : MonoBehaviour
{
    public Animator OverallController;
    private GestureRecognizer _gestureRecognizer;
    private KeywordRecognizer _keywordRecognizer;
    public GameObject ty;
    public Animator ty_bundle;
    private Animator ty_animator;
    public Animator ty_animator_in_scene;
    public bool ty_active;

    public GameObject katie;
    public Animator katie_bundle;
    private Animator katie_animator;
    public Animator katie_animator_in_scene;
    public bool katie_active;

    public GameObject nima;
    public Animator nima_bundle;
    private Animator nima_animator;
    public Animator nima_animator_in_scene;
    public bool nima_active;

    private Boolean _characterExist = false;
    public Animator sceneAnimator;

    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    private Dictionary<string, KeywordAction> _keywordDictionary;

    private void Start()
    {
        _gestureRecognizer = new GestureRecognizer();
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        _gestureRecognizer.TappedEvent += Recognizer_TappedEvent;
        _gestureRecognizer.StartCapturingGestures();

        _keywordDictionary = new Dictionary<string, KeywordAction>();

        _keywordDictionary.Add("Stand idle", StandIdleCommand);
        _keywordDictionary.Add("Backflip", BackflipCommand);
        _keywordDictionary.Add("Block", BlockingCommand);
        _keywordDictionary.Add("Kick", KickCommand);
        _keywordDictionary.Add("Capoeira", CapoeiraCommand);
        _keywordDictionary.Add("Samba", SambaDanceCommand);
        _keywordDictionary.Add("Stretch", StretchCommand);
        _keywordDictionary.Add("Squat", SquatCommand);
        _keywordDictionary.Add("Thank you very much", BowCommand);
        _keywordDictionary.Add("Hello", BowCommand);

        _keywordDictionary.Add("Dance", DanceCommand);
        _keywordDictionary.Add("PhysicalTherapy", PhysicalTherapyCommand);
        _keywordDictionary.Add("Self Defense", SelfDefenseCommand);



        _keywordRecognizer = new KeywordRecognizer(_keywordDictionary.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void OnDestroy()
    {
        _gestureRecognizer.TappedEvent -= Recognizer_TappedEvent;
        _keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (_keywordDictionary.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        RaycastHit hitInfo;

        if (!_characterExist && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity))
        {
            ty_bundle.SetTrigger("Tapped");
            ty.transform.position = hitInfo.point;
            ty.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
            //GameObject character = Instantiate(_character, hitInfo.point, Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0));
            //_characterAnimator = _character.GetComponent<Animator>();
            //_characterExist = true;

            Camera.main.gameObject.GetComponent<UnityEngine.VR.WSA.SpatialMappingRenderer>().enabled = false;
        }
    }

    private void OpeningScene(PhraseRecognizedEventArgs args)
    {
        sceneAnimator.SetTrigger("Start");
        //_characterAnimator.Play("Idle", -1, 0f);
    }

    private void DanceCommand(PhraseRecognizedEventArgs args)
    {
        if (OverallController.GetCurrentAnimatorStateInfo(0).IsName("SayName"))
        {
            OverallController.SetTrigger("Dance");

            nima.SetActive(true);
            katie.SetActive(false);
            ty.SetActive(false);
        }
        //_characterAnimator.Play("Backflip", -1, 0f);
    }
    private void PhysicalTherapyCommand(PhraseRecognizedEventArgs args)
    {
        if (OverallController.GetCurrentAnimatorStateInfo(0).IsName("SayName"))
        {
            OverallController.SetTrigger("PhysicalTherapy");

            katie.SetActive(true);
            nima.SetActive(false);
            ty.SetActive(false);
        }
    }
    private void SelfDefenseCommand(PhraseRecognizedEventArgs args)
    {
        if (OverallController.GetCurrentAnimatorStateInfo(0).IsName("SayName"))
        {
            OverallController.SetTrigger("SelfDefense");

            ty.SetActive(true);
            nima.SetActive(false);
            katie.SetActive(false);
        }
    }

    private void StandIdleCommand(PhraseRecognizedEventArgs args)
    {
        //_characterAnimator.SetTrigger("Idle");
        ty_animator_in_scene.SetTrigger("Idle");
        //_characterAnimator.Play("Idle", -1, 0f);
    }

    //ty
    private void BackflipCommand(PhraseRecognizedEventArgs args)
    {
        //_characterAnimator.SetTrigger("Backflip");
        if (ty_active)
        {
            ty_animator_in_scene.SetTrigger("Backflip");

        }
        //_characterAnimator.Play("Backflip", -1, 0f);
    }

    private void BlockingCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            ty_animator_in_scene.SetTrigger("Block");
        }
        //_characterAnimator.SetTrigger("Block");
    }

    private void KickCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            ty_animator_in_scene.SetTrigger("Kick");
        }
        //_characterAnimator.SetTrigger("Kick");
    }

    //katie
    private void StretchCommand(PhraseRecognizedEventArgs args)
    {
        if (katie_active)
        {
            katie_animator_in_scene.SetTrigger("Stretch");
        }
        //_characterAnimator.SetTrigger("Kick");
    }

    private void SquatCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            katie_animator_in_scene.SetTrigger("Squat");
        }
        //_characterAnimator.SetTrigger("Kick");
    }

    //nima
    private void CapoeiraCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            nima_animator_in_scene.SetTrigger("Capoeira");
        }
        //_characterAnimator.SetTrigger("Capoeira");
    }

    private void SambaDanceCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            nima_animator_in_scene.SetTrigger("Samba");
        }
        //_characterAnimator.SetTrigger("Samba");
    }

    private void BowCommand(PhraseRecognizedEventArgs args)
    {
        if (ty_active)
        {
            ty_animator_in_scene.SetTrigger("Bow");
        }
        //_characterAnimator.SetTrigger("Bow");
    }
    void Update()
    {
        nima_active = nima.activeSelf;
        katie_active = katie.activeSelf;
        ty_active = ty.activeSelf;
    }
}