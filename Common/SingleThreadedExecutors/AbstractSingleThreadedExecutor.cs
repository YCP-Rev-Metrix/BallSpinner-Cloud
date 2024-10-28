using System.Collections.Concurrent;

namespace Common.SingleThreadedExecutors;

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1> : IDisposable
{
    protected readonly BlockingCollection<T1> Tasks = new();

    public void Add(T1 t1) => Tasks.Add(t1);

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach (T1 t1 in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2)> Tasks = new();

    public void Add(T1 t1, T2 t2) => Tasks.Add((t1, t2));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3) => Tasks.Add((t1, t2, t3));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4) => Tasks.Add((t1, t2, t3, t4));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => Tasks.Add((t1, t2, t3, t4, t5));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => Tasks.Add((t1, t2, t3, t4, t5, t6));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15);
}

/// <summary>
/// Generates a new thread that executes tasks added to this object by invoking abstract Invoke()
/// </summary>
public abstract class AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IDisposable
{
    protected readonly BlockingCollection<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Tasks = new();

    public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) => Tasks.Add((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));

    protected AbstractSingleThreadedExecutor() => _ = Task.Factory.StartNew(Execute);

    private void Execute()
    {
        foreach ((T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) in Tasks.GetConsumingEnumerable())
        {
            Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }
    }

    public void Dispose()
    {
        Tasks.CompleteAdding();
        GC.SuppressFinalize(this);
    }

    ~AbstractSingleThreadedExecutor() => Dispose();

    protected abstract void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16);
}