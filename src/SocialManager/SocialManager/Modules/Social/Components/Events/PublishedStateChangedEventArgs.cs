namespace SocialManager.Modules.Social.Components.Events;


public class PublishedStateChangedEventArgs : EventArgs
{
    public DateTimeOffset? PublishDate { get; }
    public bool IsReady { get; }
    public PublishedStateChangedEventArgs(DateTimeOffset? publishDate, bool isReady)
    {
        PublishDate = publishDate;
        IsReady = isReady;
    }

}
