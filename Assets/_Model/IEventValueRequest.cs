using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventValueRequest
{
    void SetEventValue(E_ValueIdentifer valueIdentifer, object obj);
}
