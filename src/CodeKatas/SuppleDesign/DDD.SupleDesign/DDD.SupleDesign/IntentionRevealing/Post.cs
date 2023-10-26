public class Post
{
    private string PostId;
    private string auth;
    private string content;
    private int lks;

    public Post(string postId, string author, string content)
    {
        PostId = postId;
        auth = author;
        this.content = content;
        lks = 0;
    }

    public void like()
    {
    }

    public string getAuthor()
    {
        return auth;
    }

    
}