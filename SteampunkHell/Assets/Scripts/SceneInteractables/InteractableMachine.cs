using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableMachine
{
    void PlayerEnter(Model model);
    void PlayerOnTrigger(Model model, bool on);
}
