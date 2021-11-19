namespace EnvDev
{
    public interface IView<TModel>
    {
        void Init(TModel model);
    }
}