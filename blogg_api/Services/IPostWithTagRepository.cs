namespace blogg_api.Services
{
    public interface IPostWithTagRepository<T>
    {
        Task<IEnumerable<T>> GetPostsWithTagsAsync();
        Task<IEnumerable<T>> GetPostsByTagAsync(int tagId);
    }
}
