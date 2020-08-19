using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MainThreadTest : MonoBehaviour {
    // setup data
    NativeArray<float> reulst = new NativeArray<float>(1, Allocator.TempJob);

    public void Fun() {
        MyJob jobData = new MyJob();
        jobData.a = 10;
        jobData.b = 20;
        jobData.result = this.reulst;
    }
}
