using UnityEngine;

public class InputMatcher : MonoBehaviour
{
    private int m_PlayerCount = 0;

    [SerializeField] private InputData[] m_InputDataRefs;

    public InputData[] InputDataRefs => m_InputDataRefs;

    public static InputMatcher Instance { get; private set; }

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
    }

    public InputData GetInputData()
    {
        if (m_PlayerCount == m_InputDataRefs.Length)
            return null;

        InputData data = m_InputDataRefs[m_PlayerCount];
        m_PlayerCount++;

        return data;
    }
}
