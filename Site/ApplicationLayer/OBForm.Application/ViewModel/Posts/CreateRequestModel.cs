namespace OBFormPost.Application.ViewModel.Posts
{
    public sealed class CreateRequestModel
    {
        public int Status { get; init; }
        public string Title { get; init; }
        public int AuthorId { get; init; }
    }
}
