using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BkpkExample
{
    public partial class BkpkExample : MonoBehaviour
    {
        IEnumerator Init()
        {
            StartCoroutine(Test());
        }

        async Task Test() { }
    }
}
