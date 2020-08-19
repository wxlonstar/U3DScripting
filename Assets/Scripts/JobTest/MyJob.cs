using Unity.Collections;
using Unity.Jobs;

public class MyJob : IJob {
    public float a;
    public float b;
    public NativeArray<float> result;
    public void Execute() {
        result[0] = a + b;
    }
}

public struct AddOneJob : IJob {
    public NativeArray<float> result;
    public void Execute() {
        result[0] = result[0] + 1;
    }
}

