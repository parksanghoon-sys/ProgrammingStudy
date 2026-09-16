# C# SIMD로 단일 코어의 성능을 극대화하기

## 들어가며

많은 개발자들이 "병렬처리"라고 하면 멀티스레딩이나 멀티코어 활용을 떠올립니다. 하지만 단일 코어 안에서도 강력한 병렬처리가 가능하다는 사실을 알고 계신가요? 바로 **SIMD(Single Instruction, Multiple Data)**를 통해서입니다.

이 글에서는 C#에서 SIMD를 활용하여 단일 코어에서도 극적인 성능 향상을 얻는 방법을 살펴보겠습니다.

## SIMD란 무엇인가?

SIMD는 하나의 명령어로 여러 데이터를 동시에 처리하는 CPU의 명령어 집합입니다. 예를 들어, 4개의 숫자를 더할 때 일반적인 방법은 다음과 같습니다:

```
result[0] = a[0] + b[0]
result[1] = a[1] + b[1]
result[2] = a[2] + b[2]
result[3] = a[3] + b[3]
```

하지만 SIMD를 사용하면 이 4개의 덧셈을 **단 하나의 CPU 명령어**로 처리할 수 있습니다. 이것이 바로 단일 코어 내에서의 병렬처리입니다.

## C#에서 SIMD 사용하기

C#에서는 `System.Numerics` 네임스페이스의 `Vector<T>` 타입과 `System.Runtime.Intrinsics` 네임스페이스를 통해 SIMD를 활용할 수 있습니다.

### 1. Vector<T>를 이용한 기본 사용법

가장 간단한 방법은 `Vector<T>` 타입을 사용하는 것입니다:

```csharp
using System.Numerics;

public class VectorExample
{
    public static void AddArrays(float[] a, float[] b, float[] result)
    {
        int vectorSize = Vector<float>.Count; // CPU에 따라 4, 8, 16 등
        int i = 0;

        // SIMD 처리
        for (; i <= a.Length - vectorSize; i += vectorSize)
        {
            var va = new Vector<float>(a, i);
            var vb = new Vector<float>(b, i);
            var vr = va + vb;
            vr.CopyTo(result, i);
        }

        // 나머지 처리
        for (; i < a.Length; i++)
        {
            result[i] = a[i] + b[i];
        }
    }
}
```

### 2. 실전 예제: 이미지 밝기 조절

이미지 처리는 SIMD의 효과를 체감하기 좋은 예제입니다:

```csharp
using System.Numerics;

public class ImageProcessor
{
    public static void AdjustBrightness(byte[] pixels, float factor)
    {
        int vectorSize = Vector<float>.Count;
        int i = 0;
        
        var factorVector = new Vector<float>(factor);
        var maxVector = new Vector<float>(255.0f);
        var minVector = Vector<float>.Zero;

        for (; i <= pixels.Length - vectorSize; i += vectorSize)
        {
            // byte를 float로 변환
            var values = new float[vectorSize];
            for (int j = 0; j < vectorSize; j++)
                values[j] = pixels[i + j];

            var vector = new Vector<float>(values);
            
            // 밝기 조절
            vector = vector * factorVector;
            
            // 범위 제한 (0-255)
            vector = Vector.Min(maxVector, Vector.Max(minVector, vector));

            // 다시 byte로 변환
            for (int j = 0; j < vectorSize; j++)
                pixels[i + j] = (byte)vector[j];
        }

        // 나머지 처리
        for (; i < pixels.Length; i++)
        {
            float value = pixels[i] * factor;
            pixels[i] = (byte)Math.Clamp(value, 0, 255);
        }
    }
}
```

### 3. 더 강력한 제어: Intrinsics 활용

더 세밀한 제어가 필요하다면 `System.Runtime.Intrinsics`를 사용할 수 있습니다:

```csharp
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

public class IntrinsicsExample
{
    public static unsafe void MultiplyArrays(float[] a, float[] b, float[] result)
    {
        if (!Avx.IsSupported)
        {
            // Fallback to scalar code
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] * b[i];
            return;
        }

        int i = 0;
        fixed (float* pA = a, pB = b, pResult = result)
        {
            // AVX는 256비트 = 8개의 float 처리
            for (; i <= a.Length - 8; i += 8)
            {
                Vector256<float> va = Avx.LoadVector256(pA + i);
                Vector256<float> vb = Avx.LoadVector256(pB + i);
                Vector256<float> vr = Avx.Multiply(va, vb);
                Avx.Store(pResult + i, vr);
            }
        }

        // 나머지 처리
        for (; i < a.Length; i++)
            result[i] = a[i] * b[i];
    }
}
```

## 성능 비교

실제로 얼마나 빠를까요? 간단한 벤치마크를 만들어봅시다:

