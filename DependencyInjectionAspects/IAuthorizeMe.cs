namespace DependencyInjectionAspects
{
    public interface IAuthorizeMe
    {
        [AuthorizeRoles(AuthRoles.Normal)]
        string IAuthed(int userId);
        [AuthorizeRoles(AuthRoles.Super)]
        string INotAuthed(int userId);
    }
}