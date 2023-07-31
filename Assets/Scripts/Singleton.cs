using Scriptable_Objects;
using Synth.Modules.ADSR;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    [SerializeField] private SynthController synthController;
    [SerializeField] private TelephoneSettings telephoneSettings;
    [SerializeField] private CheatCodes cheatCodes;
    [SerializeField] private AudioFx audioFx;
    [SerializeField] private TerminalTextSettings terminalTextSettings;
    
    public static Singleton Instance { get; private set; }
    public SynthController SynthController => synthController;
    public TelephoneSettings TelephoneSettings => telephoneSettings;
    public CheatCodes CheatCodes => cheatCodes;
    public TerminalTextSettings TextSettings => terminalTextSettings;
    public AudioFx AudioFx => audioFx;
    
    public RealTimeCv RealTimeCv { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Singleton in the scene");
            return;
        }
        Instance = this;
        SynthController.ResetToInit();
        terminalTextSettings.Setup();
    }

    // private void Start()
    // {
    //     if (RealTimeCv == null)
    //     {
    //         RealTimeCv = ScriptableObject.CreateInstance<RealTimeCv>();
    //         ReaktorController.Instance.Receiver.Bind("/ADSR/CVOUT", RealTimeCv.Listen);
    //     }
    // }
}
