using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeObjectState : MonoBehaviour
{
    public string ID;
    public bool StartNormalState = true;
    public string NormalStateSound;
    public List<GameObject> NormalStateObjects;
    public string SecondaryStateSound;
    public List<GameObject> SecondaryStateObjects;

    private void OnEnable()
    {
        GameEvents.OnChangeObjectState += ChangeState;
        if (StartNormalState)
            SetNormalState();
        else
            SetSecondaryState();
    }
    private void OnDisable()
    {
        GameEvents.OnChangeObjectState -= ChangeState;
    }

    private void ChangeState(string id)
    {
        if(id == ID)
        {
            if (StartNormalState)
                SetSecondaryState();
            else
                SetNormalState();
        }
    }

    public void SetNormalState()
    {
        SecondaryStateObjects.ForEach(x => x.SetActive(false));
        NormalStateObjects.ForEach(x => x.SetActive(true));

        if (!string.IsNullOrEmpty(NormalStateSound))
            GameEvents.PlaySound(NormalStateSound);
    }
    public void SetSecondaryState()
    {
        NormalStateObjects.ForEach(x => x.SetActive(false));
        SecondaryStateObjects.ForEach(x => x.SetActive(true));

        if (!string.IsNullOrEmpty(SecondaryStateSound))
            GameEvents.PlaySound(SecondaryStateSound);
    }
}
