namespace MessiFinder.Services.Games
{
    using Models.Games;

    public interface IGameService
    {
        void Create(GameCreateFormModel gameCreateModel, int adminId);

        GameAllQueryModel All(GameAllQueryModel query);
    }
}
