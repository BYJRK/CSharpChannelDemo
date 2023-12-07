# Channel Demo

本项目旨在演示 C# 中 `Channel` 的基本用法。

查看 commit 提交历史可以看到每一步的变化：

1. 使用传统的线程（`Thread`）与阻塞集合（`BlockingCollection`）
2. 将阻塞集合修改为 `Channel`，并仍然使用同步方法
3. 使用异步方法，并使用 `CancellationToken` 来取消接收任务
4. 使用 `ChannelWriter.Complete` 方法取代 `CancellationToken`
5. 使用 `await foreach`，即 `IAsyncEnumerable` 接口取代对于 `Completed` 状态的观察及 `ChannelClosedException` 异常的捕获