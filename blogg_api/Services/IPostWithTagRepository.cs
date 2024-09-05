namespace blogg_api.Services
{
    public interface IPostWithTagRepository<T>
    {
        Task<IEnumerable<T>> GetPostsWithTagsAsync();
    }
}
