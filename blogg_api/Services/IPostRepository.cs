namespace blogg_api.Services
{
    public interface IPostRepository<T>
    {
        Task<IEnumerable<T>> GetPostWithTagsAsync(int postId);
        Task<T> RemoveTagAsync(int postId, int tagId);
    }
}
