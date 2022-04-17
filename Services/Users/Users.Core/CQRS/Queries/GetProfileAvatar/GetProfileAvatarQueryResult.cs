namespace Users.Core.CQRS.Queries.GetProfileAvatar;

public class GetProfileAvatarQueryResult
{
    public Stream AvatarStream { get; init; }

    public string Extension { get; init; }
}