```csharp
using System.Diagnostics;
using System.Numerics;

public class Benchmark
{
    public static void ComparePerformance()
    {
        const int size = 10_000_000;
        var a = new float[size];
        var b = new float[size];
        var result = new float[size];
        
        var random = new Random(42);
        for (int i = 0; i < size; i++)
        {
            a[i] = (float)random.NextDouble();
            b[i] = (float)random.NextDouble();
        }

        // 일반적인 방법
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < size; i++)
            result[i] = a[i] + b[i];
        sw.Stop();
        Console.WriteLine($"스칼라 연산: {sw.ElapsedMilliseconds}ms");

        // SIMD 방법
        sw.Restart();
        int vectorSize = Vector<float>.Count;
        int i2 = 0;
        for (; i2 <= size - vectorSize; i2 += vectorSize)
        {
            var va = new Vector<float>(a, i2);
            var vb = new Vector<float>(b, i2);
            var vr = va + vb;
            vr.CopyTo(result, i2);
        }
        sw.Stop();
        Console.WriteLine($"SIMD 연산: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine($"속도 향상: {(double)sw.ElapsedMilliseconds}배");
    }
}
```

**실행 결과 예시:**
```
스칼라 연산: 25ms
SIMD 연산: 4ms
속도 향상: 6.25배
```

## 언제 SIMD를 사용해야 할까?

SIMD가 효과적인 경우:

1. **대량의 수치 데이터 처리**: 배열, 행렬 연산
2. **이미지/비디오 처리**: 픽셀 단위 연산
3. **물리 시뮬레이션**: 벡터 연산이 많은 경우
4. **신호 처리**: FFT, 필터링 등
5. **머신러닝 추론**: 행렬 곱셈, 활성화 함수

SIMD가 비효과적인 경우:

1. 데이터 크기가 너무 작을 때
2. 조건 분기가 많은 로직
3. 데이터가 메모리에 연속되지 않을 때
4. 벡터화하기 어려운 복잡한 알고리즘

## 실무 활용 팁

### 1. 데이터 정렬(Alignment)

최적의 성능을 위해 데이터를 메모리에 정렬하세요:

```csharp
// 32바이트 정렬
[StructLayout(LayoutKind.Sequential, Pack = 32)]
public struct AlignedData
{
    public float Value;
}
```

### 2. CPU 기능 확인

런타임에 CPU가 지원하는 SIMD 명령어를 확인하세요:

```csharp
if (Avx2.IsSupported)
{
    // AVX2 코드
}
else if (Sse2.IsSupported)
{
    // SSE2 코드
}
else
{
    // 일반 코드
}
```

### 3. 컴파일러 최적화 활용

Release 모드로 빌드하고 적절한 플랫폼 타겟을 설정하세요:

```xml
<PropertyGroup>
    <PlatformTarget>x64</PlatformTarget>
    <Optimize>true</Optimize>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
</PropertyGroup>
```

### 4. 프로파일링

실제 성능 향상을 측정하세요. 때로는 메모리 대역폭이 병목이 될 수 있습니다:

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class SIMDBenchmark
{
    private float[] data;
    
    [GlobalSetup]
    public void Setup()
    {
        data = new float[1000];
    }
    
    [Benchmark(Baseline = true)]
    public void ScalarAdd() { /* ... */ }
    
    [Benchmark]
    public void VectorAdd() { /* ... */ }
}
```

## 주의사항

1. **크로스 플랫폼**: ARM 프로세서는 다른 SIMD 명령어(NEON)를 사용합니다
2. **가독성**: SIMD 코드는 복잡할 수 있으므로 주석을 충분히 작성하세요
3. **유지보수**: 성능이 중요한 핵심 부분에만 적용하세요
4. **테스트**: SIMD 코드는 버그가 생기기 쉬우므로 철저히 테스트하세요

## 마치며

SIMD는 멀티스레딩 없이도 단일 코어에서 극적인 성능 향상을 제공합니다. C#에서는 `Vector<T>`를 통해 쉽게 시작할 수 있으며, 더 세밀한 제어가 필요하면 Intrinsics를 사용할 수 있습니다.

대량의 수치 연산을 처리하는 애플리케이션을 개발한다면, SIMD는 반드시 고려해야 할 최적화 기법입니다. 특히 게임, 이미지 처리, 과학 계산 분야에서는 필수적인 기술이 되었습니다.

여러분의 프로젝트에 SIMD를 적용해보고, 단일 코어의 진정한 성능을 경험해보세요!

---

**참고 자료:**
- [Microsoft Docs: System.Numerics](https://docs.microsoft.com/dotnet/api/system.numerics)
- [Hardware Intrinsics in .NET Core](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/)
- [Intel Intrinsics Guide](https://software.intel.com/sites/landingpage/IntrinsicsGuide/)