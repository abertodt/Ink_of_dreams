public abstract class State<T>
{
    protected T context;

    public State(T context)
    {
        this.context = context;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
