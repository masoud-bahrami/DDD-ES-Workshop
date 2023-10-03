public class Post
{
    private String PostId;
    private String auth;
    private String content;
    private int lks;

    public Post(String postId, String author, String content)
    {
        PostId = postId;
        auth = author;
        this.content = content;
        lks = 0;
    }

    public void like()
    {
    }

    public String getAuthor()
    {
        return auth;
    }

    
}