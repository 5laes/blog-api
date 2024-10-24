namespace blogg_api.Services
{
    public interface ISinglePostRepository<T>
    {
        Task<IEnumerable<T>> GetPostWithTagsAsync(int postId);
    }
}
