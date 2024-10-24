namespace blogg_api.Services
{
    public interface IPostRepository<T>
    {
        Task<T> RemoveTagAsync(int postId, int tagId);
    }
}
