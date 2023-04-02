namespace testTask.Healper
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int id, Role role = Role.User);
    }
}
