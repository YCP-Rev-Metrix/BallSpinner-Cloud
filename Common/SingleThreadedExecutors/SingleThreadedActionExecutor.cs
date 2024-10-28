namespace Common.SingleThreadedExecutors;

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1> : AbstractSingleThreadedExecutor<T1>
{
    private readonly Action<T1> Action;
    public SingleThreadedActionExecutor(Action<T1> action) : base() => Action = action;
    protected override void Invoke(T1 t1) => Action.Invoke(t1);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2> : AbstractSingleThreadedExecutor<T1, T2>
{
    private readonly Action<T1, T2> Action;

    public SingleThreadedActionExecutor(Action<T1, T2> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2) => Action.Invoke(t1, t2);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3> : AbstractSingleThreadedExecutor<T1, T2, T3>
{
    private readonly Action<T1, T2, T3> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3) => Action.Invoke(t1, t2, t3);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4> : AbstractSingleThreadedExecutor<T1, T2, T3, T4>
{
    private readonly Action<T1, T2, T3, T4> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4) => Action.Invoke(t1, t2, t3, t4);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5>
{
    private readonly Action<T1, T2, T3, T4, T5> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => Action.Invoke(t1, t2, t3, t4, t5);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6>
{
    private readonly Action<T1, T2, T3, T4, T5, T6> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => Action.Invoke(t1, t2, t3, t4, t5, t6);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
}

/// <summary>
/// Executes an action on added tasks using only one thread
/// </summary>
public class SingleThreadedActionExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : AbstractSingleThreadedExecutor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Action;

    public SingleThreadedActionExecutor(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) : base() => Action = action;

    protected override void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) => Action.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
}