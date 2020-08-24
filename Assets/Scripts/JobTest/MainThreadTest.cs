using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

public class MainThreadTest : MonoBehaviour {
    [SerializeField]
    private bool useJob;

    private void Update() {
        float startTime = Time.realtimeSinceStartup;
        if(useJob) {
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for(int i = 0; i < 100; i++) {
                JobHandle jobHandle = DoCommonJob();
                jobHandleList.Add(jobHandle);
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        } else {
            for(int i = 0; i < 100; i++) {
                CommonTask();
            }
            
        }
        Debug.Log((Time.realtimeSinceStartup - startTime) * 1000f);
    }

    private void CommonTask() {
        float value = 0f;
        for(int i = 0; i < 50000; i++) {
            math.exp10(math.sqrt(value));
        }
    }

    private JobHandle DoCommonJob() {
        CommonJob commonTaskJob = new CommonJob();
        return commonTaskJob.Schedule();
    }
}

[BurstCompile]
public struct CommonJob : IJob {
    public void Execute() {
        float value = 0f;
        for(int i = 0; i < 50000; i++) {
            math.exp10(math.sqrt(value));
        }
    }
}
