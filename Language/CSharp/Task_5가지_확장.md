# .NET에서 Task<T>에 유용한 5가지 확장 | Steven Giesel
## 발생하고 잊어버리기
> 작업을 시작하고 싶지만 완료될 때까지 기다리기 싫은경우 사용하기 좋다.  

이 기능은 작업을 시작하고 싶지만 결과에 신경쓰지 않을떄 유용하다. 예를 들어 이메일을 보내는 작업을 시작하는 경우, 이메일이 전송될 때까지 기다렸다가 모두 전송이 되고 작업을 이어하고 싶지 않을것이다. 따라서 `FireAndForget` 확장 메서드를 사용하여 작업을 잊어버릴수 있다. 선택적으로 메서드에 오류 처리기를 전달할 수 있다. 이 오류처리기는 작업에서 예외가 발생시 호출 됩니다.
```c#
public static void FireAndForget(
  this Task task,
  Action<Exception> errorHandler = null)
{
    task.ContinueWith(t =>
    {
        if (t.IsFaulted && errorHandler != null)
            errorHandler(t.Exception);
    }, TaskContinuationOptions.OnlyOnFaulted);
}
```
사용
```c#
SendEmailAsync().FireAndForget(errorHandler => Console.WriteLine(errorHandler.Message));
```

## 재시도
특정 횟수 만큼 작업을 재시도 하려면 `Retry`확장 메서드를 사용할 수 있따. 이 방법은 작업이 성공하거나 최대 재시도 횟수에 도달 시 까지 재시도를 한다. 재시도 사이에 지연시간을 전달할수 있으며 이 지연은 각 재시도 사이에 사용이 된다.
* TResult
  * 코드의 비동기 실행 결과를 주는 Task\<TResult> 
  * TResult 형식의 결과를 반환해준다.
```c#
public static async Task<TResult> Retry<TResult>(this Func<Task<TResult>> taskFactory, int maxRetries, TimeSpan delay)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            return await taskFactory().ConfigureAwait(false);
        }
        catch
        {
            if (i == maxRetries - 1)
                throw;
            await Task.Delay(delay).ConfigureAwait(false);
        }
    }

    return default(TResult); // Should not be reached
}
```
사용
```c#
var result = awiat(()=> GetResultAsync().Retry(3,TimeSpan.FromSeconds(1)));
## OnFailure
테스크에서 예외가 발생하면 콜백 함수를 실행한다.
```c#
public static async Task OnFailure(this Task task, Action<Exception> onFailure)
{
    try
    {
        await task.ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        onFailure(ex);
    }
}
```
사용
```c#
await GetResultAsync().OnFailure(ex => Console.WriteLine(ex.Message));
```
## 타임아웃
작업간에 시간제한을 설정 해당 작업시간이 제한시간보다 오래걸릴시 작업이 취소된다.
```c#
public static async Task WithTimeout(this Task task, TimeSpan timeout)
{
    var delayTask = Task.Delay(timeout);
    var completedTask = await Task.WhenAny(task, delayTask);
    if (completedTask == delayTask)
        throw new TimeoutException();

    await task;
}
```
사용 :
```
await GetResultAsync().WithTimeout(TimeSpan.FromSeconds(1));
```
## 대체
작업이 실패할 떄 대체 값을 사용하려는 경우가 있다 . Fallback 확장 메서드를 사용하여 작업 실패 시 폴백 값을 사용할 수 있습니다.
```c#
public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
{
    try
    {
        return await task.ConfigureAwait(false);
    }
    catch
    {
        return fallbackValue;
    }
}
```
사용:
```
var result = await GetResultAsync().Fallback("fallback");
```